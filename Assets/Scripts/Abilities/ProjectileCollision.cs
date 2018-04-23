using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCollision : MonoBehaviour
{
    public Vector2 velocity;                //Speed of the ability on the x and y axis

    private float damage;                   //Amount of damage this object does
    private Vector2 startPos;               //Starting position
    private float critChance;               //Chance to double damage
    private float maxDistance;              //Maximum distance object can travel
    private float distance;                 //Current distance the object has traveled
    private int direction;                  //Direction the object is moving in
    private PlayerStats playerStats;        //Reference to the PlayerStats script
    private EnemyStats enemyStats;          //Reference to the EnemyStats script
    private ProjectileShoot projectile;     //Reference to the ProjectioleShoot script
    private bool playerProjectile;          //Type of projectile being shot
    private bool enemyProjectile;           //Type of projectile being shot


	//Use this for initialization
	void Start()
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
        if(playerStats != null)
        {
            playerProjectile = true;
            enemyProjectile = false;

            critChance = playerStats.combat.critChance;
            damage = projectile.abilityDamage + playerStats.combat.attack;
            direction = transform.GetComponentInParent<CollisionController>().collisions.faceDir;
        }

        //If the parent is an enemy assign their stats to the projectile
        if (enemyStats != null)
        {
            playerProjectile = false;
            enemyProjectile = true;

            critChance = enemyStats.combat.critChance;
            damage = projectile.abilityDamage + playerStats.combat.attack;
            direction = transform.GetComponentInParent<CollisionController>().collisions.faceDir;
        }

        //No longer need the parent's information
        transform.parent = null;
	}

	//Update is called once per frame
	void Update()
	{
        
        distance = Vector2.Distance(startPos, transform.position);
        //Move the projectile based on the velocity
        transform.Translate(velocity.x * direction, velocity.y, 0);

        //When distance is greater than max distance destroy the projectile;
        if(distance > maxDistance)
        {
            DestroyProjectile();
        }
	}

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
            if(other.tag == "")
            {
                DestroyProjectile();
            }
        }

        //If this is an enemey projectile
        if(enemyProjectile)
        {
            //Check for collision with the player
            if(other.tag == "Player")
            {
                playerStats = other.GetComponent<PlayerStats>();
                playerStats.TakeDamage(damage, critChance);

                DestroyProjectile();
            }

            //Or collision with another object
            if (other.tag == "Untagged")
            {
                DestroyProjectile();
            }
        }
    }

    //Destroys the projectile
    private void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}