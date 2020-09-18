using System.Net;
using UnityEngine;

public class LoginHandler : MonoBehaviour
{
    public static void AuthResponse(Packet _packet)
    {
        string _msg = _packet.ReadString();
        int _myId = _packet.ReadInt();

        Debug.Log($"Message from Auth server: {_msg}");
        LoginClient.instance.myId = _myId;
        LoginClientSend.WelcomeAuthReceived();

        // Now that we have the client's id, connect UDP
        LoginClient.instance.udp.Connect(((IPEndPoint)LoginClient.instance.tcp.socket.Client.LocalEndPoint).Port);
    }
}
