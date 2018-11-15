//Created by Robert Bryant
//
//Activates the particle system for an ability
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleShoot : MonoBehaviour
{
    [HideInInspector]
    public Transform particles;             //Particles being fired
    [HideInInspector]
    public Transform abilityUser;           //User of the ability
    [HideInInspector]
    public float abilityDamage;             //Damage the ability does


    private ParticleSystem particleSys;     //Reference to the particle system
    
    //Use this for initialization
    public void Initialize()
    {
        Debug.Log(transform.name + " particle shoot initialized");
    }

    //Activates the particles being used
    public void ActivateParticle()
    {
        //Create the particle system as a child of the user and gets the particle system component.
        Instantiate(particles.gameObject, abilityUser);
        
        particleSys = GetComponentInChildren<ParticleSystem>();
        particleSys.Play();
    }
}
