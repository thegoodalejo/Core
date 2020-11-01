using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendReference
{
    public string id { get; set; }
    public int server_slot { get; set; }
    public string user_legends_nick { get; set; }
    public bool on_my_group { get; set; }
    public FriendReference(int _server_slot, string _user_legends_nick)
    {
        id = null;
        server_slot = _server_slot;
        user_legends_nick = _user_legends_nick;
    }
    public FriendReference(string _id, int _server_slot, string _user_legends_nick, bool _on_my_group)
    {
        id = _id;
        server_slot = _server_slot;
        user_legends_nick = _user_legends_nick;
        on_my_group = _on_my_group;
    }
}
