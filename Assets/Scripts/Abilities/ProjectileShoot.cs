//Created by Robert Bryant
//Based off of Unity Tuttorial
//https://unity3d.com/learn/tutorials/topics/scripting/ability-system-scriptable-objects
//Handles the activation of the Projectile Ability
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
    [HideInInspector]
    public int numberOfProjectiles;     //The number of projectiles the ability shoots
    [HideInInspector]
    public float shotDelay;             //Delay between each projectile being fired


    //Creates the specified projectiles
    public void ShootProjectile()
    {
        //Sets the number of projectiles to 1 if the value is 0 or negative
        if(numberOfProjectiles <= 0 )
        {
            numberOfProjectiles = 1;
        }

        //Have no delay for a single projectile
        if(numberOfProjectiles == 1)
        {
            shotDelay = 0f;
        }
 
        //Delayed shots
        StartCoroutine(Shoot());     
    }


    //Instatiates the projectiles after a short delay
    private IEnumerator Shoot()
    {
       for (int i = 0; i < numberOfProjectiles; i++)
        {
            Instantiate(projectile, abilityUser.position, abilityUser.rotation, abilityUser);
            yield return new WaitForSeconds(shotDelay);
         } 
    }
}