using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertManager : MonoBehaviour
{
    private delegate void PacketHandler(string _message);
    private static Dictionary<int, PacketHandler> packetHandlers;
    public static void InitializeAlertData()
    {
        packetHandlers = new Dictionary<int, PacketHandler>()
        {
            { (int)AlertServerPackets.error, Error },
        };
    }
    public static void HandleAlert(int _id, string _message)
    {
        packetHandlers[_id](_message);
    }
    private static void Error(string _message)
    {
        Debug.Log(_message);
    }

}


