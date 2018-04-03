//Created by Robert Bryant
//
//Handles the spawning of treasure in the game
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureSpawner : MonoBehaviour
{
    public GameObject[] treasureHolder;  

	//Use this for initialization
	void Start()
	{
        Instantiate(treasureHolder[Random.Range(0,treasureHolder.Length)],
            transform.position,Quaternion.identity);        
	}
}
