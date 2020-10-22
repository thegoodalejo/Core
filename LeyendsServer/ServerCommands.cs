using System;
using MongoDB.Bson;
namespace LeyendsServer
{
    class ServerCommands

    {
        public static void ReadArgs(string _command)
        {
            switch (_command)
            {
                case "-exit":
                    Console.WriteLine("Shutdown server");
                    DbManager.UpdateStateAll();
                    Environment.Exit(0);
                    break;
                case "-clear":
                    Console.Clear();
                    break;
                case "-listAll":
                int players = 0;
                    foreach (Client _client in Server.clients.Values)
                    {
                        if (_client.token != ObjectId.Empty)
                        {
                            Console.WriteLine(_client.ToString());
                            players++;
                        }
                    }
                    Console.WriteLine($"Total Players: {players}");
                    break;
                case "-listAllQueue":
                QueueManager.QueueStatus();
                    foreach (Client _client in Server.clients.Values)
                    {
                        if (_client.token != ObjectId.Empty && _client.queueStatus)
                            Console.WriteLine(_client.ToString());
                    }
                    break;
                default:
                    Console.WriteLine("Wrong argument");
                    break;
            }
        }
    }
}