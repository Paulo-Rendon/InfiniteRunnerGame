using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The collectable coins
public class Coin : MonoBehaviour
{
    [SerializeField] float turnSpeed = 90f;


// Function Name: OnTriggerEnter
// Arguments: collider other: The object that is colliding into the coin object
// Desc: This function is called when an object collides with a coin, typically the player object
    private void OnTriggerEnter(Collider other)
    {

        // If the "other" is an obstacle, destroy the coin
        if(other.gameObject.GetComponent<Obstacle>() != null)
        {
            Destroy(gameObject);
            return;
        }

        // If the "other" is not the player, do nothing and leave the function
        if (other.gameObject.name != "Player")
        {
            return;
        }

        // Else (the "other" is the player object) destroy the coin and increment the coin counter
        Destroy(gameObject);
        GameManager.inst.IncrementCoin();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    // rotate the coin
    void Update()
    {
        transform.Rotate(0, 0, turnSpeed * Time.deltaTime);
    }
}
