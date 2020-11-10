﻿using System.Collections;
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
    public static async void Alert(Packet _packet)//ID:1
    {
        int _errCode = _packet.ReadInt();
        GameObject _alerPrefab = InitAlert();
        AlertDetail controller = _alerPrefab.GetComponent<AlertDetail>();
        controller.txtMessage.text = UIPrincipalPanel.errorCodes[_errCode];
        controller.btnConfirm.SetActive(false);
        _alerPrefab.SetActive(true);
        await Task.Delay(5000);
        Destroy(_alerPrefab);
    }
    public static async void GroupRequestResponse(Packet _packet)//ID:2
    {
        Debug.Log("GroupRequestResponse");
        List<FriendReference> _newGroup = new List<FriendReference>();
        int _fromFriend = _packet.ReadInt();
        int _groupSize = _packet.ReadInt();
        string _message;
        if (_groupSize < LoginClient.instance.friends_in_group.Count)
        {
            _message = $"{_fromFriend} leaves group";
        }else{
            _message = $"{_fromFriend} join group";
        }

        for (int i = 0; i < _groupSize; i++)
        {
            _newGroup.Add(new FriendReference(_packet.ReadInt(), _packet.ReadString()));
        }
        LoginClient.instance.friends_in_group = _newGroup;
        //------------------------------------
        GameObject _alerPrefab = InitAlert();
        AlertDetail controller = _alerPrefab.GetComponent<AlertDetail>();
        controller.txtMessage.text = _message;
        controller.btnConfirm.SetActive(false);
        _alerPrefab.SetActive(true);
        LoginClient.instance.groupSize++;
        LoginClient.instance.isLoadGroups = true;
        await Task.Delay(5000);
        Destroy(_alerPrefab);
    }
    public static async void GroupRequest(Packet _packet)//ID:3
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
    public static async void Message(Packet _packet)//ID:4
    {
        string _message = _packet.ReadString();
        Debug.Log(_message);
        GameObject _alerPrefab = InitAlert();
        AlertDetail controller = _alerPrefab.GetComponent<AlertDetail>();
        controller.txtMessage.text = _message;
        controller.btnConfirm.SetActive(false);
        _alerPrefab.SetActive(true);
        await Task.Delay(5000);
        Destroy(_alerPrefab);
    }
    public static async void FriendRequest(Packet _packet)//ID:5
    {
        int _fromFriend = _packet.ReadInt();
        string _message = _packet.ReadString();
        GameObject _alerPrefab = InitAlert();
        AlertDetail controller = _alerPrefab.GetComponent<AlertDetail>();
        controller.txtMessage.text = _message;
        controller.btnConfirm.GetComponent<Button>().onClick.AddListener(
            () =>
            {
                LoginClientSend.FriendRequestResponse(_fromFriend);
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
    public static async void GameRequest(Packet _packet)//ID:6
    {
        string _message = _packet.ReadString();
        GameObject _alerPrefab = InitAlert();
        AlertDetail controller = _alerPrefab.GetComponent<AlertDetail>();
        controller.txtMessage.text = _message;
        controller.btnConfirm.GetComponent<Button>().onClick.AddListener(
            () =>
            {
                LoginClientSend.GameResponse();
                DestroyImmediate(_alerPrefab);
            });
        controller.btnConfirm.SetActive(true);
        _alerPrefab.SetActive(true);
        await Task.Delay(9000);
        if (!instance.alertResponse)
        {
            Debug.Log($"Destroy");
            Destroy(_alerPrefab);
        }
    }
}



