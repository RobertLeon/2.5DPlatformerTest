//Created by Robert Bryant
//
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHolder : MonoBehaviour
{
    //The items an entity is holding and the amount of items
    [SerializeField]
    public Dictionary<Items, int> inventory = new Dictionary<Items, int>();

    private PlayerStats playerStats;            //Reference to the Player Stats script
    private EnemyStats enemyStats;              //Reference to the Enemy Stats script

    //Used for initialization
    private void Start()
    {
        //Get the specified components
        playerStats = GetComponent<PlayerStats>();
        enemyStats = GetComponent<EnemyStats>();
    }

    //Add an item to the dictionary
    public void AddItem(Items item)
    {
        //Already holding at least one item 
        if (inventory.ContainsKey(item))
        {
            //Get the amount of items in the inventory
            int value;
            inventory.TryGetValue(item, out value);
            
            //Check the amount of items held
            if (value < item.maxStacks)
            {
                value++;
                inventory[item] = value;
                
            }

            //Set the number of items to the maximum
            if (value >= item.maxStacks)
            {
                inventory[item] = item.maxStacks;
                Debug.Log("Max stacks reached");
            }

            //Activate the item on the player
            if (playerStats != null)
            {
                item.OnPickUp(playerStats);
            }

            //Activate the item on an enemy
            if (enemyStats != null)
            {
                item.OnPickUp(enemyStats);
            }
        }
        //Adding a new item
        else if (!inventory.ContainsKey(item))
        {
            inventory.Add(item, 1);

            //Player is picking up an item
            if (playerStats != null)
            {
                item.OnPickUp(playerStats);
            }

            //Enemy is picking up an item
            if (enemyStats != null)
            {
                item.OnPickUp(enemyStats);
            }
        }        
    }
}
