//Created by Robert Bryant
//
//Handles the behaviour of most projectiles
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    [HideInInspector]
    public int direction;                   //Direction the object is moving in
    [HideInInspector]
    public float damage;                    //Amount of damage this object does
    [HideInInspector]
    public Vector2 startPos;                //Starting position
    [HideInInspector]
    public float critChance;                //Chance to double damage
    [HideInInspector]
    public float maxDistance;               //Maximum distance object can travel
    [HideInInspector]
    public float distance;                  //Current distance the object has traveled
    [HideInInspector]
    public bool playerProjectile = false;           //Type of projectile being shot
    [HideInInspector]
    public bool enemyProjectile = false;            //Type of projectile being shot

    private PlayerStats playerStats;        //Reference to the PlayerStats script
    private EnemyStats enemyStats;          //Reference to the EnemyStats script
    private ProjectileShoot projectile;     //Reference to the ProjectioleShoot script
    private DestructableWalls dWalls;       //Reference to the DestructableWalls script


    //Use this for initialization
    public virtual void Start()
    {
        //Get the components from the projectile's parent object
        projectile = transform.GetComponentInParent<ProjectileShoot>();
        playerStats = transform.GetComponentInParent<PlayerStats>();
        enemyStats = transform.GetComponentInParent<EnemyStats>();

        //Set the start position as the spawned position
        startPos = transform.position;

        //Check for an assigned maximum distance
        if (projectile.maxRange != 0)
        {
            maxDistance = projectile.maxRange;
        }
        //Cap the distance
        else
        {
            maxDistance = 100f;
        }

        //If the parent is a player assign their stats to the projectile
        if (playerStats != null)
        {
            playerProjectile = true;
            enemyProjectile = false;

            critChance = playerStats.combat.critChance;
            damage = projectile.abilityDamage + playerStats.combat.attack;
            direction = transform.GetComponentInParent<CollisionController>().collisions.faceDir;
            enemyStats = null;
        }

        //If the parent is an enemy assign their stats to the projectile
        if (enemyStats != null)
        {
            playerProjectile = false;
            enemyProjectile = true;

            critChance = enemyStats.combat.critChance;
            damage = projectile.abilityDamage + enemyStats.combat.attack;

            //Set the direction to the enemy's face direction
            if (GetComponentInParent<CollisionController>() != null)
            {
                direction = GetComponentInParent<CollisionController>().collisions.faceDir;
            }
            //Set the direction so the enemy shoots forward
            else
            {
                direction = 1;
            }
            playerStats = null;
        }

        //Set the default to forward if no player or enemy stats found
        if (enemyStats == null && playerStats == null)
        {
            direction = 1;
            damage = projectile.abilityDamage;
        }

        //No longer need the parent's information
        transform.parent = null;
    }

    //Destroys the projectile
    public void DestroyProjectile()
    {
        Destroy(gameObject);
    }


    //Tracks how far a projectile has moved and destroys it
    public void CalculateDistance()
    {
        //Calculates the distance from the start position to the current position
        float distance = Vector2.Distance(startPos, transform.position);

        //When distance is greater than max distance destroy the projectile;
        if (distance > maxDistance)
        {
            DestroyProjectile();
        }
    }

    //Collision with other objects
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //If the projectile hits a player
        if (collision.tag == "Player")
        {
            //Get the player's stats
            playerStats = collision.GetComponent<PlayerStats>();

            //Check for an enemy projectile
            if (enemyProjectile)
            {
                //Deal damage to the player
                playerStats.TakeDamage(damage, critChance);
                playerStats.canTakeDamage = false;

                //Destroy the projectile
                DestroyProjectile();
            }

            //Check for a trap projectile
            if (!enemyProjectile && !playerProjectile)
            {
                //Deal damage to the player
                playerStats.TakeDamage(damage, critChance);
                playerStats.canTakeDamage = false;

                //Destroy the projectile
                DestroyProjectile();
            }
        }

        //If the projectile hits an enemy
        if (collision.tag == "Enemy")
        {
            //Get the enemy's stats
            enemyStats = collision.GetComponent<EnemyStats>();

            //Check for a player projectile
            if (playerProjectile)
            {
                //Deal damage to the enemy
                enemyStats.TakeDamage(damage, critChance);

                //Destroy the projectile
                DestroyProjectile();
            }
        }

        //If the projectile hits an obstacle
        if (collision.tag == "Obstacle")
        {
            //Check for a player projectile
            if(playerProjectile)
            {
                dWalls = collision.GetComponent<DestructableWalls>();

                //Check if the wall is a destructable
                if (dWalls)
                {
                    dWalls.SpawnParticles();
                }
            }

            //Destroy the projectile
            DestroyProjectile();
        }
    }
}