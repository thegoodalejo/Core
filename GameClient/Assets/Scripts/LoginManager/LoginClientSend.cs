﻿using UnityEngine;

public class LoginClientSend : MonoBehaviour
{
    /// <summary>Sends a packet to the server via TCP.</summary>
    /// <param name="_packet">The packet to send to the sever.</param>
    private static void SendTCPData(Packet _packet)
    {
        _packet.WriteLength();
        LoginClient.instance.tcp.SendData(_packet);
    }

    /// <summary>Sends a packet to the server via UDP.</summary>
    /// <param name="_packet">The packet to send to the sever.</param>
    private static void SendUDPData(Packet _packet)
    {
        _packet.WriteLength();
        LoginClient.instance.udp.SendData(_packet);
    }

    public static void WelcomeReceived()
    {
        using (Packet _packet = new Packet((int)LoginClientPackets.welcomeReceived))
        {
            _packet.Write(LoginClient.instance.myId);
            _packet.Write(LoginUIManager.instance.usernameField.text);
            _packet.Write(LoginUIManager.instance.passwordField.text);

            SendTCPData(_packet);
        }
    }

    /// <summary>Queue server for a MatchFinder.</summary>
    public static void QueueForRandomMatch()
    {
        using (Packet _packet = new Packet((int)LoginClientPackets.queueRequestForRandomMatch))
        {
            _packet.Write(LoginClient.instance.myId);
            MenuUIManager.instance.queue.interactable = false;
            SendTCPData(_packet);
        }
    }

    public static void TrashRequest()
    {
        Debug.Log("Trash request");
        using (Packet _packet = new Packet((int)LoginClientPackets.trashRequest))
        {
            _packet.Write(LoginClient.instance.myId);
            SendTCPData(_packet);
        }
    }
}
