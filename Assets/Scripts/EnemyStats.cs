using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : Stats
{
    
    private void Start()
    {
        //Initialize the enemy's stats
        health.currentHealth = health.maxHealth;
    }

    //Taking Damage
    public override void TakeDamage(float amount, float critChance)
    {
        base.TakeDamage(amount, critChance);
    }
}