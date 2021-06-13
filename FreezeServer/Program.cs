using Serializator;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading;

namespace FreezeServer
{
    internal class Program
    {
        private static NetworkServer networkServer;
        private static Game game;

        private static void Main(string[] args)
        {
            game = new Game();
            networkServer = new NetworkServer(new Telepathy.TelepathyServer(4004), new JSONSerializator());
            networkServer.Start();
            networkServer.onClientConnected += OnClientConnedcted;
            networkServer.AddListener<MoveTask>(OnPackageReceive);

            while (true)
            {
                Thread.Sleep(50);
                game.Do(networkServer);
            }
        }

        private static void OnClientConnedcted(int id)
        {
            networkServer.Send(id, new BoardProperty() { rank = game.boardRank });
            foreach (var unit in game.unitMoveStates.Values)
            {
                networkServer.Send(id, new UnitLifeState() { lifeStatus = UnitLifeStatus.Create, id = unit.id, unitState = unit });
            }
        }

        private static void OnPackageReceive(int id, MoveTask moveTask)
        {
            game.SetDestenition(moveTask);
        }
    }
}