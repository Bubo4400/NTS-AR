using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int hitsTaken = 0;
    public float scaleIncreaseFactor = 1.1f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public int TakeHit()
    {
        hitsTaken++;

        return hitsTaken;
    }
}
