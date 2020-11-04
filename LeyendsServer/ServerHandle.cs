using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace LeyendsServer
{
    class ServerHandle
    {
        public static void WelcomeReceived(int _fromClient, Packet _packet)//ID:1
        {
            int _clientIdCheck = _packet.ReadInt();
            string _username = _packet.ReadString();
            string _pass = _packet.ReadString();
            Console.WriteLine($"{Server.clients[_fromClient].tcp.socket.Client.RemoteEndPoint} is now player {_fromClient}.");
            if (_fromClient != _clientIdCheck)
            {
                Console.WriteLine($"Player \"{_username}\" (ID: {_fromClient}) has assumed the wrong client ID ({_clientIdCheck})!");
                return;
            }

            User _user = DbManager.Auth(_fromClient, _username, _pass);
            ServerSend.AuthState(_fromClient, _user);
            ServerSend.FriendList(_fromClient);
            ServerSend.UpdateFriendStatus(_fromClient, Server.clients[_fromClient].token.ToString(), true);

        }
        public static void QueueRequestForRandomMatch(int _fromClient, Packet _packet)//ID:2
        {
            Console.WriteLine($"Incoming queue request from player {_fromClient}.");
            if (!Server.clients[_fromClient].queueStatus &&
            QueueManager.preMadeGroups[_fromClient] != null)
            {
                foreach (int item in QueueManager.preMadeGroups[_fromClient].GroupMembers())
                {
                    Server.clients[item].queueStatus = true;
                    Server.clients[item].queueType = QueueType.RANDOM;
                }
                QueueManager.randomQueuesGrup.Add(_fromClient, QueueManager.preMadeGroups[_fromClient]);
            }
            QueueManager.preMadeGroups.Remove(_fromClient);
            ServerSend.QueueAcepted(_fromClient);
        }
        public static void ClientTrashRequest(int _fromClient, Packet _packet)//ID:3
        {
            Console.WriteLine($"Incoming trash request from player {_fromClient} .");
            ServerSend.SendTrash(_fromClient, (int)ErrorCode.General);
        }
        public static void QuitQueueRequest(int _fromClient, Packet _packet)//ID:4
        {
            if (!Server.clients[_fromClient].queueStatus) return;
            Console.WriteLine($"Player {_fromClient} quit queue .");
            QueueManager.preMadeGroups.Add(Server.clients[_fromClient].groupLeader, QueueManager.randomQueuesGrup[_fromClient]);
            foreach (int item in QueueManager.preMadeGroups[Server.clients[_fromClient].groupLeader].GroupMembers())
            {
                Server.clients[item].queueStatus = false;
                Server.clients[item].queueType = QueueType.NON;
            }
            QueueManager.randomQueuesGrup.Remove(Server.clients[_fromClient].groupLeader);
            ServerSend.QueueCancel(_fromClient);
        }
        public static void GroupRequest(int _fromClient, Packet _packet)//ID:5
        {
            Console.WriteLine($"Group creation request  {_fromClient} .");
            PlayerQueue _newLeaderGrup = new PlayerQueue(_fromClient, Server.clients[_fromClient].nickName);
            List<PlayerQueue> _newGrup = new List<PlayerQueue>();
            _newGrup.Add(_newLeaderGrup);
            QueueGroup _newQueueGroup = new QueueGroup(QueueType.NON, _newGrup);
            QueueManager.preMadeGroups.Add(_fromClient, _newQueueGroup);
            Server.clients[_fromClient].groupLeader = _fromClient;
            ServerSend.GroupCreated(_fromClient);
        }
        public static void GroupDisolve(int _fromClient, Packet _packet)//ID:6
        {
            Console.WriteLine($"Group disolve request  {_fromClient} .");
            Server.clients[_fromClient].groupLeader = 0;
            QueueManager.preMadeGroups.Remove(_fromClient);
            if (QueueManager.preMadeGroups[_fromClient] != null)
            {
                Console.WriteLine($"Group slot  player {QueueManager.preMadeGroups[_fromClient].groupMembers[0].id} .");
            }
            ServerSend.GroupDisolved(_fromClient);
        }

        public static void InviteFriendToGroup(int _fromClient, Packet _packet)//ID:7
        {
            Console.WriteLine($" slot {_fromClient} has lead {Server.clients[_fromClient].groupLeader}");
            if (Server.clients[_fromClient].groupLeader != _fromClient)
            {
                ServerSend.SendTrash(_fromClient, (int)ErrorCode.NoGroup);
                return;
            }
            int _toFriend = _packet.ReadInt();
            if (Server.clients[_toFriend].groupLeader != 0)
            {
                ServerSend.SendTrash(_fromClient, (int)ErrorCode.PlayerInGroup);
                return;
            }
            Console.WriteLine($"{Server.clients[_fromClient].nickName} InviteFriendToGroup  {Server.clients[_toFriend].nickName} .");
            ServerSend.GroupInvited(_toFriend, _fromClient);
        }
        public static void InviteFriendToGroupResponse(int _fromClient, Packet _packet)//ID:8
        {
            int _toFriend = _packet.ReadInt();
            if (Server.clients[_fromClient].groupLeader != 0)
            {
                ServerSend.GroupDisolved(Server.clients[_fromClient].groupLeader);
            }
            Console.WriteLine($"{Server.clients[_fromClient].nickName} join {Server.clients[_toFriend].nickName} group .");
            Server.clients[_fromClient].groupLeader = _toFriend;
            PlayerQueue _newGroupMember = new PlayerQueue(_fromClient, Server.clients[_fromClient].nickName);
            QueueManager.preMadeGroups[_toFriend].groupMembers.Add(_newGroupMember);
            ServerSend.GroupInvitedResponse(_toFriend, _fromClient);

        }

    }
}
