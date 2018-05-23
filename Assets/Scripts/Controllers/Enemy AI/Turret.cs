//Created by Robert Bryant
//
//Handles the information for a single turret and the different types of abilities it uses
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Components required for this script to function
[RequireComponent(typeof(ProjectileShoot),typeof(ParticleShoot))]
public class Turret : MonoBehaviour
{
    public ProjectileAbility projectileAbility;         //Projectile abilitry to be used on the turret
    public ParticleAbility particleAbility;             //Particle ability to be used on the turret
    public Transform turret;                            //Where the shot is going to originate from

    private ProjectileShoot projectileShoot;            //Reference to ProjectileShoot
    private ParticleShoot particleShoot;                //Reference to ProjectileShoot

    //
    private void Start()
    {
        //Get the projectileShoot and particleShoot components for use in the script
        projectileShoot = GetComponent<ProjectileShoot>();
        particleShoot = GetComponent<ParticleShoot>();

        //If there is no projectile ability assigned skip this
        if(projectileAbility != null)
        {
            //Set the necessary information in the projectile shoot class
            projectileShoot.abilityUser = turret;
            projectileShoot.projectile = projectileAbility.projectile;
            projectileShoot.maxRange = projectileAbility.maxRange;
            projectileShoot.abilityDamage = projectileAbility.abilityDamage;

        }

        //If there is no particle ability assigned skip this.
        if(particleAbility != null)
        {
            //Set the necessary information for the particle ability and initialize it
            particleShoot.abilityUser = turret;
            particleShoot.particles = particleAbility.particles;
            particleShoot.abilityDamage = particleAbility.abilityDamage;
            particleShoot.Initialize();
        }

    }

    //Shoots a projectile from the turret
    public void ShootProjectile()
    {
        //If the projectile ability isn't assigned dont fire a projectile
        if (projectileAbility != null)
        {   
            projectileShoot.ShootProjectile();
        }
    }

    //Shoots particles from the turret
    public void ShootParticles()
    {
        //If the particle ability isn't assinged dont activate the particle system
        if(particleAbility != null)
        { 
            particleShoot.ActivateParticle();
        }
    }
}
