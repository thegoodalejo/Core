using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace LeyendsServer
{
    class GameHandle
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
    }
}
