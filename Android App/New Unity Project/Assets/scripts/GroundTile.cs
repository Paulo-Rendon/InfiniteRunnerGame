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

    // Function Name: SpawnObstacle
    // arg: None
    // Desc: Determines where and when to spawn in an obstacle object
    public void SpawnObstacle()
    {
        // choose random point to spawn obstacle
        // including 1st value, excluding the last
        int obstacleSpawnIndex = Random.Range(2, 5);    // An index value within the rails float in GameManager
        Transform spawnPoint = transform.GetChild(obstacleSpawnIndex).transform;    // Use that ObstacleSpawnIndex to find the position where the obstacle should spawn

        // spawn obstacle at position
        Instantiate(obstaclePrefab, spawnPoint.position, Quaternion.identity, transform);
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
