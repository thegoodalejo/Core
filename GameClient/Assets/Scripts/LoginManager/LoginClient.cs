using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public partial class LoginClient : MonoBehaviour
{
    public string strPlayerName;
    public string strUID;
    public int groupSize;
    public bool isGroupLead;
    public bool isGrouped;
    public bool isQueued;
    public bool isLoadFriends;
    public bool isLoadGroups;
    public List<FriendReference> user_friends { get; set; }
    public List<FriendReference> friends_in_group { get; set; }

    public static LoginClient instance;
    public static int dataBufferSize = 4096;
    public string ip = "127.0.0.1";
    public string token = "";
    public int port = 26940;
    public int gamePort = 0;
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
        instance.strPlayerName = "Loading...";
        instance.groupSize = 0;
        instance.isGroupLead = false;
        instance.isGrouped = false;
        instance.isQueued = false;
        instance.isLoadFriends = false;
        instance.isLoadGroups = false;
        instance.user_friends = new List<FriendReference>();
        instance.friends_in_group = new List<FriendReference>();
    }

    public void LogOut(){
        Disconnect();
    }

    private void OnApplicationQuit()
    {
        Disconnect(); // Disconnect when the game is closed
    }

    public void ConnectToServer()
    {
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
            { (int)LoginServerPackets.gameFound, LoginHandler.GameFound },
            { (int)LoginServerPackets.groupCreated, LoginHandler.GrupCreated },
            { (int)LoginServerPackets.groupDisolved, LoginHandler.GrupDisolved },
            { (int)LoginServerPackets.friendList, LoginHandler.FriendsList },
            { (int)LoginServerPackets.groupInvited, LoginHandler.GroupInvited },
            { (int)LoginServerPackets.groupInvitedResponse, LoginHandler.GroupInvitedResponse },
            { (int)LoginServerPackets.updateFriendStatus, LoginHandler.UpdateFriendStatus },
            { (int)LoginServerPackets.singleMemberLeave, LoginHandler.SingleMemberLeave },
            { (int)LoginServerPackets.queueCanceled, LoginHandler.QueueCanceled },
            { (int)LoginServerPackets.friendRequest, LoginHandler.FriendRequest },
            { (int)LoginServerPackets.gameRequest, LoginHandler.GameRequest },
        };
    }
    

    /// <summary>Disconnects from the server and stops all network traffic.</summary>
    private void Disconnect()
    {
        if (isConnected)
        {
            isConnected = false;
            tcp.socket.Close();
            //udp.socket.Close();
            SceneManager.LoadScene("Login");
            //LoginUIManager.instance.text.text = "Disconected from server";
        }
    }
}
