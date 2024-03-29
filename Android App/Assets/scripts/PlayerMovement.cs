﻿using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerMovement : MonoBehaviour
{
    // bool stores if player is still alive for the round
    bool alive = true;
    // stores player movement speed (on x axis?)
    public float speed = 5;

    // SerializeField means it is accessable from Unity but not other scripts
    // Like a limited version of the public descriptor
    [SerializeField] Rigidbody rb; // The playerObject
    [SerializeField] LayerMask coins;

    private Animator animator;

    // Stores the user horizontalInput (arrow keys)
    float horizontalInput;
    // How much the player moves horizontally per button press (on the rails)
    [SerializeField] float horizontalMultiplier = 1;

    // How much the player speed should increase per point
    public float speedIncreasePerPoint = 0.1f;

    // PLayer's horizontal position (starts in middle rails)
    float horizontalPos = 1;

    // Controls players horizontal momentem, makes the movement more smooth
    float horizontalMomentum = 0;

    // If the player can still move horizontally (stops when the button is held to help avoid accidential movement)
    bool canMove = true;

    float magTime = 0f;
    public void MagTime(float val){
        magTime = val;
    }
    bool magActive = false;
    float shieldTime = 0f;
    public void ShieldTime(float val){
        shieldTime = val;
    }
    bool shieldActive = false;
    float invincTime = 0f;
    public void InvincTime(float val){
        invincTime = val;
    }
    bool invincActive = false;
    
    void Start(){
        animator = GetComponent<Animator>();
        animator.SetBool("isDead", false);
    }

    void OnDrawGizmos()
    {
        //Gizmos.DrawWireSphere(rb.position, 7f);
    }

    private void FixedUpdate()
    {
        if (!alive) return; // If the player is not alive, dont run

        // Calculates players forward movement
        Vector3 forwardMove = (transform.forward * speed * Time.fixedDeltaTime);

        if(horizontalInput == 0f)   // If no input detected, then the player can move
            canMove = true;
        else if(horizontalInput < 0 && canMove){    // If there is a negative input and the player can move
            horizontalInput = -1;   // round the input to -1 (helps with keeping the player on the rails without needing a full key press)
            canMove = false;    // canMove becomes false till the key is released again
        }
        else if (canMove){  // If the hoirzontal input is not zero, is not negative, but canMove is true then there must be a positive input
            horizontalInput = 1;    // Round the horizontalInput up to 1
            canMove = false;    // canMove becomes false till the key press is released again
        }
        else    // Whenever canMove is false, stop any read input
            horizontalInput = 0;
        
        // Change the horizontal position is the horizontalPos with the player input calculated
        float tmp = horizontalPos + horizontalInput;
        if(horizontalInput != 0 && tmp < 3 && tmp > -1) // If there is a horizontal input and the next position is valid based on the rails regulation
        {
            // Update the horizontal position
            horizontalPos = tmp;
        }

        // This keeps the player rig on the rails
        // If the player x position is less than the rails location it should be on
        if((float)rb.position.x < (float)GameManager.rails[(int)horizontalPos] - 0.3f){
                horizontalMomentum = 1; // The player should keep moving to the right till it alligns with the rail
        }
        // If the player x position is more than the rails location it should be on
        else if((float)rb.position.x > (float)GameManager.rails[(int)horizontalPos] + 0.3f){
                horizontalMomentum = -1;    // The player should keep moving to the left till it alligns with the rail
        }

        else
            horizontalMomentum = 0; // No movement needed otherwise

        // Moves the player horizontally based on all the calculations made
        // transform right * horizontalMomentum (to keep player on rails and read player input) * speed (allows for smooth movement at any speed) * 
        // fixedDeltaTime (allows for smooth movement based on framerate) * horizontalMultiplier (always 1 unless manually changed)
        Vector3 horizontalMove = transform.right * horizontalMomentum * speed * Time.fixedDeltaTime * horizontalMultiplier;
        rb.MovePosition(rb.position - forwardMove - horizontalMove);    // Actually moves player rig based on previous line calcs
    }


    // Update is called once per frame
    private void Update()
    {
        // Read player's input and store in horizontalInput
        horizontalInput = Input.GetAxis("Horizontal");

        if (transform.position.y < -5)  // If player drops in y axis below the ground (below the Groundtiles), kill the player
        {
                                        // Shouldn't happen unless an issue occurs with a game function so should be checked
            Die();  // Call the Die function
        }
        if(magTime > 0f && !magActive)
            StartCoroutine(actMagnet());
        if(shieldTime > 0f && !shieldActive)
            StartCoroutine(actShield());
        if(invincTime > 0f && !invincActive)
            StartCoroutine(actInvinc());
    }

    // Function Name: Die
    // arg: None
    // Desc: Restarts the game when the player dies
    public void Die()
    {   animator.SetBool("isDead", true);
        alive = false;  // Sets the alive bool to false
        Invoke("Restart", 2);   // Run the Restart function
    }

    // Function Name: Restart
    // arg: None
    // Desc: Reloads the game scene when the player dies (This will become the death scene when I make it)
    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reloads the currently active scene (the game scene)
    }

    private IEnumerator actMagnet(){
        magActive = true;
        Collider []res = {};
        while(magTime > 0f){
            res = Physics.OverlapSphere(rb.position, 7f, coins);
            for(int i = 0; i < res.Length; i++){
                Vector3 magnetField =res[i].gameObject.transform.position - rb.position;
                float index = (7f-magnetField.magnitude)/7f;
                //Debug.Log($"magnetic field: {magnetField}\t index: {index}\t coin #: {i}\t coin pos: {res[i].gameObject.transform.position}");
                res[i].gameObject.GetComponent<Rigidbody>().MovePosition(magnetField + res[i].gameObject.transform.position * Time.fixedDeltaTime);
                //Debug.Log($"coint #: {i} new coin pos: {res[i].gameObject.transform.position}");
            }
            magTime -= Time.fixedDeltaTime;
            //Debug.Log($"{(int)res.Length} coins in sphere of magnet powerup");
            yield return null;
        }
        magActive = false;
        yield return null;
    }

    private IEnumerator actShield(){
        shieldActive = true;
        while(shieldTime > 0f){
            Debug.Log("Shield still active");
            yield return null;
        }
        shieldActive = false;
        yield return null;
    }

    private IEnumerator actInvinc(){
        invincActive = true;
        while(invincTime > 0f){
            Debug.Log("Invincibility still active");
            yield return null;
        }
        yield return null;
    }
}
