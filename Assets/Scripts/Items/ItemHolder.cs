//Created by Robert Bryant
//
//Handles an entity's inventory
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemHolder : MonoBehaviour
{
    public Image itemIcon;                          //Item UI Prefab
    public Inventory[] inventory;                   //The items an entity is holding and the amount of items

    private PlayerStats playerStats;                //Reference to the Player Stats script
    private EnemyStats enemyStats;                  //Reference to the Enemy Stats script
    private Transform inventoryUI;                  //Reference to the Inventory UI in the scene
    private Dictionary<Items, int> itemInventory;   //Dictonary of items and how many are held
    private Dictionary<Image, KeyValuePair<Items,int>> imageDictionary; //

    [System.Serializable]
    public struct Inventory
    {
        public Items newItem;
        public int quantity;        
    }

    //Used for initialization
    private void Start()
    {
        //Get the specified components
        playerStats = GetComponent<PlayerStats>();
        enemyStats = GetComponent<EnemyStats>();
        inventoryUI = GameObject.Find("ItemInventory").transform;
        itemInventory = new Dictionary<Items, int>();
        imageDictionary = new Dictionary<Image, KeyValuePair<Items, int>>();

        //Adds items from the inspector to the entity's inventory
        AddItemsToDictionary();
    }

    //Adds items from the inspector to the item dictionary
    private void AddItemsToDictionary()
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if (!itemInventory.ContainsKey(inventory[i].newItem))
            {
                for(int j = 0; j < inventory[i].quantity; j++)
                {
                    AddItem(inventory[i].newItem);
                }
            }
        }
    }

    //Add an item to the inventory
    public void AddItem(Items item)
    {
        //Check if the item is not in the dictionary
        if (!itemInventory.ContainsKey(item))
        {
            //Add the item to the dictionary
            itemInventory.Add(item, 1);

            //Check for player
            if (playerStats != null)
            {
                //Assign the player the item's stats
                item.OnPickUp(playerStats);

                //Create a new icon in the UI
                Image newImage = Instantiate(itemIcon, inventoryUI);

                //Update the UI for that item
                UpdateUI(newImage, new KeyValuePair<Items, int>(item, 1));
            }

            //Check for enemy
            if(enemyStats != null)
            {
                //Assign the stats gained to the enemy
                item.OnPickUp(enemyStats);
            }
        }
        //Check if the item allows multiple copies to be equipped
        else if (item.isStackable)
        {
            //Get the current amount of items held
            int value = itemInventory[item];

            //Check if the amount is less than the maximum allowed
            if (value < item.maxStacks)
            {
                //Set the new amount of the item
                itemInventory[item] = value + 1;

                //Check for player
                if (playerStats != null)
                {
                    //Add the stats to the player
                    item.OnPickUp(playerStats);

                    //Get the image from the dictionary
                    Image oldImage = GetImageFromDictionary(item, value - 1);

                    //Update the player's UI
                    UpdateUI(oldImage, new KeyValuePair<Items, int>(item, value));
                }

                //Check for the enemy
                if (enemyStats != null)
                {
                    item.OnPickUp(enemyStats);
                }
            }

            //Check if the value is greater than  or equal to the maximum
            if (value >= item.maxStacks)
            {
               //This should do something for the player at least.
               //Not sure what.
            }
        }
        //The item is in the dictionary but not stackable
        else
        {
            //This should be the same for picking up more than the maximum allowed
        }
    }

    //Removes an item from the inventoty
    public void RemoveItem(Items item)
    {
        //Check if the item is in the inventory
        if(!itemInventory.ContainsKey(item))
        {
            //Error
            Debug.LogError("Item: " + item.name + " was not found");
        }
        else
        {
            int value = itemInventory[item];

            //There is only one item
            if(value == 1)
            {
                //Remove it from the dictionary
                itemInventory.Remove(item);

                Image removeImage = GetImageFromDictionary(item, value);

                UpdateUI(removeImage, new KeyValuePair<Items, int>(item, value), true);
            }
            else
            {
                //Decrease the value by one
                itemInventory[item] = value - 1;

                Image oldImage = GetImageFromDictionary(item, value - 1);

                UpdateUI(oldImage, new KeyValuePair<Items, int>(item, value));
            }
        }
    }

    //Gets the image from dictionary specified by the item and value
    private Image GetImageFromDictionary(Items item, int value)
    {
        //Reverse the image dictionary to find the image associated with the item
        KeyValuePair<Items, int> kvp = new KeyValuePair<Items, int>(item, value);
        Dictionary<KeyValuePair<Items, int>, Image> reverseDictionary =
            imageDictionary.ToDictionary(key => key.Value, pair => pair.Key);

        return reverseDictionary[kvp];
    }

    //Update the player's UI
    private void UpdateUI(Image newImage, KeyValuePair<Items, int> item, bool removeImage = false)
    {
        TMP_Text itemCountLabel = newImage.GetComponentInChildren<TMP_Text>();
        newImage.GetComponent<ShowItemInfo>().itemInfo = item.Key.itemName +
            "\n" + item.Key.itemDescription;

        //Check if an image needs to be removed
        if (!removeImage)
        {
            //UI Image is not in dictionary and the item is stackable
            if (!imageDictionary.ContainsKey(newImage) && item.Key.isStackable)
            {
                //Add the image and item and set the label to blank
                imageDictionary.Add(newImage, item);
                itemCountLabel.text = "";
            }
            //UI Image is not in dictionary and the item is not stackable
            else if (!imageDictionary.ContainsKey(newImage) && !item.Key.isStackable)
            {
                imageDictionary.Add(newImage, item);
                itemCountLabel.text = "MAX";
            }
            //UI Image is in the dictionary
            else if (imageDictionary.ContainsKey(newImage))
            {
                //Set the label to the amount
                if (item.Value < item.Key.maxStacks)
                {
                    itemCountLabel.text = "x" + item.Value.ToString();
                }
                //Set the label to max
                else
                {
                    itemCountLabel.text = "MAX";
                }
            }
        }
        else
        {
            //Check if the image is in the dictionary
            if(!imageDictionary.ContainsKey(newImage))
            {
                //Error
                Debug.LogError("Item: " + item.Key.itemName + " was not found!");
            }
            else
            {
                //Remove the image from the dictionary and destroy the game object
                imageDictionary.Remove(newImage);
                Destroy(newImage.gameObject);
            }
        }
    }
}
