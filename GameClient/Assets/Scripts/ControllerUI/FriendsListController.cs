using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendsListController : MonoBehaviour
{
    public GameObject ContentPanel;
    public GameObject ListItemPrefab;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log($"FriendsListController {GameInfo.user_friends.Count}");
        foreach (FriendReference _friend in GameInfo.user_friends)
        {
            GameObject newFriend = Instantiate(ListItemPrefab) as GameObject;
            FriendsDetails controller = newFriend.GetComponent<FriendsDetails>();
            controller.txtName.text = _friend.user_legends_nick;
            controller.btnInvite.onClick.AddListener(() => { });
            newFriend.transform.SetParent(ContentPanel.transform);
            newFriend.transform.localScale = Vector3.one;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }
}
