using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFriends : MonoBehaviour
{
    public static UIFriends instance;
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
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
