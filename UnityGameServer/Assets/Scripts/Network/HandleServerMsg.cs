using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;

public class HandleServerMsg : MonoBehaviour
{
    public static void Welcome(Packet _packet)//ID:1
    {
        string _msg = _packet.ReadString();
        Debug.Log($"Message from server: {_msg}");
        SendToServer.WelcomeReceived("OKrdgrsgssefesfesfeasfsfsffssfsffsfsfsfsf");
    }
    public static void EndPointGroup(Packet _packet)//ID:2
    {
        List<string> _epGroup = _packet.ReadEpGorup();
        HostClients.InitializeServerPlayerSlots(_epGroup);
    }
}
