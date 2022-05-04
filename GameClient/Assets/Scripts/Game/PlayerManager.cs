using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int id;
    public string username;
    public float health;
    public float maxHealth = 100f;
    public int itemCount = 0;
    public GameObject model;
    private Vector3 localGravity = new Vector3(0,0,0);
    public GameObject sight;
    public float horizontalRotation = 0;
    public float verticalRotation = 0;
    private Vector3 oldPos = Vector3.zero; // en caso de ser util ?
    // public RaycastHit hit;
    private float _animationBlend;
    private AnimatorManager _animator;
    // animation IDs
        private int _animIDIdle;
        private int _animIDWalk;
        private int _animIDJump;
        private Vector3 lastPos;

        float threshold = 0.01f; // minimum displacement to recognize a 

    public void Initialize(int _id, string _username)
    {
        id = _id;
        username = _username;
        health = maxHealth;
        AssignAnimationIDs();
        lastPos = Vector3.zero;   
           
    }

    void Awake(){
        _animator = GetComponent<AnimatorManager>();  
    }

    void Update(){
        //
        
   }

    private void AssignAnimationIDs()
    {
        _animIDWalk = Animator.StringToHash("Walk");
        _animIDIdle = Animator.StringToHash("Idle");
        _animIDJump = Animator.StringToHash("Jump");
    }

    public void UpdateState(Vector3 _gravity){
        localGravity = _gravity;
        transform.rotation = Quaternion.FromToRotation(Vector3.down, localGravity);
        horizontalRotation = 0;
        verticalRotation = 0;
    }

    public void Move(Vector3 _newPosition){   
        Vector3 oldPost = transform.position;
        transform.position = _newPosition;

        //Global direction of object
        Vector3 moveGlobal = oldPost - transform.position;
        moveGlobal = moveGlobal.normalized;
        Vector3 objFwd = transform.forward;
        //Debug.Log(username + " GlobalDirection " + moveGlobal + " objFace " + objFwd);

        if (Mathf.Abs(moveGlobal.x) < threshold && Mathf.Abs(moveGlobal.z) < threshold){
            _animator.idle();
            return;
        } 
        if(true){
            _animator.walk();
        }else{
            _animator.back();
        }
    }

    public void SetHealth(float _health)
    {
        health = _health;
        if (health <= 0f)
        {
            Die();
        }
    }

    public void Die()
    {
        model.SetActive(false);
    }

    public void Respawn()
    {
        model.SetActive(true);
        SetHealth(maxHealth);
    }
}
