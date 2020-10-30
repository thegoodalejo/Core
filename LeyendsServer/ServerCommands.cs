using System;
using System.Collections.Generic;
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
                case "-listAllGroups":
                    int queueSize = QueueManager.preMadeGroups.Count;
                    Console.WriteLine($"Groups size : {queueSize}");
                    foreach(KeyValuePair<int, QueueGroup> entry in QueueManager.preMadeGroups)
                    {
                        Console.WriteLine("Group Leader is Player: " + entry.Key);
                        foreach (PlayerQueue item in entry.Value.groupMembers)
                        {
                            Console.WriteLine("Player: " + item.id);
                        }
                    }
                    break;
                case "-test":
                    DbManager.FriendList(1);
                    break;
                default:
                    Console.WriteLine("Wrong argument");
                    break;
            }
        }
    }
}