using UnityEngine;

public class SpawningBehaviour : MonoBehaviour
{
    public static GameObject player;
    public GameObject enemy;
    
    public Vector3 playerSpawnLocation = new Vector3(-0.5f, 0.5f, -3.5f);
    public static Vector3 spawn1 = new Vector3(-7.15f, 0.0f, -6.5f);
    public static Vector3 spawn2 = new Vector3(7.0f, 0.0f, 2.0f);
    public static Vector3 spawn3 = new Vector3(1.5f, 0.0f, 7.0f);
    public Vector3[] enemySpawnLocations = new [] {spawn1, spawn2, spawn3};

    void start() {
        playerSpawn();
        enemySpawn();
    }

    public void playerSpawn() {
        Instantiate(player, playerSpawnLocation, Quaternion.identity);
    }

    public void enemySpawn() {
        Instantiate(enemy, enemySpawnLocations[Random.Range(0, 2)], Quaternion.identity);
    }
    public void playerDestroy() {
        Destroy(player);
    }

    public void enemyDestroy() {
        Destroy(enemy);
    }

    void pickUpSpawn(){

    }
}
