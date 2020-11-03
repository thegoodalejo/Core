using System;
using System.Collections.Generic;
using MongoDB.Bson;

namespace LeyendsServer
{
    class QueueManager
    {
        public static Dictionary<int, QueueGroup> preMadeGroups;
        public static Dictionary<int, QueueGroup> randomQueuesGrup;
        //static List<int> randomQueue;

        // Static constructor initializes NumMembers
        static QueueManager()
        {
            preMadeGroups = new Dictionary<int, QueueGroup>();
            randomQueuesGrup = new Dictionary<int, QueueGroup>();
            //randomQueue = new List<int>();

        }

        public static void Update()
        {
            /*if (randomQueue.Count >= 3)
            {
                foreach (int _id in randomQueue)
                {
                    Console.WriteLine($"Player: {_id}");
                }
                List<int> _newQueue = new List<int>();
                _newQueue.Add(randomQueue[0]);
                _newQueue.Add(randomQueue[1]);

                randomQueue.RemoveAt(0);
                foreach (int _id in randomQueue)
                {
                    Console.WriteLine($"--Player: {_id}");
                }
                randomQueue.RemoveAt(0);
                ServerSend.GameFoundRequest(_newQueue);
            }*/
        }

        public static void QueueStatus()
        {
            
        }
    }
}