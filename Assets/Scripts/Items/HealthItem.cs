using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Health Item", menuName = "Items/Health Item", order = 1)]
public class HealthItem : Items
{
    public float maxHealth;
    public float maxShields;
    public float healthRegen;
    public float shieldRegen;
    public bool fullRestore = false;


    public override void Initialize(PlayerStats playerStats)
    {
        playerStats.IncreaseMaxHealth(maxHealth, fullRestore);
        playerStats.IncreaseMaxShields(maxShields, fullRestore);
        playerStats.IncreaseHealthRegen(healthRegen);
        playerStats.IncreaseShieldRegen(shieldRegen);
    }
}

    