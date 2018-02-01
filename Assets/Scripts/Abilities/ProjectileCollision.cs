using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCollision : MonoBehaviour
{
    public Vector2 velocity;

    private float damage;
    private Vector2 startPos;
    private float critChance;
    private float maxDistance;
    private float distance;
    private int direction;
    private PlayerStats playerStats;
    private EnemyStats enemyStats;
    private ProjectileShoot projectile;
    private bool playerProjectile;
    private bool enemyProjectile;


	//Use this for initialization
	void Start()
	{
        
        projectile = transform.GetComponentInParent<ProjectileShoot>();
        playerStats = transform.GetComponentInParent<PlayerStats>();
        enemyStats = transform.GetComponentInParent<EnemyStats>();

        startPos = transform.position;

        if (projectile.maxRange != 0)
        {
            maxDistance = projectile.maxRange;
        }
        else
        {
            maxDistance = 100f;
        }

        if(playerStats != null)
        {
            playerProjectile = true;
            enemyProjectile = false;

            critChance = playerStats.combat.critChance;
            damage = projectile.abilityDamage + playerStats.combat.attack;
            direction = transform.GetComponentInParent<CollisionController>().collisions.faceDir;
        }

        if(enemyStats != null)
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
        transform.Translate(velocity.x * direction, velocity.y, 0);

        if(distance > maxDistance)
        {
            DestroyProjectile();
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if (playerProjectile)
        {
            if (other.tag == "Enemy")
            {
                enemyStats = other.GetComponent<EnemyStats>();
                enemyStats.TakeDamage(damage, critChance);

                DestroyProjectile();
            }

            if(other.tag == "Untagged")
            {
                DestroyProjectile();
            }
        }

        if(enemyProjectile)
        {
            if(other.tag == "Player")
            {
                playerStats = other.GetComponent<PlayerStats>();
                playerStats.TakeDamage(damage, critChance);

                DestroyProjectile();
            }

            if (other.tag == "Untagged")
            {
                DestroyProjectile();
            }
        }
    }

    private void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}