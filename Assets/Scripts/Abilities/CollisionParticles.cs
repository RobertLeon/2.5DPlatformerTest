//Created by Robert Bryant
//
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionParticles : MonoBehaviour
{
    private ParticleSystem particleSys;
    private ParticleShoot particleShoot;
    private PlayerStats playerStats;
    private EnemyStats enemyStats;
    private List<ParticleSystem.Particle> enterParticles = new List<ParticleSystem.Particle>();
    private bool playerParticles;
    private bool enemyParticles;
    private float damage;


	//Use this for initialization
	void Start()
	{
        playerStats = GetComponentInParent<PlayerStats>();
        enemyStats = GetComponentInParent<EnemyStats>();
        particleSys = GetComponent<ParticleSystem>();
        particleShoot = GetComponentInParent<ParticleShoot>();

        if(playerStats != null)
        {
            playerParticles = true;
            enemyParticles = false;

            damage = playerStats.combat.attack + particleShoot.abilityDamage;
        }

        if(enemyStats != null)
        {
            enemyParticles = true;
            playerParticles = false;

            damage = enemyStats.combat.attack + particleShoot.abilityDamage;
        }
	}

    private void OnParticleCollision(GameObject other)
    {
        if(other.tag == "Enemy")
        { Debug.Log("Hello"); }
    }

}
