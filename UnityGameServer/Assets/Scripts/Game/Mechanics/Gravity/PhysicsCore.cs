using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class PhysicsCore : MonoBehaviour
{
    public Vector3 gravity = new Vector3(0,0,0);
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Physics.gravity = gravity;        
    }
}
