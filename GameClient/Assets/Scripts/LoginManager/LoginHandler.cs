using System.Net;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginHandler : MonoBehaviour
{
    public static void Welcome(Packet _packet)
    {
        string _msg = _packet.ReadString();
        int _myId = _packet.ReadInt();
        Debug.Log($"Message from server: {_msg}");
        LoginClient.instance.myId = _myId;
        LoginUIManager.instance.text.text = "Auth";
        LoginClientSend.WelcomeReceived();
    }
    public static void AuthResponse(Packet _packet)
    {
        UserProfile _userResponse = _packet.ReadUser();
        LoginUIManager.instance.logIn.interactable = true;
        if (_userResponse.acc_aviable)
        {
            LoginClient.instance.udp.Connect(((IPEndPoint)LoginClient.instance.tcp.socket.Client.LocalEndPoint).Port);
            GameInfo.strPlayerName = _userResponse.userNickName;
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
        //SceneManager.LoadScene("Menu");
    }
    public static void TrashRecived(Packet _packet)
    {
        UIPrincipalPanel.HandleAlert(1, _packet);
    }
    public static void QueueRecived(Packet _packet)
    {
        Debug.Log("QueueRecived");
        UIFindGame.instance.txtQueueStatus.text = "On Queue...";
        GameInfo.isQueued = true;
        UIFindGame.instance.btnQuitQueue.SetActive(true);
    }
    public static void GameFound(Packet _packet)
    {
        UIFindGame.instance.txtQueueStatus.text = "InGame";
    }
    public static void GrupCreated(Packet _packet)
    {
        UIFindGame.instance.txtMessageServer.enabled = false;
        GameInfo.isGrouped = true;
        GameInfo.friends_in_group.Add(new FriendReference(LoginClient.instance.myId, GameInfo.strPlayerName));
        GameInfo.isLoadGroups = true;
        UIFindGame.instance.btnQueueGame.SetActive(true);
    }
    public static void GrupDisolved(Packet _packet)
    {
        UIPrincipalPanel.instance.btnHome.interactable = false;
        UIPrincipalPanel.instance.btnPlayGame.interactable = true;
        GameInfo.isGrouped = false;
        GameInfo.friends_in_group.Clear();
        MenuUIManager.instance.findGameMenu.SetActive(false);
        MenuUIManager.instance.homeMenu.SetActive(true);
    }
    public static void FriendsList(Packet _packet)
    {
        GameInfo.user_friends = _packet.ReadFriendReference();
        GameInfo.isLoadFriends = true;
    }
    public static void GroupInvited(Packet _packet)
    {
        UIPrincipalPanel.HandleAlert(3, _packet);
    }
    public static void GroupInvitedResponse(Packet _packet)
    {
        if (!GameInfo.isGrouped)
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
        foreach (FriendReference item in GameInfo.user_friends)
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
                    GameInfo.isLoadGroups = true;
                }
            }
        }
        GameInfo.isLoadFriends = true;
    }
}
