using System;
using System.Collections.Generic;
using MongoDB.Bson;

namespace LeyendsServer
{
    class QueueManager
    {
        static List<int> _randomQueue;

        // Static constructor initializes NumMembers
        static QueueManager()
        {
            _randomQueue = new List<int>();
        }

        public static void Update()
        {
            Console.WriteLine("Queue Logic ...");
        }

        public static void Add(int _id)
        {
            _randomQueue.Add(_id);
        }
        public static void Remove(int _index)
        {
            _randomQueue.Remove(_index);
        }
        public static void QueueStatus()
        {
            int _randQueueSize = _randomQueue.Count;
            Console.WriteLine($"Random Match: {_randQueueSize}");
            Console.WriteLine($"Total: {_randQueueSize} Players");
        }
    }
}