using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Items item;                  //Reference to the item
    
    private PlayerStats playerStats;    //Reference to the PlayerStats

    //Use this for initialization
    void Start()
	{
        //Get the player's stats
        GameObject player = GameObject.FindGameObjectWithTag("Player");        
        playerStats = player.GetComponent<PlayerStats>();

        //Get the renderer on the item to change it's color
        Renderer rend = GetComponent<Renderer>();
        rend.material.color = item.itemColor[(int)item.itemRarity];
	}

    //
    private void OnTriggerEnter(Collider other)
    {
        //If the player touches the item
        if (other.tag == "Player")
        {
            Debug.Log("Picking up " + transform.name);
            //Initialize the item
            item.Initialize(playerStats);

            //Destroy the gameObject
            Destroy(gameObject);
        }
    }
}