using System;
using System.Collections.Generic;

namespace LeyendsServer
{
    public class GameRoom
    {
        public int id { get; set; }
        public bool isSet { get; set; }
        public int port { get; set; }
        public int groupSize { get; set; }
        public int playersInGroup { get; set; }
        public List<QueueGroup> players { get; set; }
        public GameRoom(int _id, int _port)
        {
            id = _id;
            isSet = false;
            port = _port;
            groupSize = 0;
            playersInGroup = 0;
            players = new List<QueueGroup>();
        }
        public override string ToString()
        {
            return $" [{id}] [{port}] groupSize[{groupSize}]";
        }
        public void SetGroupMember(QueueGroup group)
        {
            playersInGroup += group.GroupSize();
            players.Add(group);
            foreach (PlayerQueue item in group.groupMembers)
            {
                Server.clients[item.id].roomId = id;
            }
            group.isOnRoom = true;
        }
        public void RemoveGroupMember(QueueGroup group)
        {
            playersInGroup -= group.GroupSize();
            players.Remove(group);
            foreach (PlayerQueue item in group.groupMembers)
            {
                Server.clients[item.id].roomId = 0;
            }
            group.isOnRoom = false;
        }
        public int GroupSize()
        {

            return players.Count;
        }

        public bool isReadyToCall()
        {
            return playersInGroup == groupSize;
        }

        public List<int> GroupMembers()
        {
            List<int> _members = new List<int>();
            foreach (QueueGroup group in players)
            {
                foreach (PlayerQueue player in group.groupMembers)
                {
                    _members.Add(player.id);
                }
            }
            return _members;
        }
        public bool isGameRoomReady()
        {
            List<int> _members = new List<int>();
            foreach (QueueGroup group in players)
            {
                foreach (PlayerQueue player in group.groupMembers)
                {
                    if (!player.queueAcepted)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}

