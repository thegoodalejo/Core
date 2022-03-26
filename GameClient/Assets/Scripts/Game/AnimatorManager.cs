using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    // Start is called before the first frame update

    private Animator _animator;
    public GameObject player;
    public bool _idle;

    void Awake(){
        _idle = true;
        _animator = GameObject.Find("/"+player.name+"ybot").GetComponent<Animator>();
        Debug.Log(player.name);
    }

    public void idle(){
        if(!_animator.GetBool("Idle")){
            Debug.LogWarning("Idleing");
            _animator.SetBool("Idle",true);
        }
    }
    public void walk(){
        if(_animator.GetBool("Idle")){
            Debug.LogWarning("Walking");
            _animator.SetBool("Idle",false);
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //_animator.SetBool("Idle",_idle);
    }
}
