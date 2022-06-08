using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //player info
    public int id;
    public string username;
    private bool[] inputs;  
    public float health;
    public float maxHealth = 100f;
    public GameObject sight;

    [Header("Player")]
    [Tooltip("Move speed of the character in m/s")]
    public float MoveSpeed = 2.0f;
    [Tooltip("Sprint speed of the character in m/s")]
    public float SprintSpeed = 5.335f;
    [Tooltip("How fast the character turns to face movement direction")]
    [Range(0.0f, 0.3f)]
    public float RotationSmoothTime = 0.12f;
    [Tooltip("Acceleration and deceleration")]
    public float SpeedChangeRate = 10.0f;

    [Space(10)]
    [Tooltip("The height the player can jump")]
    public float JumpHeight = 2f;
    [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
    public float Gravity = -15.0f;

    [Space(10)]
    [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
    public float JumpTimeout = 0.25f;
    [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
    public float FallTimeout = 0.15f;

    [Header("Player Grounded")]
    [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
    public bool Grounded = true;
    [Tooltip("Useful for rough ground")]
    public float GroundedOffset = -0.14f;
    [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
    public float GroundedRadius = 0.28f;
    [Tooltip("What layers the character uses as ground")]
    public LayerMask GroundLayers;

    [Header("Cinemachine")]
    [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
    public GameObject CinemachineCameraTarget;
    [Tooltip("How far in degrees can you move the camera up")]
    public float TopClamp = 70.0f;
    [Tooltip("How far in degrees can you move the camera down")]
    public float BottomClamp = -30.0f;
    [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
    public float CameraAngleOverride = 0.0f;
    [Tooltip("For locking the camera position on all axis")]
    public bool LockCameraPosition = false;

    // player
    private float _speed;
    private float _animationBlend;
    private float _targetRotation = 0.0f;
    private float _rotationVelocity;
    private float _verticalVelocity;
    private float _terminalVelocity = 53.0f;

    // timeout deltatime
    private float _jumpTimeoutDelta;
    private float _fallTimeoutDelta;
    
    // ????
    private const float _threshold = 0.01f;
    private CharacterController _controller;

    public void Initialize(int _id, string _username)
    {
        id = _id;
        username = _username;
        health = maxHealth;
        inputs = new bool[5];
    }
    private void Start()
    {
        _controller = GetComponent<CharacterController>();

        // reset our timeouts on start
        _jumpTimeoutDelta = JumpTimeout;
        _fallTimeoutDelta = FallTimeout;
    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
		{
			if (lfAngle < -360f) lfAngle += 360f;
			if (lfAngle > 360f) lfAngle -= 360f;
			return Mathf.Clamp(lfAngle, lfMin, lfMax);
		}

    public void UpdateState(Vector3 _gravity){
        //localGravity = _gravity;
        //transform.rotation = Quaternion.FromToRotation(Vector3.down, localGravity);
        //SendToClient.PlayerGravity(id,_gravity);
    }

    /// <summary>Processes player input and moves the player.</summary>
    public void FixedUpdate()
    {
        JumpAndGravity();
		GroundedCheck();
		Move();     
        MoveRefresh();
    }
    private void GroundedCheck()
    {
        // set sphere position, with offset
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
        Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);
    }
    private void JumpAndGravity()
		{
			if (Grounded)
			{
				// reset the fall timeout timer
				_fallTimeoutDelta = FallTimeout;

				// stop our velocity dropping infinitely when grounded
				if (_verticalVelocity < 0.0f)
				{
					_verticalVelocity = -2f;
				}

				// Jump
				if (inputs[4] && _jumpTimeoutDelta <= 0.0f)
				{
					// the square root of H * -2 * G = how much velocity needed to reach desired height
					_verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);
                    Debug.Log("//Jump");
				}

				// jump timeout
				if (_jumpTimeoutDelta >= 0.0f)
				{
					_jumpTimeoutDelta -= Time.deltaTime;
				}
			}
			else
			{
				// reset the jump timeout timer
				_jumpTimeoutDelta = JumpTimeout;

				// fall timeout
				if (_fallTimeoutDelta >= 0.0f)
				{
					_fallTimeoutDelta -= Time.deltaTime;
				}

				// if we are not grounded, do not jump
				inputs[4] = false;
			}

			// apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
			if (_verticalVelocity < _terminalVelocity)
			{
				_verticalVelocity += Gravity * Time.deltaTime;
			}
		}



    /// <summary>Calculates the player's desired movement direction and moves him.</summary>
    /// <param name="_inputDirection"></param>
    private void Move()
    {    
        float moveX = 0f;
        float moveY = 0f;
        if(inputs[0]){ //Front
            moveY =+ 1.0f;
            Debug.Log("//Front");
        }
        if(inputs[1]){ //Back
            moveY =- 1.0f;
            Debug.Log("//Back");
        }
        if(inputs[2]){ //Left
            moveX =- 1.0f;
            Debug.Log("//Left");
        }
        if(inputs[3]){ //Rigth
            moveX =+ 1.0f;
            Debug.Log("//Rigth");
        }

        // normalise input direction
        Vector3 inputDirection = Vector3.zero;
        inputDirection += transform.forward * moveY;
        inputDirection += transform.right * moveX;
        
        _controller.Move(inputDirection * (MoveSpeed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
    }

    private void MoveRefresh(){
        SendToClient.PlayerPosition(this);
        SendToClient.PlayerRotation(this,false);
    }

    /// <summary>Updates the player input with newly received input.</summary>
    /// <param name="_inputs">The new key inputs.</param>
    /// <param name="_rotation">The new rotation.</param>
    public void SetInput(bool[] _inputs, Vector3 _rotation,Vector3 _sight)
    {
        inputs = _inputs; 
        //transform.rotation = Quaternion.Euler(_rotation);
        transform.rotation = Quaternion.Euler(_rotation);
        sight.transform.rotation = Quaternion.Euler(_sight);
    }

    public void Shoot(Vector3 _viewDirection)
    {
        if (health <= 0f)
        {
            return;
        }
        NetworkManager.instance.InstantiateProjectile(sight.transform).Initialize(_viewDirection, 1000.0f, id);
        if (Physics.Raycast(sight.transform.position, _viewDirection, out RaycastHit _hit, 25f))
        {
            if (_hit.collider.CompareTag("Player"))
            {
                _hit.collider.GetComponent<Player>().TakeDamage(50f);
            }
            else if (_hit.collider.CompareTag("Enemy"))
            {
                _hit.collider.GetComponent<Enemy>().TakeDamage(50f);
            }
        }
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
        if (health <= 0f)
        {
            return;
        }
      health -= _damage;
        if (health <= 0f)
        {
            health = 0f;
            _controller.enabled = false;
            transform.position = new Vector3(0f, 25f, 0f);
            SendToClient.PlayerPosition(this);
            StartCoroutine(Respawn());
        }
      SendToClient.PlayerHealth(this);
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(5f);

        health = maxHealth;
        _controller.enabled = true;
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
        Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
			Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

			if (Grounded) Gizmos.color = transparentGreen;
			else Gizmos.color = transparentRed;
			
			// when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
			Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z), GroundedRadius);
	}
}
