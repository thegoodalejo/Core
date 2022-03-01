using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class HandleClientMsg
{
    public static void WelcomeReceived(int _fromClient, Packet _packet)
    {
        int _clientIdCheck = _packet.ReadInt();
        string _username = _packet.ReadString();

        Debug.Log($"{HostClients.clients[_fromClient].tcp.socket.Client.RemoteEndPoint} connected successfully and is now player {_fromClient}.");
        if (_fromClient != _clientIdCheck)
        {
            Debug.Log($"Player \"{_username}\" (ID: {_fromClient}) has assumed the wrong client ID ({_clientIdCheck})!");
        }
        // Now that we have the client's id, connect UDP
        HostClients.clients[_fromClient].udp.Connect((IPEndPoint)HostClients.clients[_fromClient].tcp.socket.Client.RemoteEndPoint);
        Debug.Log($"Attempt to UDP conn {(IPEndPoint)HostClients.clients[_fromClient].tcp.socket.Client.RemoteEndPoint}");
        HostClients.clients[_fromClient].SendIntoGame(_username);
    }

    public static void PlayerMovement(int _fromClient, Packet _packet)
    {
        bool[] _inputs = new bool[_packet.ReadInt()];
        for (int i = 0; i < _inputs.Length; i++)
        {
            _inputs[i] = _packet.ReadBool();
        }
        Vector3 _rotation = _packet.ReadVector3();
        Vector3 _sight = _packet.ReadVector3();
        HostClients.clients[_fromClient].player.SetInput(_inputs, _rotation, _sight);
    }

    public static void PlayerShoot(int _fromClient, Packet _packet)
    {
        Vector3 _shootDirection = _packet.ReadVector3();
        HostClients.clients[_fromClient].player.Shoot(_shootDirection);

        Debug.Log("Player " +_fromClient+" Shoot to " + _shootDirection);
    }

    public static void PlayerThrowItem(int _fromClient, Packet _packet)
    {
        Vector3 _throwDirection = _packet.ReadVector3();

        HostClients.clients[_fromClient].player.ThrowItem(_throwDirection);
    }
}
