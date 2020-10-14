using System.Net;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginHandler : MonoBehaviour
{
    public static void Welcome(Packet _packet)
    {
        string _msg = _packet.ReadString();
        int _myId = _packet.ReadInt();

        Debug.Log($"Message from server: {_msg}");
        LoginClient.instance.myId = _myId;
        LoginUIManager.instance.text.text = "auth";
        LoginClientSend.WelcomeReceived();
    }
    public static void AuthResponse(Packet _packet)
    {
        UserProfile _userResponse = _packet.ReadUser();

        Debug.Log($"Message from Auth server: {_userResponse.userAuthState}");
        Debug.Log($"Message from Auth server: {_userResponse.id}");
        Debug.Log($"Message from Auth server: {_userResponse.userNickName}");
        // Now that we have the client's id, connect UDP
        if (_userResponse.userAuthState)
        {
            LoginClient.instance.udp.Connect(((IPEndPoint)LoginClient.instance.tcp.socket.Client.LocalEndPoint).Port);
            MenuUIManager.userNickName = _userResponse.userNickName;
            LoginClient.instance.token = _userResponse.id;
            SceneManager.LoadScene("Menu");
        }
        else
        {
            LoginClient.instance.tcp.socket.Close();
            LoginUIManager.instance.text.text = "Invalid credentials";
            LoginUIManager.instance.usernameField.interactable = true;
            LoginUIManager.instance.passwordField.interactable = true;
        }
    }
}
