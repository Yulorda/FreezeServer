using System;
using System.Collections.Generic;
using System.Threading;
using Telepathy;
using Serializator;

public class NetworkServer
{
    private IServer server;
    private ISerializator serializator;

    private List<int> connectedClients = new List<int>();
    private GenericListeners listeners = new GenericListeners();

    private Thread updateThread;

    public event Action<int> onClientConnected;
    public event Action<int> onClientDisconnected;

    public bool IsAviable => server.IsAvailable;

    public NetworkServer(IServer server, ISerializator serializator)
    {
        this.server = server;
        this.serializator = serializator;
    }

    public void AddListener<U>(Action<int, U> handler) where U : class
    {
        listeners.AddListener(handler);
    }

    public void RemoveListener<U>(Action<int, U> handler) where U : class
    {
        listeners.RemoveListener(handler);
    }

    public bool Start()
    {
        connectedClients = new List<int>();

        if (server.Start())
        {
            updateThread = new Thread(Update);
            updateThread.Start();
            return true;
        }

        return false;
    }

    public void Broadcast(object package)
    {
        var message = serializator.Serialize(package);
        foreach (var client in connectedClients)
        {
            server.Send(client, message);
        }
    }

    public bool Send(int connectionId, object package)
    {
        var message = serializator.Serialize(package);
        return server.Send(connectionId, message);
    }

    public void Stop()
    {
        updateThread.Abort();
        server.Stop();
    }

    private void Update()
    {
        while (server.IsAvailable)
        {
            Thread.Sleep(1);
            UpdateInfo();
        }

        Logger.LogError("UpdateInfo Exception, server is not available");
    }

    private void UpdateInfo()
    {
        while (server.GetNextEvent(out StateMessage msg))
        {
            switch (msg.eventType)
            {
                case EventType.Connected:
                    OnClientConnected(msg.connectionId);
                    break;

                case EventType.Disconnected:
                    OnClientDisconnected(msg.connectionId);
                    break;
            }
        }

        while (server.GetNextPackage(out NetworkPackageData pkg))
        {
            OnDataReceive(pkg);
        }
    }

    private void OnClientConnected(int connectionId)
    {
        Logger.Log(connectionId.ToString(), "ClientConnected");
        connectedClients.Add(connectionId);
        onClientConnected?.Invoke(connectionId);
    }

    private void OnClientDisconnected(int connectionId)
    {
        Logger.Log(connectionId.ToString(), "ClientDisconnected");
        connectedClients.Remove(connectionId);
        onClientDisconnected?.Invoke(connectionId);
    }

    private void OnDataReceive(NetworkPackageData pkg)
    {
        if (serializator.TryDeserialize(pkg.data, out var message))
        {
            listeners.Invoke(pkg.connectionId, message, message.GetType());
        }
    }
}