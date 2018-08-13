//Created by Robert Bryant
//
//Allows the player to pick up items that have been spawned
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Items item;                  //Reference to the item
    public float waitTime;              //Time to wait after spawning the item

    //Use this for initialization
    void Start()
	{

        if(GetComponent<BoxCollider>().enabled)
        {
            GetComponent<BoxCollider>().enabled = false;
        }

        //Get the renderer on the item to change it's color
        //Renderer rend = GetComponent<Renderer>();
        //rend.material.color = item.itemColor[(int)item.itemRarity];

        //Allows the item to spawn on screen
        StartCoroutine(PickUp());
	}

    //Enables the item to be picked up by the player
    private IEnumerator PickUp()
    {
        //Wait for a second
        yield return new WaitForSeconds(waitTime);

        //Enable the box collider component
        transform.GetComponent<BoxCollider>().enabled = true;
    }

    //Check for collision with this object
    private void OnTriggerEnter(Collider other)
    {
        //If the player touches the item
        if (other.tag == "Player")
        {
            //Initialize the item
            other.GetComponent<ItemHolder>().AddItem(item);
            FindObjectOfType<AudioController>().PlaySongs("ItemPickUp");

            //Destroy the gameObject
            Destroy(gameObject);
        }
    }
}