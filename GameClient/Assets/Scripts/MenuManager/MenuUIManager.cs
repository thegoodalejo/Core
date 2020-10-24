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
    [SerializeField]
    private GameObject findGameMenu;
    [SerializeField]
    private GameObject homeMenu;
    



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

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public static void LoadPlayGame(){
        UIPrincipalPanel.instance.btnPlayGame.enabled = false;
        LoginClientSend.GroupRequest();
        MenuUIManager.instance.homeMenu.SetActive(false);
        MenuUIManager.instance.findGameMenu.SetActive(true);
    }
}
