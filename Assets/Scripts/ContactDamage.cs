//Created by Robert Bryant
//
//Deals damage to entities on contact with the trap component
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactDamage : MonoBehaviour
{
    public float damage;                            //Damage dealt on contact

    private SpikeTrapController trapController;     //Rederence to the Spike Trap Controller

    // Start is called before the first frame update
    private void Start()
    {
        //Get the reference to the Spike Trap Controller
        trapController = GetComponentInParent<SpikeTrapController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Detecting a player
        if(collision.tag == "Player")
        {
            //Reference to the player's stats
            PlayerStats playerStats = collision.GetComponent<PlayerStats>();

            //Check if the trap is active
            if(trapController.trapActive)
            {
                //Check if the player can take damage
                if(playerStats != null && playerStats.canTakeDamage)
                {
                    playerStats.TakeDamage(damage, 0f);
                }
            }
        }

        //Detecting an enemy
        if(collision.tag == "Enemy")
        {
            //Reference to the enemy stats
            EnemyStats enemyStats = collision.GetComponent<EnemyStats>();

            //Check if the trap is active
            if(trapController.trapActive)
            {
                //Enemy takes damage
                if(enemyStats != null)
                {
                    enemyStats.TakeDamage(damage, 0f);
                }
            }
        }
    }


}
