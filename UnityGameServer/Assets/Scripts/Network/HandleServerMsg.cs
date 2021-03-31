using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;

public class HandleServerMsg : MonoBehaviour
{
    public static void Welcome(Packet _packet)//ID:1
    {
        string _token = _packet.ReadString();
        int _myRid = _packet.ReadInt();
        Debug.Log($"Temp Token: {_token}");
        Debug.Log($"Room ID: {_myRid}");
        SendToServer.WelcomeReceived($"Server [{_myRid}]: Temp Token: {_token} Room ID: {_myRid} Port: {ListenServer.instance.tcp.port} MaxPlayers: {ListenServer.instance.maxPlayers}");
    }
    public static void EndPointGroup(Packet _packet)//ID:2
    {
        List<string> _epGroup = _packet.ReadEpGorup();
        HostClients.InitializeServerPlayerSlots(_epGroup);
    }
}
