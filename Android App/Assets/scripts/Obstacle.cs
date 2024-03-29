﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    PlayerMovement playerMovement;

    // Start is called before the first frame update
    void Start()
    {
        // Find playerMovement from the GameObject and set this local playerMovement to it
        playerMovement = GameObject.FindObjectOfType<PlayerMovement>();
    }

    // Function Name: OnCollisionEnter
    // Arg: Collision collision: what ran into the object (should only be the player object but should be checked anyways)
    // Desc: Terminates the player when an obstacle detects that the player has ran into it
    void OnCollisionEnter(Collision collision)
    { 
        // If the object that ran into the obstacle is the "Player"
        if(collision.gameObject.name == "Player")
        {
            // Run the "Die" function
            playerMovement.Die();
        }
        // terminate round

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
