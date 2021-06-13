public interface IServer
{
    bool IsAvailable { get; }

    bool Start();

    bool Send(int user, byte[] message);

    void Stop();

    bool GetNextEvent(out StateMessage msg);

    bool GetNextPackage(out NetworkPackageData msg);
}