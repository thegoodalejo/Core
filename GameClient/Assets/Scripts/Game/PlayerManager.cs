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
    // public RaycastHit hit;

    public void Initialize(int _id, string _username)
    {
        id = _id;
        username = _username;
        health = maxHealth;
    }

    public void UpdateState(Vector3 _gravity){
        localGravity = _gravity;
        transform.rotation = Quaternion.FromToRotation(Vector3.down, localGravity);
        horizontalRotation = 0;
        verticalRotation = 0;
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
