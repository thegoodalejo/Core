using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleServerMsg : MonoBehaviour
{
    public static void Welcome(Packet _packet)//ID:1
    {
        string _msg = _packet.ReadString();
        Debug.Log($"Message from server: {_msg}");
        SendToServer.WelcomeReceived("OKrdgrsgssefesfesfeasfsfsffssfsffsfsfsfsf");
    }
}
