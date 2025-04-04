using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int hitsTaken = 0;
    public float scaleIncreaseFactor = 1.1f;
    public Transform Player;
    public float speed = 0.5f; // Small speed for sluggish movement
    private Rigidbody rb;
    public ApplicationManager ap;

    void Start()
    {
        // Get Rigidbody component
        rb = GetComponent<Rigidbody>();

        // Ensure Player reference is set
        if (Player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                Player = playerObject.transform;
            }
        }

        // Find the ApplicationManager component on the GameObject with the "AP" tag
        GameObject apObject = GameObject.FindGameObjectWithTag("AP");
        if (apObject != null)
        {
            ap = apObject.GetComponent<ApplicationManager>();
        }
        else
        {
            Debug.LogError("ApplicationManager GameObject with tag 'AP' not found!");
        }

        // Make sure physics doesn't interfere
        if (rb != null)
        {
            rb.useGravity = false; // Prevent falling
            rb.isKinematic = false; // Allow physics movement
            rb.velocity = Vector3.zero; // Stop unwanted motion
        }
    }

    void FixedUpdate()
    {
        if (Player != null && speed > 0)
        {
            Vector3 direction = (Player.position - transform.position).normalized;
            rb.MovePosition(transform.position + direction * speed * Time.fixedDeltaTime);
        }
    }

    public int TakeHit()
    {
        hitsTaken++;
        speed *= 0.8f; // Reduce speed by 20% per hit
        return hitsTaken;
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collided with: " + other.gameObject.name);
        if (other.CompareTag("Player"))
        {
            if (ap != null) // Check if ap is not null before calling Stop()
            {
                ap.Gameover();
            }
            else
            {
                Debug.LogError("ApplicationManager component is null!");
            }

            Destroy(gameObject);
        }
    }
}