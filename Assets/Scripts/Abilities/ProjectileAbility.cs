using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Projectile Ability", menuName ="Ability/Projectile", order = 1)]
public class ProjectileAbility : Ability
{
    public Transform projectile;            //Projectile to create for the ability
    public float abilityDamage;             //Damage the ability does
    public float maxRange;                  //Maximum range of the ability

    private ProjectileShoot shoot;          //Reference to the ProjectileShoot script

    //Initialization of the ability
    public override void Initialize(GameObject obj)
    {
        shoot = obj.GetComponent<ProjectileShoot>();
        shoot.projectile = projectile;
        shoot.abilityDamage = abilityDamage;
        shoot.maxRange = maxRange;
    }

    //Activating the ability
    public override void TriggerAbility()
    {
        shoot.ShootProjectile();
    }
}