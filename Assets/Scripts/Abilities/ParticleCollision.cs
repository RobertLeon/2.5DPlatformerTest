//Created by Robert Bryant
//
//Detects collision from particles being used in abilities.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCollision : MonoBehaviour
{
    public LayerMask playerLayerMask;                   //Layer mask for player particle collision
    public LayerMask enemyLayerMask;                    //Layer mask for enemy/trap particle collision

    private ParticleShoot particleShoot;                //Reference to Particle Shoot
    private EnemyStats enemyStats;                      //Reference to Enemy Stats
    private PlayerStats playerStats;                    //Reference to Player Stats
    private ParticleSystem particleSys;                 //Reference to the Particle System
    private ParticleSystem.CollisionModule particleCol; //Reference to the particle system collision module
    private float damage;                               //Damage the ability deals
    private float critChance = 0;                             //Critical hit chance of the user
    private bool enemyParticles = false;                //Is the ability being used by an enemy?
    private bool playerParticles = false;               //Is the ability being used by the player?

    //Use this for initialization
    private void Start()
    {
        //Get the components for the script
        particleShoot = GetComponentInParent<ParticleShoot>();
        enemyStats = GetComponentInParent<EnemyStats>();
        playerStats = GetComponentInParent<PlayerStats>();
        particleSys = GetComponent<ParticleSystem>();
        particleCol = particleSys.collision;

        //If enemyStats exists then the user is an enemy
        if(enemyStats != null)
        {
            playerStats = null;
            playerParticles = false;
            enemyParticles = true;            
            particleCol.collidesWith = enemyLayerMask;
            damage = enemyStats.combat.attack + particleShoot.abilityDamage;
            critChance = enemyStats.combat.critChance;
        }

        //If playerStats exists then the use is a player
        if(playerStats != null)
        {
            enemyStats = null;
            playerParticles = true;
            enemyParticles = false;
            particleCol.collidesWith = playerLayerMask;
            damage = playerStats.combat.attack + particleShoot.abilityDamage;
            critChance = playerStats.combat.critChance;
        }

        //If neither exist then then the user is a trap
        if(playerStats == null && enemyStats == null)
        {
            particleCol.collidesWith = enemyLayerMask;
            damage = particleShoot.abilityDamage;
            critChance = 0;
        }

        transform.parent = null;
    }

    private void Update()
    {
        //Destroy the particles after they stop playing
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
        //If colliding with the player
        if (other.tag == "Player")
        {
            //Get the player's stats
            playerStats = other.GetComponent<PlayerStats>();     
            
            //If an enemy uses the particle ability
            if (enemyParticles)
            {  
                //Calculate the ability's damage
                damage = enemyStats.combat.attack + particleShoot.abilityDamage;
                critChance = enemyStats.combat.critChance;

                //Deal damage to the player and start the invincibility period
                playerStats.TakeDamage(damage, critChance);
                playerStats.canTakeDamage = false;
            }

            if(!playerParticles && !enemyParticles)
            {
                //Calculate the damage of the ability
                damage = particleShoot.abilityDamage;

                //Deal damage to the player
                playerStats.TakeDamage(damage, critChance);
                playerStats.canTakeDamage = false;
            }
        }

        //If colliding with an enemy and a player is using the particle ability
        if(other.tag == "Enemy" && playerParticles)
        {
            //Get the enemy's stats and calculate the ability's damage
            enemyStats = other.GetComponent<EnemyStats>();
            damage = playerStats.combat.attack + particleShoot.abilityDamage;
            critChance = playerStats.combat.critChance;

            //Deal damage to the enemy
            enemyStats.TakeDamage(damage, critChance);
        }

        
       
    }
}
