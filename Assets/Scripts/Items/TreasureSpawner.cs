//Created by Robert Bryant
//
//Handles the spawning of treasure in the game
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureSpawner : MonoBehaviour
{
    
    public GameObject[] treasureLocations;       //The locations for treasure to spawn
    public GameObject[] treasureHolder;         //Group of objects that hold treasure

    //Finds and spawns the 
    public void SpawnTreasure()
    {
        //Find all the treasure location in the scene
        treasureLocations = GameObject.FindGameObjectsWithTag("Treasure");

        //Loop through each location and spawn treasure holder there
        for(int i  = 0; i < treasureLocations.Length; i++)
        {
            //Spawns a random treasure holder
            Instantiate(treasureHolder[Random.Range(0, treasureHolder.Length)],
                treasureLocations[i].transform.position, Quaternion.identity);
        }

    }

}
