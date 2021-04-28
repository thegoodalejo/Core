using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public PlayerManager player;
    [SerializeField][Range(0,2)]
    public float sensitivity = 1f;
    public float clampAngle = 65f;
    public float vertCounter = 0f;

    private float oldVerticalRot;
    
    private float oldHorizontalRot;

    private void Start()
    {
        player.verticalRotation = 0f;
        player.horizontalRotation = 0f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleCursorMode();
        }
        
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            Look();
        }
    }

    private void Look()
    {
        float _mouseVertical = -Input.GetAxis("Mouse Y");
        float _mouseHorizontal = Input.GetAxis("Mouse X");        

        player.horizontalRotation += (_mouseHorizontal-oldHorizontalRot)*sensitivity;
        oldHorizontalRot=_mouseHorizontal;
        player.transform.RotateAround(player.transform.position, player.transform.up, player.horizontalRotation);

        player.verticalRotation += (_mouseVertical-oldVerticalRot)*sensitivity;
        // player.verticalRotation = Mathf.Clamp(player.verticalRotation, -clampAngle, clampAngle);
        oldVerticalRot=_mouseVertical;
        vertCounter += player.verticalRotation;
        if(vertCounter > clampAngle) {
            vertCounter = clampAngle;
            return;
            };
        if(vertCounter < -clampAngle) {
            vertCounter = -clampAngle;
            return;
            }
        player.sight.transform.RotateAround(player.transform.position, player.transform.right, player.verticalRotation);
        
        // localRotation = Quaternion.AngleAxis(player.verticalRotation,transform.right);
        // transform.localRotation = RotateAround(transform.position, transform.right, player.verticalRotation);
    }
    private void ToggleCursorMode()
    {
        Cursor.visible = !Cursor.visible;

        if (Cursor.lockState == CursorLockMode.None)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
