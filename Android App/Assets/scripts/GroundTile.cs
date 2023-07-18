using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class GroundTile : MonoBehaviour
{
// GroundSpawner object
    GroundSpawner groundSpawner;

    // Stores the coin prefab
    [SerializeField] GameObject coinPrefab;
        // Stores the obstacle prefab
    [SerializeField] GameObject obstaclePrefab;

    [SerializeField] GameObject cactiPrefab;
    [SerializeField] GameObject magnetPrefab;
    [SerializeField] GameObject shieldPrefab;
    [SerializeField] GameObject invinciblePrefab;
    private int obstacleSpawnIndex;

    // Start is called before the first frame update
    void Start()
    {
        // groundSpawner object is the GroundSpawner object in GameObject
        groundSpawner = GameObject.FindObjectOfType<GroundSpawner>();
    }

    // Function Name: OnTriggerExit
    // arg: Collider other: the object that ran into the groundTile
    // Desc: Spawns the next ground tile then destroys it after
    private void OnTriggerExit(Collider other)
    {
        groundSpawner.SpawnTile(true);
        Destroy(gameObject, 2);
    }

    private Transform GetPowerupLocation(int obstacleSpawnIndex)
    {
        int powerupSpawnIndex = Random.Range(2, 5);
        if(powerupSpawnIndex == obstacleSpawnIndex)
            powerupSpawnIndex = (powerupSpawnIndex + 1)%3 + 2;
        return transform.GetChild(powerupSpawnIndex).transform;
    }
    // Function Name: SpawnObstacle
    // arg: None
    // Desc: Determines where and when to spawn in an obstacle object
    public void SpawnObstacle(Powerup powerup = Powerup.none)
    {
        // choose random point to spawn obstacle
        // including 1st value, excluding the last
        obstacleSpawnIndex = Random.Range(2, 5);    // An index value within the rails float in GameManager

        // spawnPowerUps && !Random.Range(0, 20)
        // I think that will work, but if not Random.Range(0, 20) == 0 should have the effect I want
        
        Transform spawnPoint = transform.GetChild(obstacleSpawnIndex).transform;    // Use that ObstacleSpawnIndex to find the position where the obstacle should spawn

        // spawn obstacle at position
        Instantiate(obstaclePrefab, spawnPoint.position, Quaternion.identity, transform);

        // Handles the cacti spawning (should move to its own function, maybe within a sand class)
        for(int i = 1; i <= 4; i++)
        {
            // 10% chance of a cactus spawning
            if(Random.Range(1, 11) == 1)
            {
                spawnPoint = transform.GetChild(4+i).transform;
                Instantiate(cactiPrefab, spawnPoint.position, Quaternion.identity, transform);
            }
        }
    }

    public void SpawnPowerup(Powerup powerup = Powerup.none)
    {
        Transform powerSpawnPoint;
        switch(powerup){
            case Powerup.none:
                break;
            case Powerup.magnet:
                powerSpawnPoint = GetPowerupLocation(obstacleSpawnIndex);
                Instantiate(magnetPrefab, powerSpawnPoint.position, Quaternion.identity, transform);
                break;
            case Powerup.invincible:
                powerSpawnPoint = GetPowerupLocation(obstacleSpawnIndex);
                Instantiate(invinciblePrefab, powerSpawnPoint.position, Quaternion.identity, transform);
                break;

            case Powerup.shield:
                powerSpawnPoint = GetPowerupLocation(obstacleSpawnIndex);
                Instantiate(shieldPrefab, powerSpawnPoint.position, Quaternion.identity, transform);
                break;
        }
    }

    // Function Name: SpawnCoins
    // arg: None
    // Desc: Determines where and when to spawn in an coin object
    public void SpawnCoins()
    {
        int coinsToSpawn = 1; // Amount of coins to spawn per spawnTile object
        for (int i = 0; i < coinsToSpawn; i++) // for loop to spawn in coins
        {
            GameObject temp = Instantiate(coinPrefab, transform); // temp GameObject is the coin to be spawned
            temp.transform.position = GetRandomPointInCollider(GetComponent<Collider>());   // Get a random point within the available groundTile
                                                                                            // must still be within the rails x value constrant
                                                                                            // The float in GameManager
        }
    }

    // Function Name: GetRandomPointInCollider
    // Arg: Collider collider: Used to constrain the position where the coin can spawn to be reachable by the player
    // return: Collider collider: The coords within the bounds of the collider
    // Desc: Uses the collider to find random coordinates that are reachable by the player (on existing groundTile and on rails reachable by the player)
    Vector3 GetRandomPointInCollider(Collider collider)
    {
        // Going to store the coordinates that will be returned
        Vector3 point = new Vector3(
            GameManager.rails[Random.Range(0, 3)],  // Get a random x value on the rails array
            Random.Range(collider.bounds.min.y, collider.bounds.max.y), // Get a random y value thats on the current groundTile
            Random.Range(collider.bounds.min.z, collider.bounds.max.z));    // Get a z value that alligns with the groundTile
        if (point != collider.ClosestPoint(point))  // If the calculated point is not a valid point on the collider
        {
            // Get a new randompoint (recursive call)
            point = GetRandomPointInCollider(collider);
        }

        // set the point y value to 1
        point.y = 1;
        return point; // return point
    }
}
