using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendReference
{
    public string id { get; set; }
    public int server_slot { get; set; }
    public string user_legends_nick { get; set; }
    public FriendReference(string _id, int _server_slot, string _user_legends_nick)
    {
        id = _id;
        server_slot = _server_slot;
        user_legends_nick = _user_legends_nick;
    }
}
