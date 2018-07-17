//Created by Robert Bryant
//
//Basic particle ability
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Shows an option to create a Scriptable Object in the Unity Editor
[CreateAssetMenu(fileName = "Particle Ability", menuName = "Ability/Particle", order = 2)]
public class ParticleAbility : Ability
{
    public Transform particles;             //Projectile to create for the ability
    [HideInInspector]
    public Transform abilityUser;           //User of the ability
    public float abilityDamage;             //Damage the ability does

    private ParticleShoot shoot;            //Reference to the ParticleShoot script
    private PlayerStats playerStats;        //Reference to the PlayerStats script

    //Initialization of the ability
    public override void Initialize(GameObject obj)
    {
        shoot = obj.GetComponent<ParticleShoot>();
        playerStats = obj.GetComponent<PlayerStats>();
        shoot.abilityUser = obj.transform;
        shoot.particles = particles;
        shoot.Initialize();       
    }

    //Activate the ability
    public override void TriggerAbility()
    {
        shoot.particles = particles;
        shoot.abilityDamage = abilityDamage;        
        shoot.ActivateParticle();        
    }

    //Formats the ability's description for use as a tool tip
    public override string DescribeAbility()
    {
        string format;

        if (playerStats != null)
        {
            float damage = abilityDamage + playerStats.combat.attack;
            format = string.Format(abilityDescription, damage);
        }
        else
        {
            format = string.Format(abilityDescription, abilityDamage);
        }

        return format;
    }
}
