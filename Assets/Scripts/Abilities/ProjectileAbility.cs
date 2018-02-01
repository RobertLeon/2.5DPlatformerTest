using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Projectile Ability", menuName ="Ability/Projectile", order = 1)]
public class ProjectileAbility : Abilities
{
    public Transform projectile;
    public float abilityDamage;
    public float maxRange;

    private ProjectileShoot shoot;

    public override void Initialize(GameObject obj)
    {
        shoot = obj.GetComponent<ProjectileShoot>();
        shoot.projectile = projectile;
        shoot.abilityDamage = abilityDamage;
        shoot.maxRange = maxRange;
    }

    public override void TriggerAbility()
    {
        shoot.ShootProjectile();
    }
}