using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Controls the core game logic
public class GameManager : MonoBehaviour
{
    // Speed of the player object
    public int startSpeed = 20;
    // Stores the players score for this playthrough
    int score;

// This value is used to allow a non-linear increase to the score
    int buffer = 0;

// This value tracks the amount of coins the user has picked up in the specific playthrough
    int coins = 0;
    // Instance of the GameManager object
    public static GameManager inst;
    // Float array that stores the three x values the player object is allowed to move to
    public static float [] rails = new float[3] {-3, 0, 3};
    // The text representation of the score for this specific playthorugh
    [SerializeField] Text scoreText;
    // The text representation of the coins collected for this specific playthorugh
     [SerializeField] Text coinText;
    // Object that reflects the player's current movement (Called to increase players forward movement)
    [SerializeField] PlayerMovement playerMovement;
    public int magnetLevel = 0;

    public int MagnetLevel{
        get{return magnetLevel;}
    }
    public int shieldLevel = 0;
    public int ShieldLevel{
        get{return shieldLevel;}
    }
    public int invincibleLevel = 0;
    public int InvincibleLevel{
        get{return invincibleLevel;}
    }

    // Function Name: IncrementScore
    // Arg: int amount: The amount the score should increment (defaults to 1 but is overwritten when a coin is picked up)
    // Desc: Increments the score object, increases the player's speed dependent on the score, and update the scoreText object
    public void IncrementScore(int amount = 1)
    {
        // Buffer is used to increase the score at a slower rate
        buffer += amount;
        score = buffer/30;

        // Update the scoreText object to reflect thte updated score
        scoreText.text = $"Score: {score}";

        // Player y axis speed is the startSpeed + 1/2((startSpeed)(score/1000))
        playerMovement.speed = startSpeed + 0.5f * (startSpeed * (score/1000));
    }

    // Function Name: IncrementCoin
    // Arg: None
    // Desc: Increments coin counter, update the coin text to reflect this incrementation,
    // and increase the score
    public void IncrementCoin()
    {
        coins++;    // Increment the coins counter
        coinText.text = $"coins: {coins}";  // Update the coin text object

        IncrementScore(300);    // Call IncrementScore with a amount argument of 300
    }

    public void actMagnet()
    {
        Debug.Log("Picked up a magnet powerup!");
        playerMovement.MagTime((magnetLevel/2)*5f);
    }

    public void actInvincible()
    {
        Debug.Log("Picked up a invincibility powerup!");
    }

    public void actShield()
    {
        Debug.Log("Picked up a shield powerup!");
    }

    private void Awake()
    {
        inst = this;    // Loop the GameManager inst to store the same values as "this" object 
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        IncrementScore();   // Constantly increment the score while the player is still alive (by a value of 1)
    }
}
