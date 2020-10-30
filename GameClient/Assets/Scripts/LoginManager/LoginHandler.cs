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
        string _trash = _packet.ReadString();
        Debug.Log($"TrashRecived {_trash}");

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
        UIFindGame.instance.btnQueueGame.SetActive(true);
    }
    public static void GrupDisolved(Packet _packet)
    {
        UIPrincipalPanel.instance.btnHome.interactable = false;
        UIPrincipalPanel.instance.btnPlayGame.interactable = true;
        GameInfo.isGrouped = false;
        MenuUIManager.instance.findGameMenu.SetActive(false);
        MenuUIManager.instance.homeMenu.SetActive(true);
    }
    public static void FriendsList(Packet _packet)
    {
        GameInfo.user_friends = _packet.ReadFriendReference();
        Debug.Log($"FriendsList size {GameInfo.user_friends.Count}");
        GameInfo.isLoadFriends = true;
        AlertManager.HandleAlert(1,"LOL");
    }
}
