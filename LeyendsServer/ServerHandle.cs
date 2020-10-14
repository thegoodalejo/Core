using System;
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
            Console.WriteLine("_fromClient : " + _fromClient);
            Console.WriteLine("_clientIdCheck : " + _clientIdCheck);
            Console.WriteLine($"{Server.clients[_fromClient].tcp.socket.Client.RemoteEndPoint} is now player {_fromClient}.");
            if (_fromClient != _clientIdCheck)
            {
                Console.WriteLine($"Player \"{_username}\" (ID: {_fromClient}) has assumed the wrong client ID ({_clientIdCheck})!");
            }

            ServerSend.AuthState(_fromClient, DbManager.Auth(_username, _pass));
        }

        public static void QueueForRandomMatch(int _fromClient, Packet _packet)
        {
            int _clientIdCheck = _packet.ReadInt();
            string _userToken = _packet.ReadString();
            Console.WriteLine($"Incoming queue request from player {_clientIdCheck} token {_userToken} .");
        }


    }
}
