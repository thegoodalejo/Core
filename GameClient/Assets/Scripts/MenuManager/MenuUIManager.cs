using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data.Common;

public class MenuUIManager : MonoBehaviour
{

    public static MenuUIManager instance;

    [SerializeField]
    private GameObject mainMenu;
    [SerializeField]
    private GameObject principalPanel;
    [SerializeField]
    private GameObject shortProfile;
    [SerializeField]
    private GameObject friends;

    //Panel Buttons
    public GameObject findGameMenu;
    public GameObject homeMenu;




    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance MenuUIManager already exists, destroying object!");
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        LoadHome();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public static void LoadHome()
    {
        UIPrincipalPanel.instance.btnHome.interactable = false;
        UIPrincipalPanel.instance.btnPlayGame.interactable = true;
        MenuUIManager.instance.findGameMenu.SetActive(false);
        MenuUIManager.instance.homeMenu.SetActive(true);
    }
    public static void LoadPlayGame()
    {
        if (!GameInfo.isGrouped)
        {
            LoginClientSend.GroupRequest();
        }
        else
        {
            UIPrincipalPanel.instance.btnHome.interactable = true;
            UIPrincipalPanel.instance.btnPlayGame.interactable = false;
            MenuUIManager.instance.findGameMenu.SetActive(true);
            MenuUIManager.instance.homeMenu.SetActive(false);
        }
    }
    public static void LoadGroupGame()
    {
        UIPrincipalPanel.instance.btnHome.interactable = true;
        UIPrincipalPanel.instance.btnPlayGame.interactable = false;
        MenuUIManager.instance.findGameMenu.SetActive(true);
        MenuUIManager.instance.homeMenu.SetActive(false);
        UIFindGame.instance.btnQueueGame.SetActive(false);
    }
}
