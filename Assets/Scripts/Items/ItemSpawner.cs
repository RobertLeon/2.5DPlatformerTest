//Created by Robert Bryant
//
//Handles the spawning of items
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public ItemPickup itemPickup;                   //Item pickup prefab
    public int numberOfItems;                       //Number of items to be spawned

    public ItemTable table;                         //Items to assign to the pick up prefab
    public Vector3[] itemSpawnOffsets;              //Where the item spawns in relation to the spawner

    private InputManager inputManager;              //Reference to the Input Manager Script
    private bool multipleItems = false;             //Flag for multiple items
    private bool itemSpawned = false;               //Check if the item has spawned

    //Use this for initialization
    void Start()
    {
        inputManager = FindObjectOfType<InputManager>();

        if(numberOfItems >= 2)
        {
            multipleItems = true;
        }

        if(itemSpawned)
        {
            transform.localScale = new Vector3(2, 2, 1);
        }
    }

    //Checks for collision
    private void OnTriggerStay(Collider other)
    {
        //If the player is on the item spawner
        if (other.tag == "Player")
        {
           
            //Check for the player's input
            if (inputManager.GetKeyDown("Interact") || inputManager.GetButtonDown("Interact"))
            {
                ItemPickup newItem = null;
                //Spawning a single item
                if (!multipleItems && !itemSpawned)
                {
                    newItem = Instantiate(itemPickup, transform.position + itemSpawnOffsets[0], Quaternion.identity);
                    newItem.item = table.DropItem();
                    itemSpawned = true;
                }
                //Spawning multiple items
                else if(multipleItems && !itemSpawned)
                {
                    for (int i = 0; i < numberOfItems; i++)
                    {
                        itemPickup = Instantiate(itemPickup, transform.position + itemSpawnOffsets[i], Quaternion.identity);
                        itemPickup.item = table.DropItem();
                        itemSpawned = true;
                    }
                }
            }
        }
    }

    //Shows the item spawn offsets in the scene view
    private void OnDrawGizmos()
    {
        for (int i = 0; i < itemSpawnOffsets.Length; i++)
        {
            Gizmos.color = new Color(1, 0, 0, .75f);
            Gizmos.DrawCube(transform.position + itemSpawnOffsets[i], transform.localScale);
        }
    }
}
