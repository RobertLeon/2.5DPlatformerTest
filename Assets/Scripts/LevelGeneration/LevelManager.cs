//Created by Robert Bryant
//
//Handles the initialization of the game world
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public Character player;                        //Player prefab

    private Camera playerCam;                       //Player Camera
    private GameObject miniMapCam;                  //Mini map camera
    private Vector3 playerSpawnPos;                 //Position of the player spawn
    private GameObject[] treasureSpawns;            //Treasure spawn object
    private GameObject[] enemySpawns;               //Enemy spawn object


    public IEnumerator InitializeSpawning()
    {
        yield return new WaitForSeconds(0.1f);

        Debug.Log("Spawning");
        //Find and assign objects in the scene        
        playerCam = Camera.main;
        miniMapCam = GameObject.FindGameObjectWithTag("MiniMapCamera");

        //Get the spawn points for the player and treasure rooms
        playerSpawnPos = GameObject.FindGameObjectWithTag("PlayerSpawn").transform.position;
        treasureSpawns = GameObject.FindGameObjectsWithTag("TreasureSpawn");
        enemySpawns = GameObject.FindGameObjectsWithTag("EnemySpawn");

        //Create the player in the scene
        Instantiate(player.playerPrefab, playerSpawnPos, Quaternion.identity);

        //Eneable each treasure to spawn in
        for (int i = 0; i < treasureSpawns.Length; i++)
        {
            //  treasureSpawns[i].GetComponent<TreasureSpawner>().enabled = true;
        }

        for (int i = 0; i < enemySpawns.Length; i++)
        {
            //Spawn enemies
            //enemySpawns[i].GetComponent<EnemeySpawner>().enabled = true;
        }

        //Enable the player camera and mini map camera scripts
        playerCam.GetComponent<CameraController>().enabled = true;
        miniMapCam.GetComponent<MiniMapCameraController>().enabled = true;
    }

    
}
