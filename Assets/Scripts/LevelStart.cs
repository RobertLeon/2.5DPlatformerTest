//Created by Robert Bryant
//
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStart : MonoBehaviour
{
    private GameManager manager;
    private Vector3 playerSpawnPoint;
    private GameObject player;
    private EnemyStats[] enemyStats;

    //Use this for initialization
    void Start()
    {
        manager = GameManager.Instance;
        playerSpawnPoint = GameObject.FindGameObjectWithTag("PlayerSpawn").transform.position;  
        player = manager.Player.playerPrefab;

        if (player != null)
        {
            Instantiate(player, playerSpawnPoint, Quaternion.identity);          
        }
        else
        {
            Debug.LogError("No player found");
        }        
    }

}
