//Created by Robert Bryant
//
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileShoot : MonoBehaviour
{
    [HideInInspector]
    public Transform projectile;        //Projectile being fired
    [HideInInspector]
    public Transform abilityUser;       //User of the ability
    [HideInInspector]
    public float abilityDamage;         //Damage the ability does
    [HideInInspector]
    public float maxRange;              //Maximum range of the projectile


    //Creates the specified projectile
    public void ShootProjectile()
    {
        Instantiate(projectile, abilityUser.position, abilityUser.rotation, abilityUser);
    }
}