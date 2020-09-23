using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System;
using System.Globalization;
using TMPro;
using UnityEngine.UI;

public class PopUpSystem : MonoBehaviour
{

    public GameObject popUpContainer;
    public Text popUpText;

    public void OpenPopUp(string _text)
    {
        if(popUpContainer != null)
        {
            popUpContainer.SetActive(true);
            popUpText.text = _text;
        }
        
    }

    public void UpdatePopUp(string _text)
    {
        if (popUpContainer != null)
        {
            popUpText.text = _text;
        }

    }

    public void ClosePopUp()
    {
        if (popUpContainer != null)
        {
            popUpContainer.SetActive(false);
        }

    }
}
