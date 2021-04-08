using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace LeyendsServer
{
    class QueueManager
    {
        public static bool isBuilding { get; private set; }
        public static bool isCalling { get; private set; }
        public static int targetRoom { get; private set; }
        public static Dictionary<int, QueueGroup> preMadeGroups;
        public static Dictionary<int, QueueGroup> randomQueuesGrup;
        
        static QueueManager()
        {
            isBuilding = true;
            isCalling = false;
            preMadeGroups = new Dictionary<int, QueueGroup>();
            randomQueuesGrup = new Dictionary<int, QueueGroup>();
        }

        public static void Update()
        {
            if (isBuilding)
            {
                BuildNewRoom(1);// Numero de jugadores para la sala
            }
            if (isCalling) return;
            if (Server.rooms[targetRoom].isReadyToCall()) CallGame();
            if (Server.rooms[targetRoom].playersInGroup <= Server.rooms[targetRoom].groupSize &&
            QueueManager.randomQueuesGrup.Count > 0)
            {
                try
                {
                    // Console.WriteLine($"LF Members {Server.rooms[targetRoom].playersInGroup} of {Server.rooms[targetRoom].groupSize}");
                    foreach (QueueGroup item in randomQueuesGrup.Values)
                    {
                        // Console.WriteLine($"{item.id}, isOnRoom {item.isOnRoom}");
                        if (!item.isOnRoom)
                        {
                            Console.WriteLine($"{item.id} is {item.GroupSize()} members, need {Server.rooms[targetRoom].groupSize - Server.rooms[targetRoom].playersInGroup}");
                            if (item.GroupSize() <= (Server.rooms[targetRoom].groupSize - Server.rooms[targetRoom].playersInGroup))
                            {
                                Server.rooms[targetRoom].SetGroupMember(item.id);
                                return;
                            }
                        }
                    }
                }
                catch (System.Exception)
                {
                    Console.WriteLine("Coleccion modificada Update");
                }

            }
        }

        private static void BuildNewRoom(int _groupSize)
        {
            for (int i = 1; i <= Server.MaxRooms; i++)
            {
                if (!Server.rooms[i].isSet)
                {
                    Server.rooms[i].isSet = true;
                    Server.rooms[i].groupSize = _groupSize;
                    Console.WriteLine($"Add new Room {Server.rooms[i].id} of {Server.MaxRooms} Rooms");
                    Console.WriteLine(Server.rooms[i].ToString());
                    targetRoom = i;
                    isBuilding = false;
                    string filename = Path.Combine("D:\\Legends\\GameServerSln", "UnityGameServer.exe");
                    //var proc = System.Diagnostics.Process.Start(filename, "-port " + Server.rooms[i].port.ToString()+" -maxPlayers "+ _groupSize);
                    Server.rooms[i].Start();
                    return;
                }
            }
        }

        private async static void CallGame()
        {
            try
            {
                isCalling = true;
                Console.WriteLine($"Calling {isCalling}");
                /*Commands.ReadArgs("-Games");
                Commands.ReadArgs("-listGroups");
                Commands.ReadArgs("-listQueues");*/
                ServerSend.GameFoundRequest(Server.rooms[targetRoom].GroupMembers());
                //GameSend.BaseMap(Server.rooms[targetRoom].id);
                await Task.Delay(10000);
                if (Server.rooms[targetRoom].isGameRoomReady())
                {
                    Console.WriteLine("Todos aceptaron");
                    Server.rooms[targetRoom].SendGame();
                    await Task.Delay(10000);
                    foreach (int item in Server.rooms[targetRoom].groupsInRoom)
                    {
                        randomQueuesGrup.Remove(item);
                    }
                    QueueManager.isBuilding = true;
                }
                else
                {
                    Server.rooms[targetRoom].ResetGame();
                }
                isCalling = false;
                Console.WriteLine($"CAlling {isCalling}");
            }
            catch (System.Exception)
            {

                Console.WriteLine("Coleccion modificada CallGame");
            }

        }
    }
}