using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MongoDB.Driver;
using System.Data.Common;

public class MenuUIManager : MonoBehaviour
{

    public static MenuUIManager instance;

    public GameObject mainMenu;
    public InputField usernameField;
    public Text text;

    public static string userNickName;

    public string queuePlayers;


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

    // Start is called before the first frame update
    void Start()
    {
        text.text = userNickName;
        Debug.Log($"UserToken {LoginClient.instance.token}");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void FindGame()
    {
        Debug.Log($"UserToken {LoginClient.instance.token} in Queue");
        LoginClientSend.QueueForRandomMatch();
    }
}
