﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

/// <summary>Sent from server to client.</summary>
public enum LoginServerPackets
{
    welcome = 1,
    test,
    auth,
    queueUpdate,
    gameFound,
    groupCreated,
    groupDisolved,
    friendList,
    groupInvited,
    groupInvitedResponse,
    updateFriendStatus,
    updateGroupStatus,
    queueCanceled,
}

/// <summary>Sent from client to server.</summary>
public enum LoginClientPackets
{
    welcomeReceived = 1,
    queueRequestForRandomMatch,
    trashRequest,
    quitQueue,
    groupRequest,
    disolveGroupRequest,
    inviteFriendToGroup,
    inviteFriendToGroupResponse
}
public enum AlertServerPackets
{
    message = 1,
    groupRequestResponse,
    groupRequest
}
public enum ErrorCodes
{
    General = 1,
    NoGroup,
    GroupDisolved,
    PlayerInGroup,
}


