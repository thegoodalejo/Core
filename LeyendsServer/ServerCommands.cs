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
                case "-listGames":
                    int games = 0;
                    foreach (GameRoom _room in Server.rooms.Values)
                    {
                        if (_room.isSet)
                        {
                            Console.WriteLine(_room.ToString());
                            games++;
                        }
                    }
                    Console.WriteLine($"Total Rooms: {games}");
                    break;
                case "-listAllQueue":
                    int queueSize = QueueManager.randomQueuesGrup.Count;
                    Console.WriteLine($"Queues : {queueSize}");
                    foreach (KeyValuePair<int, QueueGroup> entry in QueueManager.randomQueuesGrup)
                    {
                        Console.WriteLine("Group Leader is Player: " + Server.clients[entry.Key].nickName);
                        foreach (PlayerQueue item in entry.Value.groupMembers)
                        {
                            Console.WriteLine("Player: " + Server.clients[item.id].nickName);
                        }
                    }
                    break;
                case "-listAllGroup":
                    int groupSize = QueueManager.preMadeGroups.Count;
                    Console.WriteLine($"Groups : {groupSize}");
                    foreach (KeyValuePair<int, QueueGroup> entry in QueueManager.preMadeGroups)
                    {
                        Console.WriteLine("Group Leader is Player: " + Server.clients[entry.Key].nickName);
                        foreach (PlayerQueue item in entry.Value.groupMembers)
                        {
                            Console.WriteLine("Player: " + Server.clients[item.id].nickName);
                        }
                    }
                    break;
                case "-test":
                    Console.WriteLine("Test");
                    break;
                default:
                    Console.WriteLine("Wrong argument");
                    break;
            }
        }
    }
}