//Created by Robert Bryant
//Based off of Unity Tuttorial
//https://unity3d.com/learn/tutorials/topics/scripting/ability-system-scriptable-objects
//Basic projectile abiliy
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
    public int numberOfProjectiles;         //Number of projectiles the ability has
    public float abilityDamage;             //Damage the ability does
    public float maxRange;                  //Maximum range of the ability

    private ProjectileShoot shoot;          //Reference to the ProjectileShoot script
    private PlayerStats playerStats;        //Reference to the Player Stats script

    //Initialization of the ability
    public override void Initialize(GameObject obj)
    {
        shoot = obj.GetComponent<ProjectileShoot>();
        playerStats = obj.GetComponent<PlayerStats>();
        abilityUser = obj.transform;
        shoot.abilityUser = abilityUser;
    }

    //Activating the ability
    public override void TriggerAbility()
    {
        shoot.projectile = projectile;
        shoot.abilityDamage = abilityDamage;
        shoot.maxRange = maxRange;
        shoot.numberOfProjectiles = numberOfProjectiles;
        shoot.ShootProjectile();
    }

    //Formats the ability's description for use as a tool tip
    public override string DescribeAbility()
    {
        string format;

        if(playerStats != null)
        {
            float damage = abilityDamage + playerStats.combat.attack;
            format = string.Format(abilityDescription, numberOfProjectiles, damage);
        }
        else
        {
            format = string.Format(abilityDescription, numberOfProjectiles, abilityDamage);
        }      

        return format;
    }
}