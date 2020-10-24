using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInfo
{
    public static string StrPlayerName;

        // Static constructor initializes NumMembers
        static GameInfo()
        {
            StrPlayerName = "Loading...";
        }
}
