//Created by Robert Bryant
//Based off of Unity Tutorial
//https://unity3d.com/learn/tutorials/topics/scripting/ability-system-scriptable-objects
//Basic raycast ability
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Raycast Ability", menuName ="Ability/Raycast", order = 3)]
public class RaycastAbility : Ability
{

    public float abilityDamage;                     //Damage the ability does
    public float abilityRange;                      //Range of the ability
    public float abilityDuration;                   //How long the ability lasts
    public Color abilityColor;                      //Color of the raycast
    public Material abilityMaterial;                //Material for the raycast

    private RaycastShoot rcShoot;                   //Reference to the RaycastShoot script
    private PlayerStats playerStats;                //Reterence to the PlayerStats script

    //Initialize the ability for use by the player
    public override void Initialize(GameObject obj)
    {
        //Assign the RaycastShoot and PlayerStats scripts
        rcShoot = obj.GetComponent<RaycastShoot>();
        playerStats = obj.GetComponent<PlayerStats>();
        rcShoot.Initialize();
    }

    //Triggers the ability
    public override void TriggerAbility()
    {
        rcShoot.abilityDamage = abilityDamage;
        rcShoot.abilityRange = abilityRange;
        rcShoot.abilityDuration = abilityDuration;
        rcShoot.lineRenderer.material = abilityMaterial;
        rcShoot.lineRenderer.material.color = abilityColor;

        rcShoot.Shoot();
    }


    //Formats the ability's description for use as a tool tip
    public override string DescribeAbility()
    {
        string format;

        if (playerStats != null)
        {
            float damage = abilityDamage + playerStats.combat.attack;
            format = string.Format(abilityDescription, damage);
        }
        else
        {
            format = string.Format(abilityDescription, abilityDamage);
        }

        return format;
    }
}
