using System.Collections.Generic;
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
    public static void QueueRecived(Packet _packet)//ID:4
    {
        Debug.Log("QueueRecived");
        UIFindGame.instance.txtQueueStatus.text = "On Queue...";
        LoginClient.instance.isQueued = true;
        UIFindGame.instance.btnQuitQueue.SetActive(true);
    }
    public static void GameFound(Packet _packet)//ID:5
    {
        UIFindGame.instance.txtQueueStatus.text = "InGame";
    }
    public static void GrupCreated(Packet _packet)//ID:6
    {
        UIFindGame.instance.txtMessageServer.enabled = false;
        LoginClient.instance.isGrouped = true;
        LoginClient.instance.isGroupLead = true;
        LoginClient.instance.friends_in_group.Add(new FriendReference(LoginClient.instance.myId, LoginClient.instance.strPlayerName));
        LoginClient.instance.isLoadGroups = true;
        UIFindGame.instance.btnQueueGame.SetActive(true);
        UIFindGame.instance.btnQuitGroup.SetActive(true);
    }
    public static void GrupDisolved(Packet _packet)//ID:7
    {
        UIPrincipalPanel.instance.btnHome.interactable = false;
        UIPrincipalPanel.instance.btnPlayGame.interactable = true;
        LoginClient.instance.isGrouped = false;
        LoginClient.instance.isGroupLead = false;
        LoginClient.instance.isQueued = false;
        LoginClient.instance.friends_in_group.Clear();
        LoginClient.instance.isLoadGroups = true;
        UIFindGame.instance.btnQueueGame.SetActive(false);
        UIFindGame.instance.btnQuitGroup.SetActive(false);
        MenuUIManager.instance.findGameMenu.SetActive(false);
        MenuUIManager.instance.homeMenu.SetActive(true);
        UIFindGame.instance.btnQueueGame.GetComponent<Button>().interactable = true;
        UIPrincipalPanel.HandleAlert(1, _packet);
    }
    public static void FriendsList(Packet _packet)//ID:8
    {
        LoginClient.instance.user_friends = _packet.ReadFriendReference();
        LoginClient.instance.isLoadFriends = true;
    }
    public static void GroupInvited(Packet _packet)//ID:9
    {
        UIPrincipalPanel.HandleAlert(3, _packet);
    }
    public static void GroupInvitedResponse(Packet _packet)//ID:10
    {
        if (!LoginClient.instance.isGrouped)
        {
            MenuUIManager.LoadGroupGame();
        }
        UIPrincipalPanel.HandleAlert(2, _packet);

    }
    public static void UpdateFriendStatus(Packet _packet)//ID:11
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
    public static void SingleMemberLeave(Packet _packet)//ID:12
    {
        int _memberGone = _packet.ReadInt();
        Debug.Log("SingleMemberLeave" + _memberGone);
        if (LoginClient.instance.myId == _memberGone)
        {
            Debug.Log("SingleMemberLeave YOU");
            UIPrincipalPanel.instance.btnHome.interactable = false;
            UIPrincipalPanel.instance.btnPlayGame.interactable = true;
            LoginClient.instance.isGrouped = false;
            LoginClient.instance.isGroupLead = false;
            LoginClient.instance.isQueued = false;
            LoginClient.instance.friends_in_group.Clear();
            LoginClient.instance.isLoadGroups = true;
            UIFindGame.instance.btnQueueGame.SetActive(false);
            UIFindGame.instance.btnQuitGroup.SetActive(false);
            MenuUIManager.instance.findGameMenu.SetActive(false);
            MenuUIManager.instance.homeMenu.SetActive(true);
            UIFindGame.instance.btnQueueGame.GetComponent<Button>().interactable = true;
        }
        else
        {
            List<FriendReference> _newGroup = new List<FriendReference>();
            int _groupSize = _packet.ReadInt();
            Debug.Log("SingleMemberLeave ANOTHER");
            for (int i = 0; i < _groupSize; i++)
            {
                _newGroup.Add(new FriendReference(_packet.ReadInt(), _packet.ReadString()));
            }
            LoginClient.instance.friends_in_group = _newGroup;
            LoginClient.instance.isLoadGroups = true;
        }

    }
    public static void QueueCanceled(Packet _packet)//ID:13
    {
        Debug.Log("QueueRecived");
        UIFindGame.instance.txtQueueStatus.text = "On Group...";
        LoginClient.instance.isQueued = false;
        UIFindGame.instance.btnQueueGame.GetComponent<Button>().interactable = true;
        UIFindGame.instance.btnQuitQueue.SetActive(false);
        UIPrincipalPanel.HandleAlert(4, _packet);

    }
    public static void FriendRequest(Packet _packet)//ID:14
    {
        UIPrincipalPanel.HandleAlert(5, _packet);
    }
}
