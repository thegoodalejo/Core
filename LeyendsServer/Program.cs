﻿using System;
using System.Threading;
using MongoDB.Driver;

namespace LeyendsServer
{
    class Program
    {
        private static bool isRunning = false;

        static void Main(string[] args)
        {
            try
            {
                var connString = "mongodb://127.0.0.1:27017";
                MongoClient client = new MongoClient(connString);

                // List all the MongoDB databases
                var allDatabases = client.ListDatabases().ToList();

                Console.WriteLine("MongoDB db array type: " + allDatabases.GetType());
                Console.WriteLine("MongoDB databases:");
                Console.WriteLine(string.Join(", ", allDatabases));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error:" + ex.Message);
            }
            
            Console.Title = "Game Server";
            isRunning = true;

            Thread logInThread = new Thread(new ThreadStart(LoginThread));
            logInThread.Start();

            //26940 reservado para logica de Auth
            Server.Start(50, 26940);
        }

        private static void LoginThread()
        {
            Console.WriteLine($"Login thread started. Running at {Constants.TICKS_PER_SEC} ticks per second.");
            DateTime _nextLoop = DateTime.Now;

            while (isRunning)
            {
                while (_nextLoop < DateTime.Now)
                {
                    // If the time for the next loop is in the past, aka it's time to execute another tick
                    LoginAuth.Update(); // Execute game logic

                    _nextLoop = _nextLoop.AddMilliseconds(Constants.MS_PER_TICK); // Calculate at what point in time the next tick should be executed

                    if (_nextLoop > DateTime.Now)
                    {
                        // If the execution time for the next tick is in the future, aka the server is NOT running behind
                        Thread.Sleep(_nextLoop - DateTime.Now); // Let the thread sleep until it's needed again.
                    }
                }
            }
        }

        static void OnProcessExit()
        {
            Server.Stop();
        }
    }
}
