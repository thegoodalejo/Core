using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginUIManager : MonoBehaviour
{
    public static LoginUIManager instance;

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
            Debug.Log("Instance AUTH already exists, destroying object!");
            Destroy(this);
        }
    }

    public void ConnectToGame()
    {
        startMenu.SetActive(false);
        usernameField.interactable = false;
        SetUserName(usernameField.text);
        Debug.Log("Click()");
        LoginClient.instance.ConnectToServer();
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
