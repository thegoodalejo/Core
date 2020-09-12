using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Login : MonoBehaviour
{
    public static Login instance;

    public GameObject startMenu;
    public InputField usernameField;
    public InputField passwordField;

    private static string userName;
 
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    public void ConnectToGame()
    {
        startMenu.SetActive(false);
        usernameField.interactable = false;
        SetUserName(usernameField.text);
        Debug.Log("Click()");
        SceneManager.LoadScene("Menu");
    }

    private static void SetUserName(string _userName)
    {
        userName = _userName;
    }

    public static string GetUserName()
    {
        return userName;
    }
}
