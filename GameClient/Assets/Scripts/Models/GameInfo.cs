using System.Collections.Generic;

public class GameInfo
{
    public static string strPlayerName;
    public static string strUID;
    public static bool isGrouped;
    public static bool isQueued;
    public static bool isLoadFriends;
    public static List<FriendReference> user_friends { get; set; }

    // Static constructor initializes NumMembers
    static GameInfo()
    {
        strPlayerName = "Loading...";
        isGrouped = false;
        isQueued = false;
        isLoadFriends = false;
        user_friends = new List<FriendReference>();
    }
}
