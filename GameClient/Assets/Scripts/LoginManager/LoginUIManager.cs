using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LoginUIManager : MonoBehaviour
{
    public static LoginUIManager instance;

    public GameObject startMenu;
    public InputField usernameField;
    public InputField passwordField;

    public GameObject dialogueBox;
    public Text text;

    private static string userName;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance AUTH already exists, destroying object!");
            Destroy(this);
        }
    }

    public void ConnectToGame()
    {
        //startMenu.SetActive(false);
        usernameField.interactable = false;
        passwordField.interactable = false;
        SetUserName(usernameField.text);
        dialogueBox.SetActive(true);
        text.text = "Connecting to game ...";
        LoginClient.instance.ConnectToServer();
        text.text = "Authenticate ...";
        //LoginClientSend.Auth(usernameField.text, passwordField.text);

        //SceneManager.LoadScene("Menu");
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
