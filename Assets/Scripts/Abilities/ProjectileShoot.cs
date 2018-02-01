using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileShoot : MonoBehaviour
{
    public Transform projectile;
    public Transform abilityUser;
    public float abilityDamage;
    public float maxRange;

    private void Start()
    {
        if (abilityUser == null)
            abilityUser = transform;
    }

    public void ShootProjectile()
    {
        Instantiate(projectile, abilityUser.position, abilityUser.rotation, abilityUser);
    }
}