using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneManagerC : MonoBehaviour
{
    public static ZoneManagerC instance;
    public GameObject _zonePrefab;
    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
        Zone.size = 15;
    }
    public Zone InstantiateZone()
    {
        return Instantiate(_zonePrefab, new Vector3(0, 0, 0), Quaternion.identity).GetComponent<Zone>();
    }
    void Start()
    {
        Zone.size = 25;
        Zone.gForce = 15;
        MakeCube();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private static void MakeCube(){
        Vector3[] veticesRigth = new Vector3[] {
            new Vector3(1,1,1),
            new Vector3(1,1,-1),
            new Vector3(1,-1,-1),
            new Vector3(1,-1,1),
            };//RIGTH
        Zone _rigth = instance.InstantiateZone();
        _rigth.Initialize(veticesRigth, Vector3.right ,false);

        Vector3[] veticesLeft = new Vector3[] {
            new Vector3(-1,1,1),
            new Vector3(-1,1,-1),
            new Vector3(-1,-1,-1),
            new Vector3(-1,-1,1),
            };//LEFT
        Zone _left = instance.InstantiateZone();
        _left.Initialize(veticesLeft, Vector3.left,true);

        Vector3[] veticesBot = new Vector3[] {
            new Vector3(1,-1,1),
            new Vector3(1,-1,-1),
            new Vector3(-1,-1,-1),
            new Vector3(-1,-1,1),
            };//BOT
        Zone _bot = instance.InstantiateZone();
        _bot.Initialize(veticesBot, Vector3.down ,false);

        Vector3[] veticesTop = new Vector3[] {
            new Vector3(1,1,1),
            new Vector3(1,1,-1),
            new Vector3(-1,1,-1),
            new Vector3(-1,1,1),
            };//TOP
        Zone _top = instance.InstantiateZone();
        _top.Initialize(veticesTop, Vector3.up ,true);

        Vector3[] veticesBack = new Vector3[] {
            new Vector3(1,1,-1),
            new Vector3(1,-1,-1),
            new Vector3(-1,-1,-1),
            new Vector3(-1,1,-1),
            };//BACK
        Zone _back = instance.InstantiateZone();
        _back.Initialize(veticesBack, Vector3.back ,true);

        Vector3[] veticesFront = new Vector3[] {
            new Vector3(1,1,1),
            new Vector3(1,-1,1),
            new Vector3(-1,-1,1),
            new Vector3(-1,1,1),
            };//TOP
        Zone _front = instance.InstantiateZone();
        _front.Initialize(veticesFront, Vector3.forward ,false);
    }
}
