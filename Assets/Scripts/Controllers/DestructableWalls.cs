//Created by Robert Bryant
//
//Handles the collision for destuctable walls
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableWalls : MonoBehaviour
{
    public int hitPoints = 3;                   //Amount of times the wall needs to be hit

    private ParticleSystem dustParticles;       //Reference to the Particle System component

	// Use this for initialization
	void Start ()
    {
        dustParticles = GetComponent<ParticleSystem>();
	}

    //Spawns particles and handles the wall's health
    public void SpawnParticles()
    {
        //Play particles
        dustParticles.Play();

        //Remove one hit point
        hitPoints -= 1;

        //Check if the wall has health
        if(hitPoints <= 0)
        {
            Destroy(gameObject);
        }

    }

}
