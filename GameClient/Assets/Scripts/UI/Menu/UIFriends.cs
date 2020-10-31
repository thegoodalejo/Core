using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFriends : MonoBehaviour
{
    public static UIFriends instance;
    public GameObject itemTemplate;
    public GameObject content;

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
        //StartCoroutine(UpdateFriends());
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameInfo.isLoadFriends) { return; }
        ResetFriends();
        AddFriends();
        Debug.Log("UpdatingCuzFriendsFinish");

    }
    /*private IEnumerator UpdateFriends()
    {
        while (true)
        {
            loadFriends = true;
            Debug.Log("SpawnCoroutine");
            yield return new WaitForSeconds(10f);
        }

    }*/
    private void AddFriends()
    {
        foreach (FriendReference item in GameInfo.user_friends)
        {
            if (item.server_slot != 0)
            {
                Debug.Log($"F {item.id} {item.server_slot} {item.user_legends_nick}");
                GameObject friendsPrefab = Instantiate(itemTemplate, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                friendsPrefab.transform.SetParent(content.transform);
                FriendsDetails controller = friendsPrefab.GetComponent<FriendsDetails>();
                controller.txtName.text = item.user_legends_nick;
                controller.btnInvite.onClick.AddListener(() => { LoginClientSend.InviteFriendToGroup(item.server_slot); });
            }
        }
        GameInfo.isLoadFriends = false;
    }
    private void ResetFriends()
    {
        foreach (Transform item in content.transform)
        {
            GameObject.Destroy(item.gameObject);
        }
    }

}
