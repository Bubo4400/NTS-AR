using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int hitsTaken = 0;
    public float scaleIncreaseFactor = 1.1f;
    public Transform Player;
    public float speed = 1.0f;

    void Start()
    {
        if (Player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                Player = playerObject.transform;
            }
        }
    }

    void Update()
    {
        if (Player != null)
        {
            Vector3 direction = (Player.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
        }
    }
    
    public int TakeHit()
    {
        hitsTaken++;
        return hitsTaken;
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collided with: " + other.gameObject.name);
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}