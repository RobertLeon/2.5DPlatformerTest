//Created by Robert Bryant
//
//Base class for the stats and death of each entity in the game
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//Stats for various entities in the game
public class Stats : MonoBehaviour
{
    //Health Stats
    [System.Serializable]
    public struct Health
    {
        [Range(10, 1000)]
        public float maxHealth;            //Maximum amount of health
        [HideInInspector]
        public float currentHealth;        //Current amount of health
        [Range(25, 100)]
        public float maxShields;           //Maximum amount of shields
        [HideInInspector]
        public float currentShields;       //Current amount of shields
        [Range(0, 20)]
        public float shieldRegen;          //Amount restored to shields
        [Range(0, 20)]
        public float healthRegen;          //Amount restored to health
    }

    //Combat Stats
    [System.Serializable]
    public struct Combat
    {
        [Range(1, 20)]
        public float attack;               //Attack
        [Range(1, 25)]
        public float defense;              //Defense
        [Range(0.01f, 0.1f)]
        public float critChance;           //Critical hit chance
        [Range(1, 10)]
        public float attackSpeed;          //Attack Speed
    }

    //Movement Stats
    [System.Serializable]
    public struct Movement
    {
        [Range(0,4)]
        public float speedModifier;         //Move speed modifier
        [Range(0,5)]
        public float jumpHeightModifier;    //Jump height modifier
        [HideInInspector]
        public int numJumps;                //Set the number of jumps an entity can perform
    }

    //Experience
    [System.Serializable]
    public struct Experience
    {
        public int currentLevel;            //Current Level
        public int currentExp;              //Current Experience
        public int[] expLevels;             //Amount of experince per level
        public int maxLevel;                //Level cap
    }


    [Header("Health")]
    public Health health;

    [Header("Attack Modifiers")]
    public Combat combat;

    [Header("Movement Modifiers")]
    public Movement movement;

    [Header("Experience")]
    public Experience exp;

    public Transform textSpawn;
    public TMP_Text floatingText;
    public Color damageColor;
    public Color healColor;
    [HideInInspector]
    public GameObject damageCanvas;


    private void Awake()
    {
        damageCanvas = GameObject.FindGameObjectWithTag("DamageCanvas");
    }

    //Increases the maximium health
    public void IncreaseMaxHealth(float amount, bool restore = false)
    {
        //Increase health by the given amount
        health.maxHealth += amount;

        //If true heal to full
        if (restore)
            RestoreHealth(health.maxHealth);
    }


    //Increase the maximum amount of shields
    public void IncreaseMaxShields(float amount, bool restore = false)
    {
        //Increase the maximum by a given amount
        health.maxShields += amount;

        //If true restore to full
        if (restore)
            RestoreShields(health.maxShields);
    }

    //Increase the amount of health regenerates
    public void IncreaseHealthRegen(float amount)
    {
        health.healthRegen += amount;
    }

    //Increase the amount shield regenerates
    public void IncreaseShieldRegen(float amount)
    {
        health.shieldRegen += amount;
    }

    //Change the amount of attack
    public void ChangeAttack(float amount)
    {
        combat.attack += amount;
    }

    //Change the amount of defense
    public void ChangeDefense(float amount)
    {
        combat.defense += amount;

        //Mininum cap for defense
        if(combat.defense < 0)
        {
            combat.defense = 0;
        }

    }

    //Change the attack speed
    public void ChangeAttackSpeed(float amount)
    {
        combat.attackSpeed += amount;

        //Maximum cap for combat speed
        if(combat.attackSpeed < 1.0f)
        {
            combat.attackSpeed = 1.0f;
        }
    }

    //Change the crit chance
    public void ChangeCritChance(float amount)
    {
        combat.critChance += amount;

        //Maximum cap for crit chance
        if (combat.critChance > 1.0f)
            combat.critChance = 1.0f;
    }

    //Increase Movement speed
    public virtual void IncreaseSpeed(float amount)
    {
        movement.speedModifier += amount;

        //Maximum cap for speed Modifier
        if(movement.speedModifier > 4)
        {
            movement.speedModifier = 4;
        }
    }

    //Increase the height of jumps
    public virtual void IncreaseJumpHeight(float amount)
    {
        movement.jumpHeightModifier += amount;
    }

    //Increase the number of jumps possible
    public void IncreaseJumps()
    {
        movement.numJumps++;
    }

    //Gaining Experince
    public virtual void GainExperience(int amount)
    {
        exp.currentExp += amount;

        if (exp.currentExp >= exp.expLevels[exp.currentLevel - 1])
        {
            //Carry over extra experience to the next level
            exp.currentExp = exp.currentExp - exp.expLevels[exp.currentLevel - 1];

            if (exp.currentLevel != exp.maxLevel)
            {
                LevelUp();
            }
        }
    }

    //Increase level
    public virtual void LevelUp()
    {
        exp.currentLevel++;

        //If at max level stop experience gain
        if (exp.currentLevel > exp.maxLevel)
        {
            exp.currentLevel = exp.maxLevel;
        }
    }

    //Taking Damage
    public virtual void TakeDamage(float amount, float critChance)
    {
        //
        float crit = Random.value;
        float damage = amount;

        //If the attack is a critical hit double damage
        if (critChance >= crit)
        {
            damage *= 2;
        }

        //Reduce damge dealt by the amount of defense
        damage -= Mathf.Round(combat.defense / 2);

        if (damage <= 0)
        {
            damage = 0;
        }

        //Reduce the current shield amount by damage taken
        health.currentShields -= damage;

        //Spawn floating text for damage
        if (damage >= 1)
        {
            TMP_Text dmgText = Instantiate(floatingText) as TMP_Text;
            dmgText.text = damage.ToString();
            dmgText.color = new Color(damageColor.r, damageColor.g, damageColor.b);
            dmgText.transform.SetParent(damageCanvas.transform);
            dmgText.transform.position = textSpawn.position;
        }

        //If the shields is less than 0 reduce health by the amount left
        if (health.currentShields < 0)
        {
            damage = -health.currentShields;
            health.currentHealth -= damage;
            health.currentShields = 0;
        }

        //Check for death
        if (health.currentHealth <= 0)
        {
            health.currentHealth = 0;            
            Die();
        }
    }

    //Death
    public virtual void Die()
    {
        Debug.Log(transform.name + " has died.");
    }

    //Restoring health
    public virtual void RestoreHealth(float amount)
    {
        health.currentHealth += amount;

        if (health.currentHealth < health.maxHealth)
        {
            //Spawn floating text for healing
            TMP_Text healText = Instantiate(floatingText) as TMP_Text;
            healText.text = amount.ToString();
            healText.color = new Color(healColor.r, healColor.g, healColor.b);
            healText.transform.SetParent(damageCanvas.transform);
            healText.transform.position = textSpawn.position;
        }
        //If the amount restored is more than maximum health
        //set current health to maximum
        if (health.currentHealth >= health.maxHealth)
        {
            health.currentHealth = health.maxHealth;
        }
    }

    //Restoring Shields
    public virtual void RestoreShields(float amount)
    {
        health.currentShields += amount;

        //If the amount restored is more than maximum shields
        //set current shields to maximum
        if (health.currentShields >= health.maxShields)
        {
            health.currentShields = health.maxShields;
        }
    }

}