using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendToServer : MonoBehaviour
{
    /// <summary>Sends a packet to a client via TCP.</summary>
    /// <param name="_toClient">The client to send the packet the packet to.</param>
    /// <param name="_packet">The packet to send to the client.</param>
    private static void SendTCPData(Packet _packet)
    {
        _packet.WriteLength();
        ListenServer.instance.tcp.SendData(_packet);
    }

    #region Packets
    /// <summary>Sends a welcome message to the given client.</summary>
    /// <param name="_toClient">The client to send the packet to.</param>
    /// <param name="_msg">The message to send.</param>
    public static void WelcomeReceived(string _msg)
    {
        using (Packet _packet = new Packet((int)ToServerPackets.welcomeReceived))
        {
            _packet.Write(_msg);
            SendTCPData(_packet);
        }
    }
    #endregion
}
