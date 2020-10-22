using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class LoginClient : MonoBehaviour
{

    public static LoginClient instance;
    public static int dataBufferSize = 4096;
    public string ip = "127.0.0.1";
    public string token = "";
    public int port = 26940;
    public int myId = 0;
    public TCP tcp;
    public UDP udp;

    private bool isConnected = false;
    private delegate void PacketHandler(Packet _packet);
    private static Dictionary<int, PacketHandler> packetHandlers;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Login instance already exists, destroying object!");
            Destroy(this);
        }
    }

    private void OnApplicationQuit()
    {
        Disconnect(); // Disconnect when the game is closed
    }

    public void ConnectToServer()
    {
        LoginUIManager.instance.logIn.interactable = false;
        tcp = new TCP();
        udp = new UDP();

        InitializeClientData();

        isConnected = true;
        tcp.Connect(); // Connect tcp, udp gets connected once tcp is done
    }

    /// <summary>Initializes all necessary client data.</summary>
    private void InitializeClientData()
    {
        packetHandlers = new Dictionary<int, PacketHandler>()
        {
            { (int)LoginServerPackets.welcome, LoginHandler.Welcome },
            { (int)LoginServerPackets.auth, LoginHandler.AuthResponse },
            { (int)LoginServerPackets.test, LoginHandler.TrashRecived },
            { (int)LoginServerPackets.queueUpdate, LoginHandler.QueueRecived },
        };
        Debug.Log("Initialized packets.");
    }

    /// <summary>Disconnects from the server and stops all network traffic.</summary>
    private void Disconnect()
    {
        if (isConnected)
        {
            isConnected = false;
            tcp.socket.Close();
            udp.socket.Close();

            Debug.Log("Disconnected from login server.");
        }
    }
}
