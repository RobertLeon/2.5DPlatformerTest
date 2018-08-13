//Created by Robert Bryant
//
//Handles the logic for a projectile that rotates around the caster
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingProjectile : MonoBehaviour {

    public Vector3 offset;                              //Offset of the projectile
    public Vector3 axis;                                //Axis for the projectile to rotate around
    public float speed;                                 //Speed of the projectile
    public ProjectileAbility projectileAbility;         //Projectile ability the projectile uses
    public ParticleAbility particleAbility;             //Particle ability the projectile uses

    private ProjectileShoot projectileShoot;            //Reference to the Projectile Shoot script
    private ParticleShoot particleShoot;                //Reference to the Particle Shoot script
    private float shotTimer;                            //Timer for the cooldown on the ability

	// Use this for initialization
	void Start ()
    {
        //Set the position of the projectile
        transform.SetPositionAndRotation(transform.parent.position + offset, Quaternion.identity);

        //Get the projectile and particle shoot components
        projectileShoot = GetComponent<ProjectileShoot>();
        particleShoot = GetComponent<ParticleShoot>();

        //Assign projectile variables
        if(projectileShoot != null)
        {
            projectileShoot.abilityUser = transform;
            projectileShoot.projectile = projectileAbility.projectile;
            projectileShoot.maxRange = projectileAbility.maxRange;
            projectileShoot.numberOfProjectiles = projectileAbility.numberOfProjectiles;
            projectileShoot.abilityDamage = projectileAbility.abilityDamage;
        }

        //Assing particle variables
        if(particleAbility != null)
        {
            particleShoot.abilityUser = transform;
            particleShoot.particles = particleAbility.particles;
            particleShoot.abilityDamage = particleAbility.abilityDamage;
            particleShoot.Initialize();
        }
        
        //Set the shot timer to 0
        shotTimer = 0;
	}

    // Update is called once per frame
    void Update()
    {
        //Increase the shot time
        shotTimer += Time.deltaTime;

        //Rotate the projectile
        transform.RotateAround(transform.parent.position, axis, speed * Time.deltaTime);
    }

    //If an enemy is within range
    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Enemy")
        {   
            //Shoot a projectile
            if(projectileShoot != null && shotTimer > projectileAbility.abilityCooldown)
            {
                projectileShoot.ShootProjectile();
                shotTimer = 0;
            }
            //Activate a particle effect
            else if(particleShoot != null && shotTimer > particleAbility.abilityCooldown)
            {
                particleShoot.ActivateParticle();
                shotTimer = 0;
            }
        }
    }

}
