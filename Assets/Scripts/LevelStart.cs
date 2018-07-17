//Created by Robert Bryant
//
//Activates all the necessary objects in a given scene.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStart : MonoBehaviour
{
    private GameManager manager;            //Reference to the Game Manager script
    private Vector3 playerSpawnPoint;       //Spawn points for the player
    private GameObject player;              //Reference to the player's game object
    private EnemyStats[] enemyStats;        //Reference to the enemies in the scene

    //Use this for initialization
    void Start()
    {
        //Get the spawn points and the player's game object
        manager = GameManager.Instance;
        playerSpawnPoint = GameObject.FindGameObjectWithTag("PlayerSpawn").transform.position;  
        player = manager.Player.playerPrefab;

        //If the player exists spawn the player in a spawn point
        if (player != null)
        {
            //If there is a valid spawn point spawn the player at it
            if (playerSpawnPoint != null)
            {
                Instantiate(player, playerSpawnPoint, Quaternion.identity);
            }
            //Spawn the player at (0,0,0) and return an error.
            else
            {
                Debug.LogError("No spawn points found");
                Instantiate(player, Vector3.zero, Quaternion.identity);
            }
        }
        else
        {
            Debug.LogError("No player found");
        }        
    }

}
