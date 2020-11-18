using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace LeyendsServer
{
    public class GameRoom
    {
        public int id { get; set; }
        public bool isSet { get; set; }
        public int port { get; set; }
        public int groupSize { get; set; }
        public int playersInGroup { get; set; }
        //public List<QueueGroup> players { get; set; }
        public List<int> groupsInRoom { get; set; }
        public GameRoom(int _id, int _port)
        {
            id = _id;
            isSet = false;
            port = _port;
            groupSize = 0;
            playersInGroup = 0;
            // players = new List<QueueGroup>();
            groupsInRoom = new List<int>();
        }
        public override string ToString()
        {
            return $" [{id}] [{port}] groupSize[{groupSize}]";
        }
        // public void SetGroupMember(QueueGroup group)
        // {
        //     playersInGroup += group.GroupSize();
        //     players.Add(group);
        //     foreach (PlayerQueue item in group.groupMembers)
        //     {
        //         Server.clients[item.id].roomId = id;
        //     }
        //     group.isOnRoom = true;
        // }
        public void SetGroupMember(int _groupId)
        {
            playersInGroup += QueueManager.randomQueuesGrup[_groupId].GroupSize();
            groupsInRoom.Add(QueueManager.randomQueuesGrup[_groupId].id);
            foreach (PlayerQueue item in QueueManager.randomQueuesGrup[_groupId].groupMembers)
            {
                Server.clients[item.id].roomId = id;
            }
            QueueManager.randomQueuesGrup[_groupId].isOnRoom = true;
        }
        // public void RemoveGroupMember(QueueGroup group)
        // {
        //     playersInGroup -= group.GroupSize();
        //     players.Remove(group);
        //     foreach (PlayerQueue item in group.groupMembers)
        //     {
        //         Server.clients[item.id].roomId = 0;
        //     }
        //     group.isOnRoom = false;
        // }
        public void RemoveGroupMember(int _groupId)
        {
            playersInGroup -= QueueManager.randomQueuesGrup[_groupId].GroupSize();
            groupsInRoom.Remove(QueueManager.randomQueuesGrup[_groupId].id);
            foreach (PlayerQueue item in QueueManager.randomQueuesGrup[_groupId].groupMembers)
            {
                Server.clients[item.id].roomId = 0;
            }
            QueueManager.randomQueuesGrup[_groupId].isOnRoom = false;
        }
        public int GroupSize()
        {
            int counter = 0;
            foreach (int item in groupsInRoom)
            {
                counter += QueueManager.randomQueuesGrup[item].GroupSize();
            }
            return counter;
        }

        public bool isReadyToCall()
        {
            return playersInGroup == groupSize;
        }

        public List<int> GroupMembers()
        {
            List<int> _members = new List<int>();
            foreach (int group in groupsInRoom)
            {
                foreach (PlayerQueue player in QueueManager.randomQueuesGrup[group].groupMembers)
                {
                    _members.Add(player.id);
                }
            }
            return _members;
        }
        public bool isGameRoomReady()
        {
            List<int> _members = new List<int>();
            foreach (int group in groupsInRoom)
            {
                foreach (PlayerQueue player in QueueManager.randomQueuesGrup[group].groupMembers)
                {
                    if (!player.queueAcepted)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        public void AceptGame(int _fromPlayer)
        {
            List<int> _members = new List<int>();
            foreach (int group in groupsInRoom)
            {
                foreach (PlayerQueue player in QueueManager.randomQueuesGrup[group].groupMembers)
                {
                    if (player.id == _fromPlayer)
                    {
                        player.queueAcepted = true;
                        return;
                    }
                }
            }
        }
        public void ResetGame()
        {
            List<int> _removeGroups = new List<int>();
            List<int> _quitQueuePlayers = new List<int>();
            foreach (int group in groupsInRoom)
            {
                Console.WriteLine($"Reseting Group {group}");
                foreach (PlayerQueue player in QueueManager.randomQueuesGrup[group].groupMembers)
                {
                    if (!player.queueAcepted)
                    {
                        _quitQueuePlayers.Add(player.id);
                        Console.WriteLine($"player {Server.clients[player.id].nickName} rejects queue");
                        _removeGroups.Add(Server.clients[player.id].groupLeader);
                        break;
                    }
                }
            }
            foreach (int item in _removeGroups)
            {
                groupsInRoom.Remove(item);
            }
            foreach (int item in _quitQueuePlayers)
            {
                ServerHandle.QuitQueueRequest(item, null);
            }
            foreach (int group in groupsInRoom)
            {
                foreach (PlayerQueue player in QueueManager.randomQueuesGrup[group].groupMembers)
                {
                    player.queueAcepted = false;
                }
            }
        }
        public void SendGame()
        {
            ServerSend.GameCall(id);
        }
    }
}

