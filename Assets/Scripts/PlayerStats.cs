//Created by Robert Bryant
//
//Handles the player's stats and death
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

[RequireComponent(typeof(PlatformController))]
public class PlayerStats : Stats
{
    [Header("Combat Stuff")]
    public bool inCombat;                       //Check if in combat
    public float combatTime;                    //Amount of time spent in combat
    public float regenTime;                     //Amount of time till next regeneration happens

    [Header("Level Up")]
    public float healthIncrease = 10f;          //Amount of health increased on level up
    public float attackIncrease = 5f;           //Amount of attack increased on level up


    private float combatTimer;                  //Combat timer
    private float regenTimer;                   //Regeneration timer
    private PlayerController playerController;  //Reference to the Player Controller
    private HealthBar healthBar;
    private GameObject expBar;

    //Use this for initialization
    private void Start()
    {
        //Get the PlayerController Component
        playerController = GetComponent<PlayerController>();
        healthBar = FindObjectOfType<HealthBar>();
        //expBar = FindObjectOfType<ExpBar>();

        //Set current level to 1
        if (exp.currentLevel <= 0)
        {
            exp.currentLevel = 1;
        }

        //Initialize stats
        movement.numJumps = 1;
        health.currentHealth = health.maxHealth;
        health.currentShields = health.maxShields;
        exp.maxLevel = exp.expLevels.Length;
        combatTimer = combatTime;
        regenTimer = regenTime;

        if (healthBar != null)
        {
            healthBar.UpdateHealthBar(this);
            healthBar.UpdateExpBar(this);
        }

        
    }

	//Update is called once per frame
	void Update()
	{
        //When in combat stop health and shield regeneration
        if (inCombat)
        {
            combatTimer -= Time.deltaTime;

            //Reset the combat timer
            if (combatTimer <= 0)
            {
                combatTimer = combatTime;
                inCombat = false;
            }
        }
        //Start regenerating health and shield to maximum
        else
        {
            regenTimer -= Time.deltaTime;

            //Reset the regeneration timer
            if (regenTimer <= 0)
            {
                RestoreHealth(health.healthRegen);
                RestoreShields(health.shieldRegen);
                regenTimer = regenTime;
            }
        }
	}

    public override void GainExperience(int amount)
    {
        base.GainExperience(amount);

        //Exp Bar
        if(healthBar != null)
        {
            healthBar.UpdateExpBar(this);
        }
    }

    //Gaining Levels
    public override void LevelUp()
    {
        base.LevelUp();
        IncreaseMaxHealth(healthIncrease);
        ChangeAttack(attackIncrease);

        //Health bar & Exp bar
        if (healthBar != null)
        {
            healthBar.UpdateHealthBar(this);
            healthBar.UpdateExpBar(this);
        }
    }

    //Taking Damage
    public override void TakeDamage(float amount, float critChance)
    {
        //Put the player in combat and reset the combat timer
        inCombat = true;
        combatTimer = combatTime;

        base.TakeDamage(amount, critChance);

        if(healthBar != null)
        {
            healthBar.UpdateHealthBar(this);
        }
    }

    public override void RestoreHealth(float amount)
    {
        base.RestoreHealth(amount);

        if (healthBar != null)
        {
            healthBar.UpdateHealthBar(this);
        }
    }

    public override void RestoreShields(float amount)
    {
        base.RestoreShields(amount);

        if (healthBar != null)
        {
            healthBar.UpdateHealthBar(this);
        }
    }

    //Death
    public override void Die()
    {
        base.Die();

        transform.GetComponent<PlayerController>().enabled = false;
        
        StartCoroutine(RestartScene());
    }

    //Restarts current scene
    private IEnumerator RestartScene()
    {
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    //Increase jump height
    public override void IncreaseJumpHeight(float amount)
    {
        base.IncreaseJumpHeight(amount);
        
        //Update the player's movement
        playerController.UpdateMovement();
    }
    
    //Increase movement speed
    public override void IncreaseSpeed(float amount)
    {
        base.IncreaseSpeed(amount);
        
        //Update the player's movement
        playerController.UpdateMovement();
    }

    //For Testing Purposes
    private void OnGUI()
    {
        GUILayout.Label("EXP: " + exp.currentExp + " / " + exp.expLevels[exp.currentLevel-1]);
        
        if(GUILayout.Button("Take Damage"))
        {
            TakeDamage(5, 0.25f);
        }

        if (GUILayout.Button("Gain Health"))
        {
            IncreaseMaxHealth(5);
        }

        if (GUILayout.Button("Restore Health"))
        {
            RestoreHealth(5);
        }

        if (GUILayout.Button("Gain Shields"))
        {
            IncreaseMaxShields(5);
        }

        if (GUILayout.Button("Restore Shields"))
        {
           RestoreShields(5);
        }

        if (GUILayout.Button("Gain EXP"))
        {
            GainExperience(7);
        }
    }

}