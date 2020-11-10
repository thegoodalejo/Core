using System;
using System.Collections.Generic;
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
            isBuilding = false;
            isCalling = false;
            preMadeGroups = new Dictionary<int, QueueGroup>();
            randomQueuesGrup = new Dictionary<int, QueueGroup>();
        }

        public static void Update()
        {
            if (!isBuilding)
            {
                BuildNewRoom();
            }
            if (isCalling) return;
            if (Server.rooms[targetRoom].isReadyToCall()) CallGame();
            // Console.WriteLine($"Not isCalling, Pointing {targetRoom}");
            if (Server.rooms[targetRoom].playersInGroup <= Server.rooms[targetRoom].groupSize &&
            QueueManager.randomQueuesGrup.Count > 0)
            {
                Console.WriteLine($"LF Members {Server.rooms[targetRoom].playersInGroup} of {Server.rooms[targetRoom].groupSize}");
                foreach (QueueGroup item in randomQueuesGrup.Values)
                {
                    Console.WriteLine($"{item.id}, isOnRoom {item.isOnRoom}");
                    if (!item.isOnRoom)
                    {
                        Console.WriteLine($"{item.id} is {item.GroupSize()} members, need {Server.rooms[targetRoom].groupSize - Server.rooms[targetRoom].playersInGroup}");
                        if (item.GroupSize() <= (Server.rooms[targetRoom].groupSize - Server.rooms[targetRoom].playersInGroup))
                        {
                            Server.rooms[targetRoom].SetGroupMember(item);
                            return;
                        }
                    }
                }
            }
        }

        private static void BuildNewRoom()
        {
            for (int i = 1; i <= Server.MaxRooms; i++)
            {
                if (!Server.rooms[i].isSet)
                {
                    Server.rooms[i].isSet = true;
                    Server.rooms[i].groupSize = 2;
                    Console.WriteLine("Add new Room");
                    targetRoom = i;
                    isBuilding = true;
                    return;
                }
            }
        }

        private async static void CallGame()
        {
            isCalling = true;
            Console.WriteLine($"CAlling {isCalling}");
            ServerSend.GameFoundRequest(Server.rooms[targetRoom].GroupMembers());
            await Task.Delay(10000);
            if(Server.rooms[targetRoom].isGameRoomReady()){
                Console.WriteLine("Todos aceptaron");
            }
            isCalling = false;
            Console.WriteLine($"CAlling {isCalling}");
        }
    }
}