using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;

namespace LeyendsServer
{
    class ServerSend
    {
        /// <summary>Sends a packet to a client via TCP.</summary>
        /// <param name="_toClient">The client to send the packet the packet to.</param>
        /// <param name="_packet">The packet to send to the client.</param>
        private static void SendTCPData(int _toClient, Packet _packet)
        {
            _packet.WriteLength();
            Server.clients[_toClient].tcp.SendData(_packet);
        }

        /// <summary>Sends a packet to a client via UDP.</summary>
        /// <param name="_toClient">The client to send the packet the packet to.</param>
        /// <param name="_packet">The packet to send to the client.</param>
        private static void SendUDPData(int _toClient, Packet _packet)
        {
            _packet.WriteLength();
            Server.clients[_toClient].udp.SendData(_packet);
        }

        /// <summary>Sends a packet to all clients via TCP.</summary>
        /// <param name="_packet">The packet to send.</param>
        private static void SendTCPDataToAll(Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                Server.clients[i].tcp.SendData(_packet);
            }
        }
        /// <summary>Sends a packet to all clients except one via TCP.</summary>
        /// <param name="_exceptClient">The client to NOT send the data to.</param>
        /// <param name="_packet">The packet to send.</param>
        private static void SendTCPDataToAll(int _exceptClient, Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                if (i != _exceptClient)
                {
                    Server.clients[i].tcp.SendData(_packet);
                }
            }
        }
        /// <summary>Sends a packet to all clients in certain grup via TCP.</summary>
        /// <param name="_grupClients">The grup of clients to send the data to.</param>
        /// <param name="_packet">The packet to send.</param>
        private static void SendTCPDataToAll(List<int> _grupClients, Packet _packet)
        {
            _packet.WriteLength();
            foreach (int _id in _grupClients)
            {
                if (_id != 0)
                {
                    Server.clients[_id].tcp.SendData(_packet);
                }
                else
                {
                    Console.WriteLine("Sending data to 0 slot client ... ERROR");
                }

            }
        }

        /// <summary>Sends a packet to all clients via UDP.</summary>
        /// <param name="_packet">The packet to send.</param>
        private static void SendUDPDataToAll(Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                Server.clients[i].udp.SendData(_packet);
            }
        }
        /// <summary>Sends a packet to all clients except one via UDP.</summary>
        /// <param name="_exceptClient">The client to NOT send the data to.</param>
        /// <param name="_packet">The packet to send.</param>
        private static void SendUDPDataToAll(int _exceptClient, Packet _packet)
        {
            _packet.WriteLength();
            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                if (i != _exceptClient)
                {
                    Server.clients[i].udp.SendData(_packet);
                }
            }
        }

        #region Packets
        /// <summary>Sends a welcome message to the given client.</summary>
        /// <param name="_toClient">The client to send the packet to.</param>
        /// <param name="_msg">The message to send.</param>
        public static void Welcome(int _toClient, string _msg)
        {
            using (Packet _packet = new Packet((int)ServerPackets.welcome))
            {
                _packet.Write(_msg);
                _packet.Write(_toClient);

                SendTCPData(_toClient, _packet);
            }
        }
        /// <summary>Sends a welcome message to the given client.</summary>
        /// <param name="_toClient">The client to send the packet to.</param>
        /// <param name="_msg">The message to send.</param>
        public static void AuthState(int _toClient, User _user)
        {
            Console.WriteLine("Sending Auth Response  {" + _user.acc_aviable + "} to Client {" + _toClient + "} ");
            using (Packet _packet = new Packet((int)ServerPackets.auth))
            {
                _packet.Write(_user);
                SendTCPData(_toClient, _packet);
            }
        }

        public static void SendTrash(int _toClient, int _errorCode)
        {
            Console.WriteLine($"Sending Trash # {_errorCode} to Client {_toClient} ");
            using (Packet _packet = new Packet((int)ServerPackets.test))
            {
                _packet.Write(_errorCode);
                SendTCPData(_toClient, _packet);
            }
        }

        public static void QueueAcepted(int _toClient)
        {
            Console.WriteLine($"Sending Queue Response to client {_toClient}");
            using (Packet _packet = new Packet((int)ServerPackets.queueUpdate))
            {
                SendTCPDataToAll(QueueManager.randomQueuesGrup[_toClient].GroupMembers(), _packet);
            }
        }
        public static void QueueCancel(int _toClient)
        {
            Console.WriteLine($"Sending Queue canceled response to client {_toClient}");
            using (Packet _packet = new Packet((int)ServerPackets.queueUpdate))
            {
                SendTCPData(_toClient, _packet);
            }
        }

        public static void GameFoundRequest(List<int> _grupClients)
        {
            Console.WriteLine("Sending Game Found to clients:");
            foreach (int _id in _grupClients)
            {
                Console.WriteLine(_id);
            }
            using (Packet _packet = new Packet((int)ServerPackets.gameFound))
            {
                SendTCPDataToAll(_grupClients, _packet);
            }
        }
        public static void GroupCreated(int _toClient)
        {
            using (Packet _packet = new Packet((int)ServerPackets.groupCreated))
            {
                SendTCPData(_toClient, _packet);
            }
        }
        public static void GroupDisolved(int _groupLead)
        {
            using (Packet _packet = new Packet((int)ServerPackets.groupDisolved))
            {
                List<int> _toMembers = new List<int>();
                foreach (PlayerQueue item in QueueManager.preMadeGroups[_groupLead].groupMembers)
                {
                    Server.clients[item.id].groupLeader = 0;
                    if (!item.isGroupLeader)
                    {
                        _toMembers.Add(item.id);
                    }
                }
                QueueManager.preMadeGroups.Remove(_groupLead);
                _packet.Write(3);
                SendTCPDataToAll(_toMembers, _packet);
            }
        }
        public static void FriendList(int _toClient)
        {
            using (Packet _packet = new Packet((int)ServerPackets.friendList))
            {
                List<User> usersList = DbManager.FriendList(_toClient);
                _packet.Write(usersList.Count);
                foreach (User item in usersList)
                {
                    _packet.Write(item.GetFriendReference());
                }
                SendTCPData(_toClient, _packet);
            }
        }
        public static void GroupInvited(int _toClient, int _fromFriend)
        {
            if (_toClient == 0) return;
            using (Packet _packet = new Packet((int)ServerPackets.groupInvited))
            {
                _packet.Write(_fromFriend);
                _packet.Write($"{Server.clients[_fromFriend].nickName} grouop request");
                SendTCPData(_toClient, _packet);
            }
        }

        public static void GroupInvitedResponse(int _groupLead, int _fromClient)
        {
            using (Packet _packet = new Packet((int)ServerPackets.groupInvitedResponse))
            {
                _packet.Write(_fromClient);
                _packet.Write(QueueManager.preMadeGroups[_groupLead].GroupSize());
                foreach (PlayerQueue item in QueueManager.preMadeGroups[_groupLead].groupMembers)
                {
                    Console.WriteLine($"P {item.id} {item.nick_name}");
                    _packet.Write(item);
                }
                SendTCPDataToAll(QueueManager.preMadeGroups[_groupLead].GroupMembers(), _packet);
            }
        }
        public static void UpdateFriendStatus(int _fromClient, string _token, bool _status)
        {
            using (Packet _packet = new Packet((int)ServerPackets.updateFriendStatus))
            {
                List<int> _toFriends = new List<int>();
                List<User> usersList = DbManager.FriendList(_fromClient);
                _packet.Write(_token);
                _packet.Write(_fromClient);
                _packet.Write(_status);
                foreach (User item in usersList)
                {
                    _packet.Write(item.GetFriendReference());
                    if (item.server_slot != 0)
                    {
                        _toFriends.Add(item.server_slot);
                    }
                }
                SendTCPDataToAll(_toFriends, _packet);
            }
        }
        public static void UpdateGroupStatus(int _fromClient)
        {
            Console.WriteLine($"Sending UpdateGroupStatus disconect to {_fromClient} group members");
            /*using (Packet _packet = new Packet((int)ServerPackets.updateGroupStatus))
            {
                _packet.Write(_fromClient);
                List<int> groupMembers = QueueManager.preMadeGroups[Server.clients[_fromClient].groupLeader].GroupMembers(_fromClient);
                SendTCPDataToAll(groupMembers, _packet);
            }*/
        }
        #endregion
    }
}
