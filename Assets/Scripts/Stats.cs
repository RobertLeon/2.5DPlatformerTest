using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Health
{
        public float maxHealth;
        public float currentHealth;
        public float maxShields;
        public float currentShields;
        public float shieldRegen;
        public float healthRegen;
}
[System.Serializable]
public struct Combat
{
    public float attack;
    public float defense;
    public float critChance;
    public float attackSpeed;
}
[System.Serializable]
public struct Movement
{
    public float speedModifier;
    public float jumpHeightModifier;
    public int numJumps;
}
[System.Serializable]
public struct Experience
{
    public int currentLevel;
    public int currentExp;
    public int maxLevel;
    public int[] expLevels;
}

public class Stats : MonoBehaviour
{
    [Header("Health")]
    public Health health;

    [Header("Attack Modifiers")]
    public Combat combat;

    [Header("Movement Modifiers")]
    public Movement movement;

    [Header("Experience")]
    public Experience exp;

    //Gaining Health
    public void IncreaseMaxHealth(float amount)
    {
        IncreaseMaxHealth(amount, false);
    }

   
    public void IncreaseMaxHealth(float amount, bool restore = false)
    {
        health.maxHealth += amount;

        if (restore)
            RestoreHealth(health.maxHealth);
    }

    //Gaining Shields
    public void IncreaseMaxShields(float amount)
    {
        IncreaseMaxShields(amount, false);
    }

    public void IncreaseMaxShields(float amount, bool restore = false)
    {
        health.maxShields += amount;

        if (restore)
            RestoreShields(health.maxShields);
    }

    //
    public void IncreaseHealthRegen(float amount)
    {
        health.healthRegen += amount;
    }

    //
    public void IncreaseShieldRegen(float amount)
    {
        health.shieldRegen += amount;
    }

    //
    public void ChangeAttack(float amount)
    {
        combat.attack += amount;
    }

    //
    public void ChangeDefense(float amount)
    {
        combat.defense += amount;
    }

    //
    public void ChangeAttackSpeed(float amount)
    {
        combat.attackSpeed += amount;
    }

    //
    public void ChangeCritChance(float amount)
    {
        combat.critChance += amount;

        if (combat.critChance >= 1.0f)
            combat.critChance = 1.0f;
    }

    //
    public virtual void IncreaseSpeed(float amount)
    {
        movement.speedModifier += amount;
    }

    //
    public virtual void IncreaseJumpHeight(float amount)
    {
        movement.jumpHeightModifier += amount;
    }

    //
    public void IncreaseJumps()
    {
        movement.numJumps++;
    }

    //Gaining Experince
    public void GainExperience(int amount)
    {
        exp.currentExp += amount;

        if (exp.currentExp >= exp.expLevels[exp.currentLevel - 1])
        {
            exp.currentExp = exp.currentExp - exp.expLevels[exp.currentLevel - 1];
            LevelUp();           
        }
    }

    //
    public virtual void LevelUp()
    {
        exp.currentLevel++;

        if (exp.currentLevel > exp.maxLevel)
        {
            exp.currentLevel = exp.maxLevel;
            exp.currentExp = 0;
        }
    }

    //Taking Damage
    public virtual void TakeDamage(float amount, float critChance)
    {
        float crit = Random.Range(0.00f, 1.00f);
        float damage = amount;
        
        if (critChance >= crit)
        {
            damage *= 2;
        }

        damage -= combat.defense;

        health.currentShields -= damage;

        if (health.currentShields < 0)
        {
            damage = -health.currentShields;
            health.currentHealth -= damage;
            health.currentShields = 0;
        }

        if (health.currentHealth <= 0)
        {
            health.currentHealth = 0;            
            Die();
        }

        Debug.Log(transform.name + " has taken " + damage + " damage");
    }

    //Death
    public virtual void Die()
    {
        Debug.Log(transform.name + " has died.");
    }

    //Restoring health
    public void RestoreHealth(float amount)
    {
        health.currentHealth += amount;

        if (health.currentHealth >= health.maxHealth)
        {
            health.currentHealth = health.maxHealth;
        }
    }

    //Restoring Shields
    public void RestoreShields(float amount)
    {
        health.currentShields += amount;

        if (health.currentShields >= health.maxShields)
        {
            health.currentShields = health.maxShields;
        }
    }

}