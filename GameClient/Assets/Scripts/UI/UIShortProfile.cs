using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIShortProfile : MonoBehaviour
{
    public static UIShortProfile instance;
    public Text txtPlayerName;
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
        txtPlayerName.text = GameInfo.StrPlayerName;
    }
    void Update()
    {

    }
    public static void FindGame()
    {
        
    }
}
