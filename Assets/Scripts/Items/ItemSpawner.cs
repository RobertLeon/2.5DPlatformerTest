//Created by Robert Bryant
//
//Handles the spawning of items
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public Items[] itemList;                        //List of items to be spawned
    public ItemPickup item;                         //Item prefab
    public int numberOfItems;                       //Number of items to be spawned

    private PlayerInput playerInput;                //Player input
    private ItemPickup[] pickups;                   //Spawned items
    private bool multipleItems = false;             //Flag for multiple items

    //Use this for initialization
    void Start()
    {
        //Reference to the player's input
        playerInput = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>();

        pickups = new ItemPickup[numberOfItems];

        //Check the amount of items to spawn
        if (numberOfItems >= 2)
        {
            multipleItems = true;
        }
        //Destroy the spawner if it has 0 or negative amount of items
        else if (numberOfItems <= 0)
        {
            Debug.LogError(transform.name + " has been spawned with " + numberOfItems + "." +
                " Please ensure that each item spawner has a non zero and positive amount of items to spawn.");
            Destroy(gameObject);
        }

    }

    //Checks for collision
    private void OnTriggerStay(Collider other)
    {
        //If the player is on the item spawner
        if (other.tag == "Player")
        {
            //Check for the player's input
            if (Input.GetKeyDown(playerInput.kbInteract) || Input.GetKeyDown(playerInput.ctInteract))
            {
                //Spawn a sigle item
                if (!multipleItems)
                {
                    pickups[0] = Instantiate(item, transform.position, Quaternion.identity);
                    pickups[0].item = itemList[Random.Range(0, itemList.Length)];
                }
                else
                {
                    //Spawn each item
                    for(int i = 0; i < pickups.Length; i++)
                    {
                        pickups[i] = Instantiate(item,
                            transform.position + new Vector3(i* 1.5f,0,0), Quaternion.identity);
                        pickups[i].item = itemList[Random.Range(0, itemList.Length)];
                    }
                }
            }
        }
    }
}
