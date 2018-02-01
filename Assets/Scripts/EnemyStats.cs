using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : Stats
{

    private void Start()
    {
        health.currentHealth = health.maxHealth;
    }

    public override void TakeDamage(float amount, float critChance)
    {
        base.TakeDamage(amount, critChance);
    }
}