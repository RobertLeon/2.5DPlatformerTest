//Created by Robert Bryant
//
//Handles the initialization of the game world
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject playerPrefab;                 //Player prefab

    private LevelGeneration levelGen;               //Level generation
    private Camera playerCam;                       //Player Camera
    private GameObject miniMapCam;                  //Mini map camera
    private Vector3 playerSpawnPos;                 //Position of the player spawn
    private GameObject[] treasureSpawns;            //Treasure spawn object
    private GameObject[] enemySpawns;               //Enemy spawn object
    private bool initiaized = false;                //Check for game initialization
    

	//Use this for initialization
	void Start()
	{
        //Find and assign objects in the scene
        levelGen = GameObject.FindGameObjectWithTag("LevelGeneration").GetComponent<LevelGeneration>();
        playerCam = Camera.main;
        miniMapCam = GameObject.FindGameObjectWithTag("MiniMapCamera");

	}

    private void Initialize()
    {
        //Get the spawn points for the player and treasure rooms
        playerSpawnPos = GameObject.FindGameObjectWithTag("PlayerSpawn").transform.position;
        treasureSpawns = GameObject.FindGameObjectsWithTag("TreasureSpawn");

        //Create the player in the scene
        Instantiate(playerPrefab, playerSpawnPos, Quaternion.identity);

        //Eneable each treasure to spawn in
        for (int i = 0; i < treasureSpawns.Length; i++)
        {
            treasureSpawns[i].GetComponent<TreasureSpawner>().enabled = true;
        }
       
        //Enable the player camera and mini map camera scripts
        playerCam.GetComponent<CameraController>().enabled = true;
        miniMapCam.GetComponent<MiniMapCameraController>().enabled = true;
        initiaized = true;
    }

    //Update is called once per frame
    void Update()
	{
        //Check if level generation is complete
        if (levelGen.complete && !initiaized)
        {
            Initialize();
        }
        //Error for faild initalization
        else if (!initiaized)
        {
            Debug.LogError("Game initialization failed.");
            initiaized = true;
        }
	}
}
