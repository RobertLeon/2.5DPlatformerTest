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
    public bool playerProjectile;           //Type of projectile being shot
    [HideInInspector]
    public bool enemyProjectile;            //Type of projectile being shot

    private PlayerStats playerStats;        //Reference to the PlayerStats script
    private EnemyStats enemyStats;          //Reference to the EnemyStats script
    private ProjectileShoot projectile;     //Reference to the ProjectioleShoot script
    

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
    private void OnTriggerEnter(Collider other)
    {
        //If this is a player projectile
        if (playerProjectile)
        {
            //Check for collision with an enemey
            if (other.tag == "Enemy")
            {
                enemyStats = other.GetComponent<EnemyStats>();
                enemyStats.TakeDamage(damage, critChance);

                DestroyProjectile();
            }

            //Or collision with an object
            if (other.tag == "Obstacle")
            {
                DestroyProjectile();
            }
        }

        //If this is an enemey projectile
        if (enemyProjectile)
        {
            //Check for collision with the player
            if (other.tag == "Player")
            {
                playerStats = other.GetComponent<PlayerStats>();
                playerStats.TakeDamage(damage, critChance);
                playerStats.canTakeDamage = false;

                DestroyProjectile();
            }

            //Or collision with another object
            if (other.tag == "Obstacle")
            {
                DestroyProjectile();
            }
        }
    }
}
