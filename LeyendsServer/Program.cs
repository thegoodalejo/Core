using System;
using System.Threading;


namespace LeyendsServer
{
    class Program
    {
        private static bool isRunning = false;

        static void Main(string[] args)
        {
            Console.Title = "Game Server";
            isRunning = true;
            Server.Start(50, 10, 26940);
            Thread logInThread = new Thread(new ThreadStart(LoginThread));
            Thread startKeyboardListener = new Thread(new ThreadStart(StartKeyboardListener));
            Thread queueListener = new Thread(new ThreadStart(QueueListener));
            logInThread.Start();
            startKeyboardListener.Start();
            queueListener.Start();
        }

        private static void LoginThread()
        {
            DateTime _nextLoop = DateTime.Now;

            while (isRunning)
            {
                while (_nextLoop < DateTime.Now)
                {
                    // If the time for the next loop is in the past, aka it's time to execute another tick
                    ServerLogic.Update(); // Execute game logic

                    _nextLoop = _nextLoop.AddMilliseconds(Constants.MS_PER_TICK_LOGIN); // Calculate at what point in time the next tick should be executed

                    if (_nextLoop > DateTime.Now)
                    {
                        // If the execution time for the next tick is in the future, aka the server is NOT running behind
                        Thread.Sleep(_nextLoop - DateTime.Now); // Let the thread sleep until it's needed again.
                    }
                }
            }
        }

        private static void StartKeyboardListener()
        {
            DateTime _nextLoop = DateTime.Now;
            while (isRunning)
            {
                while (_nextLoop < DateTime.Now)
                {
                    // If the time for the next loop is in the past, aka it's time to execute another tick
                    ServerCommands.ReadArgs(Console.ReadLine()); // Execute game logic

                    _nextLoop = _nextLoop.AddMilliseconds(Constants.MS_PER_TICK_LOGIN); // Calculate at what point in time the next tick should be executed

                    if (_nextLoop > DateTime.Now)
                    {
                        // If the execution time for the next tick is in the future, aka the server is NOT running behind
                        Thread.Sleep(_nextLoop - DateTime.Now); // Let the thread sleep until it's needed again.
                    }
                }
            }
        }

        private static void QueueListener()
        {
            DateTime _nextLoop = DateTime.Now;
            while (isRunning)
            {
                while (_nextLoop < DateTime.Now)
                {
                    QueueManager.Update();

                    _nextLoop = _nextLoop.AddMilliseconds(Constants.MS_PER_TICK_TICKS_PER_SEC_QUEUE); // Calculate at what point in time the next tick should be executed

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
