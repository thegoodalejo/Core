using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIShortProfile : MonoBehaviour
{
    public static UIShortProfile instance;
    public Text txtPlayerName;
    public GameObject btnLogOut;
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
        txtPlayerName.text = LoginClient.instance.strPlayerName;
    }
    void Update()
    {

    }
    public static void LogOut()
    {
        LoginClient.instance.LogOut();
    }
}
