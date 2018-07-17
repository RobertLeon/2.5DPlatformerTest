//Created by Robert Bryant
//
//Handles the enemy's stats and death
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyStats : Stats
{
    public string[] damageSounds;                   //Damage sounds for the enemey
    public string deathSound;                       //Enemy's death sound
    public int expAmount;                           //Amount of experience given on death
    public Color expColor;                          //Color of the experience text

    [Header("Item Drops")]
    public ItemPickup itemPickUp;                   //Reference to the item being spawned
    [SerializeField]
    public List<ItemTable> itemTable;               //List of items and their drop rates

    private PlayerStats playerStats;                //Reference to the Player Stats script
    private bool isDead = false;                    //Check if the enemy is dead
    private AudioController audioCon;               //Reference to the Audio Controller Script                  


    //Used for initialization
    private void Start()
    {
        //Initialize the enemy's stats
        health.currentHealth = health.maxHealth;

        //Get the player stats
        playerStats = FindObjectOfType<PlayerStats>();
    }


    //Taking Damage
    public override void TakeDamage(float amount, float critChance)
    {
        base.TakeDamage(amount, critChance);
        FindObjectOfType<AudioController>().Play(damageSounds[Random.Range(0,damageSounds.Length)]);
    }

    //Death
    public override void Die()
    {
        //Play the death sound
        FindObjectOfType<AudioController>().Play(deathSound);
        StartCoroutine(RemoveEnemy());
        base.Die();
    }

    //Dropping Items
    private void DropItem()
    {
        //Get a random value for the drop chance and make a new list of items
        float roll = Random.value;
        List<Items> itemToSpawn = new List<Items>();

        //Loop through each item in the item table
        foreach (ItemTable drop in itemTable)
        {
            //Add the item if the it has a higher drop chance than the roll
            if (drop.dropChance > roll)
            {
                itemToSpawn.Add(drop.item);
            }

            //Get a random number from the amount of items in the list
            int item = Random.Range(0, itemToSpawn.Count - 1);

            //If there is an item spawn one where the enemy died
            if (itemToSpawn[item] != null)
            {
                ItemPickup pickUp = Instantiate(itemPickUp, transform.position, Quaternion.identity);
                pickUp.item = itemToSpawn[item];
            }
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

            //Spawn the experience floating text
            TMP_Text expText = Instantiate(floatingText);
            expText.text = expAmount.ToString();
            expText.color = new Color(expColor.r, expColor.g, expColor.b);
            expText.transform.SetParent(damageCanvas.transform);
            expText.transform.position = textSpawn.position;


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