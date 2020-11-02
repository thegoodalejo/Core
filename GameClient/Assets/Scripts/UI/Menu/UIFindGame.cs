using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFindGame : MonoBehaviour
{
    public static UIFindGame instance;
    public GameObject itemTemplate;
    public GameObject content;
    public Text txtQueueStatus;
    public Text txtMessageServer;
    public Text txtPlayersInGroup;
    public GameObject btnQueueGame;
    public GameObject btnAceptQueue;
    public GameObject btnQuitQueue;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance UIFindGame already exists, destroying object!");
            Destroy(this);
        }
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!LoginClient.instance.isLoadGroups) { return; }
        txtPlayersInGroup.text = LoginClient.instance.friends_in_group.Count.ToString();
        Debug.Log("UpdatingCuzGroupsFinish");
        LoginClient.instance.isLoadGroups = false;
        ResetFriends();
        AddFriends();

    }
    private void AddFriends()
    {
        foreach (FriendReference item in LoginClient.instance.friends_in_group)
        {
            Debug.Log($"F {item.id} {item.server_slot} {item.user_legends_nick}");
            GameObject friendsPrefab = Instantiate(itemTemplate, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            friendsPrefab.transform.SetParent(content.transform);
            FriendsDetails controller = friendsPrefab.GetComponent<FriendsDetails>();
            controller.txtName.text = item.user_legends_nick;
            controller.btnInvite.SetActive(false);
        }
        LoginClient.instance.isLoadGroups = false;
    }
    private void ResetFriends()
    {
        foreach (Transform item in content.transform)
        {
            GameObject.Destroy(item.gameObject);
        }
    }
    public static void FindGame()
    {
        Debug.Log("QueueForRandomMatch");
        LoginClientSend.QueueForRandomMatch();
    }

    public static void AceptQueue()
    {
        Debug.Log("QueueForRandomMatch");
        LoginClientSend.QuitQueue();
    }

    public static void QuitQueue()
    {
        Debug.Log("QueueForRandomMatch");
        LoginClientSend.QuitQueue();
    }
}
