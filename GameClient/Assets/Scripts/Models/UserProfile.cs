using MongoDB.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserProfile 
{
    public string id { set; get; }
    public string userNickName { private set; get; }
    public bool acc_aviable { private set; get; }

    public UserProfile(string _id, string _userName, bool _acc_aviable)
    {
        id = _id;
        userNickName = _userName;
        acc_aviable = _acc_aviable;
    }
}
