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
    [SerializeField]
    public List<ItemTable> itemTable;               //Items to assign to the pick up prefab
    public Vector3[] itemSpawnOffsets;              //Where the item spawns in relation to the spawner

    private InputManager inputManager;              //Reference to the Input Manager Script
    private bool multipleItems = false;             //Flag for multiple items
    private float itemDrop;                         //Chance for an item to drop
    private bool itemSpawned = false;               //Check if the item has spawned

    //Use this for initialization
    void Start()
    {
        inputManager = FindObjectOfType<InputManager>();
        
        //Set the drop chance of the item spawner
        itemDrop = Random.value;

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
            if (inputManager.GetKey("Interact") || inputManager.GetButtonDown("Interact"))
            {
                //Create a list of items to spawn
                List<Items> itemToSpawn = new List<Items>();

                //Spawning a single item
                if (!multipleItems && !itemSpawned)
                {
                    //Loop through each item in the item table
                    foreach(ItemTable drop in itemTable)
                    {
                        //Add the item if it has a higher drop chance than the roll
                        if(drop.dropChance > itemDrop)
                        {
                            itemToSpawn.Add(drop.item);
                        }
                    }

                    //Create the item at the specified position
                    ItemPickup pickUp = Instantiate(itemPickup,
                        transform.position + itemSpawnOffsets[0], Quaternion.identity);
                    pickUp.item = itemToSpawn[Random.Range(0, itemToSpawn.Count - 1)];

                    //Stops the item spawner from spawning more items
                    itemSpawned = true;
                }
                //Spawning multiple items
                else if(multipleItems && !itemSpawned)
                {
                    //Loop through for each item to be spawned
                    for (int i = 0; i < numberOfItems; i++)
                    {
                        //Loop through each item in the item table
                        foreach (ItemTable drop in itemTable)
                        {
                            //Add the item if it has a higher drop chance than the roll
                            if (drop.dropChance > itemDrop)
                            {
                                itemToSpawn.Add(drop.item);
                            }

                            //Create the item at the specified position
                            ItemPickup pickUp = Instantiate(itemPickup,
                                transform.position + itemSpawnOffsets[i], Quaternion.identity);
                            pickUp.item = itemToSpawn[Random.Range(0, itemToSpawn.Count - 1)];

                            //Stops the item spawner from spawning more items
                            itemSpawned = true;
                        }
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
            Gizmos.color = new Color(1, 0, 0, .25f);
            Gizmos.DrawCube(transform.position + itemSpawnOffsets[i], transform.localScale);
        }
    }
}
