using UnityEngine;
using UnityEngine.UI;

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
