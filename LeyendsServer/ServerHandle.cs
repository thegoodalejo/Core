﻿using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace LeyendsServer
{
    class ServerHandle
    {
        public static void WelcomeReceived(int _fromClient, Packet _packet)
        {
            int _clientIdCheck = _packet.ReadInt();
            string _username = _packet.ReadString();
            string _pass = _packet.ReadString();
            Console.WriteLine($"{Server.clients[_fromClient].tcp.socket.Client.RemoteEndPoint} is now player {_fromClient}.");
            if (_fromClient != _clientIdCheck)
            {
                Console.WriteLine($"Player \"{_username}\" (ID: {_fromClient}) has assumed the wrong client ID ({_clientIdCheck})!");
            }

            User _user = DbManager.Auth(_fromClient, _username, _pass);
            ServerSend.FriendList(_fromClient);
            ServerSend.AuthState(_fromClient, _user);
            
        }

        public static void AuthRequest(int _fromClient, Packet _packet)
        {
            string _username = _packet.ReadString();
            string _pass = _packet.ReadString();
            ServerSend.AuthState(_fromClient, DbManager.Auth(_fromClient, _username, _pass));
            ServerSend.FriendList(_fromClient);
        }
        public static void ClientTrashRequest(int _fromClient, Packet _packet)
        {
            Console.WriteLine($"Incoming trash request from player {_fromClient} .");
            ServerSend.SendTrash(_fromClient);
        }
        public static void QueueRequestForRandomMatch(int _fromClient, Packet _packet)
        {
            int _clientIdCheck = _packet.ReadInt();
            Console.WriteLine($"Incoming queue request from player {_clientIdCheck}.");
            if (!Server.clients[_fromClient].queueStatus && _fromClient != _clientIdCheck)
            {
                Server.clients[_fromClient].queueStatus = true;
                Server.clients[_fromClient].queueType = QueueType.RANDOM;
                QueueManager.Add(_fromClient);
            }
            ServerSend.QueueAcepted(_fromClient);
        }
        public static void QuitQueueRequest(int _fromClient, Packet _packet)
        {
            Console.WriteLine($"Player {_fromClient} quit queue .");
            ServerSend.QueueCancel(_fromClient);
        }
        public static void GroupRequest(int _fromClient, Packet _packet)
        {
            Console.WriteLine($"Group creation request  {_fromClient} .");
            PlayerQueue _newLeaderGrup = new PlayerQueue(_fromClient);
            List<PlayerQueue> _newGrup= new List<PlayerQueue>();
            _newGrup.Add(_newLeaderGrup);
            QueueGroup _newQueueGroup = new QueueGroup(QueueType.NON, _newGrup);
            QueueManager.preMadeGroups.Add(_fromClient,_newQueueGroup);
            Server.clients[_fromClient].groupLeader = _fromClient;
            ServerSend.GroupCreated(_fromClient);
        }
        public static void GroupDisolve(int _fromClient, Packet _packet)
        {
            Console.WriteLine($"Group disolve request  {_fromClient} .");
            Server.clients[_fromClient].groupLeader = 0;
            QueueManager.preMadeGroups.Remove(_fromClient);
            if(QueueManager.preMadeGroups[_fromClient]!=null){
                Console.WriteLine($"Group slot  player {QueueManager.preMadeGroups[_fromClient].groupMembers[0].id} .");
            }
            ServerSend.GroupDisolved(_fromClient);
        }

        public static void InviteFriendToGroup(int _fromClient, Packet _packet)
        {
            int _toFriend = _packet.ReadInt();
            Console.WriteLine($"Player {_fromClient} InviteFriendToGroup  {_toFriend} .");
            ServerSend.GroupInvited(_toFriend,_fromClient);
        }
        public static void GroupInvitedResponse(int _fromClient, Packet _packet)
        {
            int _toFriend = _packet.ReadInt();
            bool _response = _packet.ReadBool();
            Console.WriteLine($"Player {_fromClient} GroupInvitedResponse  {_response} .");
            if(_response){
                
            }
            ServerSend.GroupInvitedResponse(_toFriend,_fromClient,_response);
            
        }

    }
}
