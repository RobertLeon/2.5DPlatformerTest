//Created by Robert Bryant
//
//Detects collision from particles being used in abilities.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCollision : MonoBehaviour
{
    private ParticleShoot particleShoot;                //Reference to Particle Shoot
    private EnemyStats enemyStats;                      //Reference to Enemy Stats
    private PlayerStats playerStats;                    //Reference to Player Stats
    private ParticleSystem particleSys;                 //
    private float damage;                               //Damage the ability deals
    private float crit;                                 //Critical hit chance of the user
    private bool enemyParticles;                        //Is the ability being used by an enemy?
    private bool playerParticles;                       //Is the ability being used by the player?

    //Use this for initialization
    private void Start()
    {
        //Get the components for the 
        particleShoot = GetComponentInParent<ParticleShoot>();
        enemyStats = GetComponentInParent<EnemyStats>();
        playerStats = GetComponentInParent<PlayerStats>();
        particleSys = GetComponent<ParticleSystem>();

        //If enemyStats exists then this is an enemy ability
        if(enemyStats != null)
        {
            playerStats = null;
            playerParticles = false;
            enemyParticles = true;
        }

        //If playerStats exists then this is a player ability
        if(playerStats != null)
        {
            enemyStats = null;
            playerParticles = true;
            enemyParticles = false;
        }
        transform.parent = null;
    }

    private void Update()
    {
        if(particleSys)
        {
            if(!particleSys.IsAlive())
            {
               
                Destroy(gameObject);
            }
        }
    }

    //When the particles collide with another object
    private void OnParticleCollision(GameObject other)
    {
        //If colliding with the player and an enemy is using the particle ability
        if(other.tag == "Player" && enemyParticles)
        {
            //Get the player's stats and calculate the ability's damage
            playerStats = other.GetComponent<PlayerStats>();
            damage = enemyStats.combat.attack + particleShoot.abilityDamage;
            crit = enemyStats.combat.critChance;

            //Deal damage to the player and start the invincibility period
            playerStats.TakeDamage(damage, crit);
            playerStats.canTakeDamage = false;
        }

        //If colliding with an enemy and a player is using the particle ability
        if(other.tag == "Enemy" && playerParticles)
        {
            //Get the enemy's stats and calculate the ability's damage
            enemyStats = other.GetComponent<EnemyStats>();
            damage = playerStats.combat.attack + particleShoot.abilityDamage;
            crit = playerStats.combat.critChance;

            //Deal damage to the enemy
            enemyStats.TakeDamage(damage, crit);
        }
    }
}
