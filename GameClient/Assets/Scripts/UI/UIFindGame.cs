using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFindGame : MonoBehaviour
{
    public static UIFindGame instance;
    public Text txtQueueStatus;
    public Text txtMessageServer;
    public Text txtPlayersInGroup;
    public Button btnQueueGame;
    public Button btnAceptQueue;
    public Button btnQuitQueue;
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
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public static void FindGame()
    {
        Debug.Log("QueueForRandomMatch");
        LoginClientSend.QueueForRandomMatch();
    }

    public static void AceptQueue()
    {
        Debug.Log("QueueForRandomMatch");
        LoginClientSend.QuitQueue();
    }

    public static void QuitQueue()
    {
        Debug.Log("QueueForRandomMatch");
        LoginClientSend.QuitQueue();
    }
}
