using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFriends : MonoBehaviour
{
    public static UIFriends instance;
    public GameObject friendsPrefab;
    public GameObject friendList;
    public GameObject inptSearchFriend;
    public GameObject btnSearchFriend;

    public bool loadFriends;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance UIFriends already exists, destroying object!");
            Destroy(this);
        }
    }
    void Start()
    {

    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter)) { SearchFriend(); }
        if (!LoginClient.instance.isLoadFriends) { return; }
        ResetFriends();
        AddFriends();
        Debug.Log("UpdatingCuzFriendsFinish");

    }
    private void AddFriends()
    {
        foreach (FriendReference item in LoginClient.instance.user_friends)
        {
            if (item.server_slot != 0)
            {
                Debug.Log($"F {item.id} {item.server_slot} {item.user_legends_nick}");
                GameObject _friendsPrefab = Instantiate(friendsPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                _friendsPrefab.transform.SetParent(friendList.transform);
                FriendsDetails controller = _friendsPrefab.GetComponent<FriendsDetails>();
                controller.txtName.text = item.user_legends_nick;
                controller.btnInvite.GetComponent<Button>().onClick.AddListener
                (() =>
                    {
                        LoginClientSend.InviteFriendToGroup(item.server_slot);
                    }
                );
            }
        }
        LoginClient.instance.isLoadFriends = false;
    }
    private void ResetFriends()
    {
        foreach (Transform item in friendList.transform)
        {
            GameObject.Destroy(item.gameObject);
        }
    }

    public static void SearchFriend()
    {
        if (instance.inptSearchFriend.GetComponent<InputField>().text != "")
        {
            string _searchFriend = instance.inptSearchFriend.GetComponent<InputField>().text.Trim();
            instance.inptSearchFriend.GetComponent<InputField>().text = "";
            LoginClientSend.SearchFriend(_searchFriend);
        }
    }

}
