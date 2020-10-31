using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlertManager : MonoBehaviour
{
    public static AlertManager instance;
    public GameObject alertContainer;
    public GameObject alertPrefab;
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
    private static GameObject InitAlert()
    {
        GameObject friendsPrefab;
        friendsPrefab = Instantiate(instance.alertPrefab, instance.alertContainer.transform.position, Quaternion.identity) as GameObject;
        friendsPrefab.transform.SetParent(instance.alertContainer.transform);
        friendsPrefab.SetActive(true);
        return friendsPrefab;
    }
    public static void Error(Packet _packet)
    {
        int _errCode = _packet.ReadInt();
        Debug.Log($"Error: {UIPrincipalPanel.errorCodes[_errCode]}");
        GameObject friendsPrefab = InitAlert();
        AlertDetail controller = friendsPrefab.GetComponent<AlertDetail>();
        controller.txtMessage.text = UIPrincipalPanel.errorCodes[_errCode];
        controller.btnAcept.SetActive(false);
        controller.btnCancel.SetActive(false);
        controller.btnConfirm.SetActive(false);
        friendsPrefab.SetActive(true);
    }
    public static void FriendRequest(Packet _packet)
    {
        string _message = _packet.ReadString();
        GameObject friendsPrefab = InitAlert();
        AlertDetail controller = instance.alertPrefab.GetComponent<AlertDetail>();
        controller.txtMessage.text = _message;
        friendsPrefab.SetActive(true);
    }
    public static void GroupRequest(Packet _packet)
    {
        int _fromFriend = _packet.ReadInt();
        string _message = _packet.ReadString();
        Debug.Log($"ID Slot {_fromFriend} : {_message}");
        GameObject friendsPrefab = InitAlert();
        AlertDetail controller = friendsPrefab.GetComponent<AlertDetail>();
        controller.txtMessage.text = _message;
        controller.btnConfirm.SetActive(false);
        friendsPrefab.SetActive(true);
    }

}



