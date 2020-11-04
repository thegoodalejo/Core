using System.Net;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginHandler : MonoBehaviour
{
    public static void Welcome(Packet _packet)//ID:1
    {
        string _msg = _packet.ReadString();
        int _myId = _packet.ReadInt();
        Debug.Log($"Message from server: {_msg}");
        LoginClient.instance.myId = _myId;
        LoginUIManager.instance.text.text = "Auth";
        LoginClientSend.WelcomeReceived();
    }
    public static void AuthResponse(Packet _packet)//ID:2
    {
        UserProfile _userResponse = _packet.ReadUser();
        if (_userResponse.acc_aviable)
        {
            LoginClient.instance.udp.Connect(((IPEndPoint)LoginClient.instance.tcp.socket.Client.LocalEndPoint).Port);
            LoginClient.instance.strPlayerName = _userResponse.userNickName;
            LoginClient.instance.token = _userResponse.id;
            SceneManager.LoadScene("Menu");
        }
        else
        {
            LoginClient.instance.tcp.socket.Close();
            LoginUIManager.instance.text.text = "Invalid credentials";
            LoginUIManager.instance.usernameField.interactable = true;
            LoginUIManager.instance.passwordField.interactable = true;
        }
        //LoginClient.instance.udp.Connect(((IPEndPoint)LoginClient.instance.tcp.socket.Client.LocalEndPoint).Port);
        LoginUIManager.instance.logIn.SetActive(true);
    }
    public static void TrashRecived(Packet _packet)//ID:3
    {
        UIPrincipalPanel.HandleAlert(1, _packet);
    }
    public static void QueueRecived(Packet _packet)
    {
        Debug.Log("QueueRecived");
        UIFindGame.instance.txtQueueStatus.text = "On Queue...";
        LoginClient.instance.isQueued = true;
        UIFindGame.instance.btnQuitQueue.SetActive(true);
    }
    public static void GameFound(Packet _packet)
    {
        UIFindGame.instance.txtQueueStatus.text = "InGame";
    }
    public static void GrupCreated(Packet _packet)
    {
        UIFindGame.instance.txtMessageServer.enabled = false;
        LoginClient.instance.isGrouped = true;
        LoginClient.instance.friends_in_group.Add(new FriendReference(LoginClient.instance.myId, LoginClient.instance.strPlayerName));
        LoginClient.instance.isLoadGroups = true;
        UIFindGame.instance.btnQueueGame.SetActive(true);
    }
    public static void GrupDisolved(Packet _packet)
    {
        UIPrincipalPanel.instance.btnHome.interactable = false;
        UIPrincipalPanel.instance.btnPlayGame.interactable = true;
        LoginClient.instance.isGrouped = false;
        LoginClient.instance.isQueued = false;
        LoginClient.instance.friends_in_group.Clear();
        LoginClient.instance.isLoadGroups = true;
        MenuUIManager.instance.findGameMenu.SetActive(false);
        MenuUIManager.instance.homeMenu.SetActive(true);
        UIPrincipalPanel.HandleAlert(1, _packet);
    }
    public static void FriendsList(Packet _packet)
    {
        LoginClient.instance.user_friends = _packet.ReadFriendReference();
        LoginClient.instance.isLoadFriends = true;
    }
    public static void GroupInvited(Packet _packet)
    {
        UIPrincipalPanel.HandleAlert(3, _packet);
    }
    public static void GroupInvitedResponse(Packet _packet)
    {
        if (!LoginClient.instance.isGrouped)
        {
            MenuUIManager.LoadGroupGame();
        }
        UIPrincipalPanel.HandleAlert(2, _packet);

    }
    public static void UpdateFriendStatus(Packet _packet)
    {
        string _token = _packet.ReadString();
        int _slot = _packet.ReadInt();
        bool _status = _packet.ReadBool();
        foreach (FriendReference item in LoginClient.instance.user_friends)
        {
            if (item.id == _token)
            {
                if (_status)
                {
                    item.server_slot = _slot;
                }
                else
                {
                    item.server_slot = 0;
                    item.on_my_group = false;
                    LoginClient.instance.isLoadGroups = true;
                }
            }
        }
        LoginClient.instance.isLoadFriends = true;
    }
}
