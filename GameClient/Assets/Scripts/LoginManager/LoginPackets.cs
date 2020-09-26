using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

/// <summary>Sent from server to client.</summary>
public enum LoginServerPackets
{
    welcome = 1,
    auth
}

/// <summary>Sent from client to server.</summary>
public enum LoginClientPackets
{
    welcomeReceived = 1
}


