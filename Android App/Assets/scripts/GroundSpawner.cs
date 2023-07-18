using System.Collections.Specialized;
using UnityEngine;

public class GroundSpawner : MonoBehaviour
{

    // The groundTile preFab
    [SerializeField] GameObject groundTile;

    // Stores the origin of the next groundTile to spawn
    Vector3 nextSpawnPoint;

    // This bool is used to stop spawning obstacles (for debugging purposes)
    [SerializeField] bool spawnObstacles = true;
    [SerializeField] bool spawnPowerups = true;
    [SerializeField] int powerupOdds = 100;

    // Function Name: SpawnTile
    // arg: bool spawnItems - True means spawn items, false means only spawn ground tiles
    // Desc: spawns the next ground tile, the coins, and objects if respected bools are set
    public void SpawnTile (bool spawnItems) 
    {
        // creates GameObject that is going to be the next spawned tile
        GameObject temp = Instantiate(groundTile, nextSpawnPoint, Quaternion.identity); 
        // stores the cordinates for the origin of the next spawned groundTile prefab
        nextSpawnPoint = temp.transform.GetChild(1).transform.position;
        Powerup powerupToSpawn = Powerup.none;
        if(!spawnItems)
            return;
        if(spawnPowerups && Random.Range(0, 100) < powerupOdds){
            int tmp = Random.Range(0, 9);
            switch(tmp/3){
                case 0:
                    if(tmp%3 < (GameManager.inst.MagnetLevel+1)/2)
                        powerupToSpawn = Powerup.magnet;
                    break;
                case 1:
                    if(tmp%3 < (GameManager.inst.InvincibleLevel+1)/2)
                        powerupToSpawn = Powerup.invincible;
                    break;
                case 2:
                    if(tmp%3 < (GameManager.inst.ShieldLevel+1)/2)
                        powerupToSpawn = Powerup.shield;
                    break;
            }
        }

        // If spawnItems bool is true and spawnObstacles bool is true
        if(spawnObstacles)
            temp.GetComponent<GroundTile>().SpawnObstacle(powerupToSpawn);    // call the SpawnObstacle function

        if(powerupToSpawn != Powerup.none)
            temp.GetComponent<GroundTile>().SpawnPowerup(powerupToSpawn);

        // If spawnItems bool is true call the SpawnCoins function
        temp.GetComponent<GroundTile>().SpawnCoins();

    }
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 15; i++)
        {
            if (i < 3) SpawnTile(false);
            else SpawnTile(true);
        }
    }

}

public enum Powerup{
    none, magnet, invincible, shield
}
