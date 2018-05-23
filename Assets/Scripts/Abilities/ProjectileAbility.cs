//Created by Robert Bryant
//
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Shows an option to create a Scriptable Object in the Unity Editor
[CreateAssetMenu(fileName ="Projectile Ability", menuName ="Ability/Projectile", order = 1)]
public class ProjectileAbility : Ability
{
    public Transform projectile;            //Projectile to create for the ability
    [HideInInspector]
    public Transform abilityUser;           //User of the ability
    public float abilityDamage;             //Damage the ability does
    public float maxRange;                  //Maximum range of the ability

    private ProjectileShoot shoot;          //Reference to the ProjectileShoot script

    //Initialization of the ability
    public override void Initialize(GameObject obj)
    {
        shoot = obj.GetComponent<ProjectileShoot>();
        abilityUser = obj.transform;
        shoot.abilityUser = abilityUser;
        
    }

    //Activating the ability
    public override void TriggerAbility()
    {
        shoot.projectile = projectile;
        shoot.abilityDamage = abilityDamage;
        shoot.maxRange = maxRange;                 
        shoot.ShootProjectile();
    }
}