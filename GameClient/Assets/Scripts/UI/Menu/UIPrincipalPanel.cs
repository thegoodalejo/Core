using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPrincipalPanel : MonoBehaviour
{
    public static UIPrincipalPanel instance;
    public Button btnHome;
    public Button btnPlayGame;
    private delegate void MessageHandler(Packet _packet);
    private static Dictionary<int, MessageHandler> messageHandlers;
    public static Dictionary<int, string> errorCodes;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance MENU already exists, destroying object!");
            Destroy(this);
        }
        InitializeAlertData();
        InitializeErrorCodes();
    }
    public static void HandleAlert(int _id, Packet _packet)
    {
        messageHandlers[_id](_packet);
    }
    /// <summary>Initializes all necessary alert data.</summary>
    private void InitializeAlertData()
    {
        messageHandlers = new Dictionary<int, MessageHandler>()
        {
            { (int)AlertServerPackets.alert, AlertManager.Alert },
            { (int)AlertServerPackets.groupRequestResponse, AlertManager.GroupRequestResponse },
            { (int)AlertServerPackets.groupRequest, AlertManager.GroupRequest },
            { (int)AlertServerPackets.message, AlertManager.Message },
            { (int)AlertServerPackets.friendRequest, AlertManager.FriendRequest },
        };
    }
    private void InitializeErrorCodes()
    {
        errorCodes = new Dictionary<int, string>()
        {
            { (int)ErrorCodes.General, "Internal Server Error" },
            { (int)ErrorCodes.NoGroup, "No eres lider de un grupo" },
            { (int)ErrorCodes.GroupDisolved, "Grupo disuelto" },
            { (int)ErrorCodes.PlayerInGroup, "Jugador en un grupo" },
            { (int)ErrorCodes.PlayerInQueue, "Jugador en fila" },
            { (int)ErrorCodes.PlayerNotFound, "Jugador NoEncontrado/Offline" },
        };
    }
}
