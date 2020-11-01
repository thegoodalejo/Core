using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class AlertManager : MonoBehaviour
{
    public static AlertManager instance;
    public GameObject alertContainer;
    public GameObject alertPrefab;

    public bool alertResponse;
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
        alertResponse = false;
    }
    private static GameObject InitAlert()
    {
        GameObject _alerPrefab;
        _alerPrefab = Instantiate(instance.alertPrefab, instance.alertContainer.transform.position, Quaternion.identity) as GameObject;
        _alerPrefab.transform.SetParent(instance.alertContainer.transform);
        _alerPrefab.SetActive(true);
        return _alerPrefab;
    }
    public static async void Error(Packet _packet)
    {
        int _errCode = _packet.ReadInt();
        Debug.Log($"Error: {UIPrincipalPanel.errorCodes[_errCode]}");
        GameObject _alerPrefab = InitAlert();
        AlertDetail controller = _alerPrefab.GetComponent<AlertDetail>();
        controller.txtMessage.text = UIPrincipalPanel.errorCodes[_errCode];
        controller.btnConfirm.SetActive(false);
        _alerPrefab.SetActive(true);
        await Task.Delay(5000);
        Destroy(_alerPrefab);
    }
    public static async void GroupRequestResponse(Packet _packet)
    {
        Debug.Log("GroupRequestResponse");
        List<FriendReference> _newGroup = new List<FriendReference>();
        int _fromFriend = _packet.ReadInt();
        int _groupSize = _packet.ReadInt();
        string _message;
        if (_groupSize < GameInfo.friends_in_group.Count)
        {
            _message = $"{_fromFriend} leaves group";
        }else{
            _message = $"{_fromFriend} join group";
        }

        for (int i = 0; i < _groupSize; i++)
        {
            _newGroup.Add(new FriendReference(_packet.ReadInt(), _packet.ReadString()));
        }
        GameInfo.friends_in_group = _newGroup;
        //------------------------------------
        GameObject _alerPrefab = InitAlert();
        AlertDetail controller = _alerPrefab.GetComponent<AlertDetail>();
        controller.txtMessage.text = _message;
        controller.btnConfirm.SetActive(false);
        _alerPrefab.SetActive(true);
        GameInfo.groupSize++;
        GameInfo.isLoadGroups = true;
        await Task.Delay(5000);
        Destroy(_alerPrefab);
    }
    public static async void GroupRequest(Packet _packet)
    {
        int _fromFriend = _packet.ReadInt();
        string _message = _packet.ReadString();
        GameObject _alerPrefab = InitAlert();
        AlertDetail controller = _alerPrefab.GetComponent<AlertDetail>();
        controller.txtMessage.text = _message;
        controller.btnConfirm.GetComponent<Button>().onClick.AddListener(
            () =>
            {
                LoginClientSend.InviteFriendToGroupResponse(_fromFriend);
                DestroyImmediate(_alerPrefab);
            });
        controller.btnConfirm.SetActive(true);
        _alerPrefab.SetActive(true);
        await Task.Delay(10000);
        if (!instance.alertResponse)
        {
            Debug.Log($"Destroy");
            Destroy(_alerPrefab);
        }
    }
}



