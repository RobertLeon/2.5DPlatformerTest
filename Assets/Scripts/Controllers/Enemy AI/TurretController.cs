//Created by Robert Bryant
//
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{  
    public Turret[] turrets;                    //Turrets
    public float shootTime = 1.0f;              //Time to fire the first volley
    public bool incrementalShots = false;       //Delayed turret fire
    public float incrementTime = 0f;            //Time between shots

    private float shotTime;                     //Timer for the shots
    private float incShotTime;                  //Incremental shot timer
    private int currentTurret;                  //Current turret counter

    //Use this for initialization
    void Start()
    {
        incShotTime = shootTime;
        currentTurret = 0;
    }

    //Update is called once per frame
    void Update()
    {
        //Reset the turret number and shot timer
        if(currentTurret > turrets.Length - 1)
        {
            currentTurret = 0;
            shotTime = 0;
            incShotTime = shootTime;
        }

        //Delayed shots
        if (incrementalShots)
        {
            //Fire a projectile turret
            if (turrets[currentTurret].projectileAbility != null)
            {
                if (shotTime > incShotTime)
                {
                    turrets[currentTurret].ShootProjectile();
                    currentTurret++;
                    incShotTime += incrementTime;
                }
            }
            //Fire a particle turret
            else if (turrets[currentTurret].particleAbility != null)
            {
                if (shotTime > incShotTime)
                {
                    turrets[currentTurret].ShootParticles();
                    currentTurret++;
                    incShotTime += incrementTime;
                }
            }
        }
        //Fire all turrets at once
        else
        {
            if (shotTime > shootTime)
            {
                //Fire each turret
                foreach (Turret gun in turrets)
                {
                    if (gun.particleAbility != null)
                    {
                        gun.ShootParticles();
                    }
                    else if (gun.projectileAbility != null)
                    {
                        gun.ShootProjectile();
                    }
                }
                shotTime = 0;
            }
        }

        //Increase the timer for when to shoot
        shotTime += Time.deltaTime;
    }
}
