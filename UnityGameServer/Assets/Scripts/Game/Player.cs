using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int id;
    public string username;
    public Vector3 localGravity = new Vector3(0,0,0);
    public GameObject front;
    public GameObject back;
    public GameObject head;
    public GameObject rigth;
    public GameObject left;
    public GameObject bot;
    public GameObject sight;
    
    // public CharacterController controller;
    // public Transform shootOrigin;
    // public float gravity = -9.81f;
    public float moveSpeed = 0.5f;
    private Rigidbody rb;
    public float jumpSpeed = 15f;
    public float health;
    public float maxHealth = 100f;
    public bool isGrounded;

    private bool[] inputs;    

    private void Start()
    {
        moveSpeed *= Time.fixedDeltaTime;
        isGrounded = false;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    public void Initialize(int _id, string _username)
    {
        id = _id;
        username = _username;
        health = maxHealth;

        inputs = new bool[5];
    }

    public void UpdateState(Vector3 _gravity){
        localGravity = _gravity;
        transform.rotation = Quaternion.FromToRotation(Vector3.down, localGravity); 
        // GetComponent<Camera>().transform.rotation = Quaternion.FromToRotation(Vector3.down, localGravity);
        SendToClient.PlayerGravity(id,_gravity);
    }

    /// <summary>Processes player input and moves the player.</summary>
    public void FixedUpdate()
    {        
        Vector3 tempVect = Vector3.zero;
                
        isGrounded = Physics.Raycast(transform.position,bot.transform.position,1.7f);
        if(!isGrounded){
            tempVect += localGravity;
        }
        if (inputs[4] && isGrounded)
        {
            Debug.Log("Imaginarie Jump");
            tempVect += -localGravity*jumpSpeed;
        }
        if(inputs[0]){
            Debug.Log($"MoveForward {transform.position} to {front.transform.position} Rot {transform.rotation}");
            transform.position = front.transform.position;
        }
        if(inputs[1]){
            Debug.Log($"MoveBackwards {transform.position} to {back.transform.position} Rot {transform.rotation}");
            transform.position = back.transform.position;
        }
        if(inputs[3]){
            Debug.Log($"MoveRigth {transform.position} to {rigth.transform.position} Rot {transform.rotation}");
            transform.position = rigth.transform.position;
        }
        if(inputs[2]){
            Debug.Log($"MoveLeft {transform.position} to {left.transform.position} Rot {transform.rotation}");
            transform.position = left.transform.position;
        }

        Physics.gravity = localGravity+tempVect;
        Move(tempVect);        
    }


    /// <summary>Calculates the player's desired movement direction and moves him.</summary>
    /// <param name="_inputDirection"></param>
    private void Move(Vector2 _inputDirection)
    {
        SendToClient.PlayerPosition(this);
        SendToClient.PlayerRotation(this,false);
    }

    /// <summary>Updates the player input with newly received input.</summary>
    /// <param name="_inputs">The new key inputs.</param>
    /// <param name="_rotation">The new rotation.</param>
    public void SetInput(bool[] _inputs, Vector3 _rotation,Vector3 _sight)
    {
        inputs = _inputs;
        transform.rotation = Quaternion.Euler(_rotation);
        sight.transform.rotation = Quaternion.Euler(_sight);
    }

    public void Shoot(Vector3 _viewDirection)
    {
        // if (health <= 0f)
        // {
        //     return;
        // }

        // if (Physics.Raycast(shootOrigin.position, _viewDirection, out RaycastHit _hit, 25f))
        // {
        //     if (_hit.collider.CompareTag("Player"))
        //     {
        //         _hit.collider.GetComponent<Player>().TakeDamage(50f);
        //     }
        //     else if (_hit.collider.CompareTag("Enemy"))
        //     {
        //         _hit.collider.GetComponent<Enemy>().TakeDamage(50f);
        //     }
        // }
    }

    public void ThrowItem(Vector3 _viewDirection)
    {
        // if (health <= 0f)
        // {
        //     return;
        // }

        // if (itemAmount > 0)
        // {
        //     itemAmount--;
        //     NetworkManager.instance.InstantiateProjectile(shootOrigin).Initialize(_viewDirection, throwForce, id);
        // }
    }

    public void TakeDamage(float _damage)
    {
        // if (health <= 0f)
        // {
        //     return;
        // }

        // health -= _damage;
        // if (health <= 0f)
        // {
        //     health = 0f;
        //     controller.enabled = false;
        //     transform.position = new Vector3(0f, 25f, 0f);
        //     SendToClient.PlayerPosition(this);
        //     StartCoroutine(Respawn());
        // }

        // SendToClient.PlayerHealth(this);
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(5f);

        health = maxHealth;
        //controller.enabled = true;
        SendToClient.PlayerRespawned(this);
    }

    public bool AttemptPickupItem()
    {
        // if (itemAmount >= maxItemAmount)
        // {
        //     return false;
        // }

        // itemAmount++;
        return true;
    }
    private void OnDrawGizmos () {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position+transform.up * 2 ,0.2f);
        Gizmos.color = Color.green;
		Gizmos.DrawLine(transform.position ,transform.position+transform.up * 2);

        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(rigth.transform.position,0.1f);
        Gizmos.color = Color.white;
		Gizmos.DrawLine(transform.position ,rigth.transform.position);

        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(left.transform.position,0.1f);
        Gizmos.color = Color.black;
		Gizmos.DrawLine(transform.position ,left.transform.position);
	}
}
