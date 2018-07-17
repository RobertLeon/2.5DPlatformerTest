//Created by Robert Bryany
//
//Creates an item that modifies the player's health stats
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Health Item", menuName = "Items/Health Item", order = 1)]
public class HealthItem : Items
{
    [Range(5, 50)]
    public float maxHealth;             //Amount of maximum health the item gives
    [Range(5, 50)]
    public float maxShields;            //Amount of maximum shield the item gives
    [Range(1, 20)]
    public float healthRegen;           //Amount of health regeneration the item gives
    [Range(1, 10)]
    public float shieldRegen;           //Amount of shield regeneration the item gives
    public bool fullRestore = false;    //Does the item fully restore health or shields?
    public bool onHit = false;          //Does the item do something when the owner is hit

    //Initialize the item
    public override void OnPickUp(Stats stats)
    {
        //Increase the specified stats by a given amount
        stats.IncreaseMaxHealth(maxHealth, fullRestore);
        stats.IncreaseMaxShields(maxShields, fullRestore);
        stats.IncreaseHealthRegen(healthRegen);
        stats.IncreaseShieldRegen(shieldRegen);
    }
}

    