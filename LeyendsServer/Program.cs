﻿using System;
using System.Threading;
using MongoDB.Bson;
using MongoDB.Driver;

namespace LeyendsServer
{
    class Program
    {
        private static bool isRunning = false;

        static void Main(string[] args)
        {
            Console.Title = "Game Server";
            isRunning = true;
            //Console.WriteLine(DbManager.Auth("Alejo","abc"));
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
