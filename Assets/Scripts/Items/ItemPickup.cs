using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Items item;
    
    private PlayerStats playerStats;

    //Use this for initialization
    void Start()
	{
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Renderer rend = GetComponent<Renderer>();
        playerStats = player.GetComponent<PlayerStats>();
        rend.material.color = item.itemColor[(int)item.itemRarity];
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Picking up " + transform.name);
            item.Initialize(playerStats);
            Destroy(gameObject);
        }
    }
}