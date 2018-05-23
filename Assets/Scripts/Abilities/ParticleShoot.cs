//Created by Robert Bryant
//
//Activates the particle system for an ability
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleShoot : MonoBehaviour
{
    [HideInInspector]
    public Transform particles;         //Projectile being fired
    [HideInInspector]
    public Transform abilityUser;           //User of the ability
    [HideInInspector]
    public float abilityDamage;             //Damage the ability does


    private ParticleSystem particleSys;     //Reference to the particle system
    
    //Use this for initialization
    public void Initialize()
    {
        Instantiate(particles.gameObject,abilityUser);
        particleSys = GetComponentInChildren<ParticleSystem>();
    }

    public void ActivateParticle()
    {
        particleSys.Play();
    }
}
