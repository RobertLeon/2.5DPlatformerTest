//Created by Robert Bryant
//
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Shows an option to create a Scriptable Object in the Unity Editor
[CreateAssetMenu(fileName = "Particle Ability", menuName = "Ability/Particle", order = 2)]
public class ParticleAbility : Ability
{
    public Transform particles;            //Projectile to create for the ability
    [HideInInspector]
    public Transform abilityUser;           //User of the ability
    public float abilityDamage;             //Damage the ability does

    private ParticleShoot shoot;

    public override void Initialize(GameObject obj)
    {
        shoot = obj.GetComponent<ParticleShoot>();
        shoot.abilityUser = obj.transform;
        shoot.particles = particles;
        shoot.Initialize();       
    }

    public override void TriggerAbility()
    {
        shoot.particles = particles;
        shoot.abilityDamage = abilityDamage;        
        shoot.ActivateParticle();
    }
}
