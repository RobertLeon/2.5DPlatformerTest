//Created by Robert Bryant
//
//Handles the enemy's stats and death
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyStats : Stats
{
    public int expAmount;                           //Amount of experience given on death
    public Color expColor;                          //Color of the experience text        

    [Header("Item Drops")]
    public ItemPickup itemPickUp;                   //Reference to the item being spawned
    public ItemTable itemTable;                     //Item table for the enemy

    private PlayerStats playerStats;                //Reference to the Player Stats script
    private bool isDead = false;                    //Check if the enemy is dead                  


    //Used for initialization
    public override void Start()
    {
        //Initialize the enemy's stats
        health.currentHealth = health.maxHealth;

        //Get the player stats
        playerStats = FindObjectOfType<PlayerStats>();

        base.Start();
    }


    //Taking Damage
    public override void TakeDamage(float amount, float critChance)
    {
        base.TakeDamage(amount, critChance);        
    }

    //Death
    public override void Die()
    {
        //Play the death sound
        StartCoroutine(RemoveEnemy());
        base.Die();
    }

    //Tries to spawn an item after an enemy dies
    private void DropItem()
    {
        //Get an item from the item table
        Items newItem = itemTable.DropItem();

        //If an item is picked spawn the pick up prefab
        if (newItem != null)
        {
            ItemPickup pickUp = Instantiate(itemPickUp, transform.position, Quaternion.identity);
            pickUp.item = newItem;
        }
    }

    //Removes the enemy from the scene
    private IEnumerator RemoveEnemy()
    {
        //Check if the enemy is alive
        if (!isDead)
        {
            //Set the enemy's death
            isDead = true;
            //Change the tage to disallow the enemy from absorbing projectiles
            transform.tag = "Untagged";

            //Wait the specified amount of time
            yield return new WaitForSeconds(0.5f);


            if (damageCanvas != null)
            {
                //Spawn the experience floating text
                TMP_Text expText = Instantiate(floatingText);
                expText.transform.position = textSpawn.position;
                expText.text = expAmount.ToString();
                expText.color = new Color(expColor.r, expColor.g, expColor.b);
                expText.transform.SetParent(damageCanvas.transform);
            }
            else
            {
                Debug.LogError("Damage Canvas not found");
            }

            //Check for the Player
            if (playerStats == null)
            {
                //Check for the player again and add the experience
                playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
                playerStats.GainExperience(expAmount);
            }
            else
            {
                //Player gains experience
                playerStats.GainExperience(expAmount);
            }

            //Attempt to drop an item
            DropItem();

            //Destroy the enemy game object
            Destroy(gameObject);
        }
    }
}