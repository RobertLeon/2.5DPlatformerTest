using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileShoot : MonoBehaviour
{
    public Transform projectile;        //Projectile being fired
    public Transform abilityUser;       //User of the ability
    public float abilityDamage;         //Damage the ability does
    public float maxRange;              //Maximum range of the projectile

    private void Start()
    {
        //If the ability user is not specified use the current transform
        if (abilityUser == null)
            abilityUser = transform;
    }

    //Creates the specified projectile
    public void ShootProjectile()
    {
        Instantiate(projectile, abilityUser.position, abilityUser.rotation, abilityUser);
    }
}