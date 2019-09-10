//Created by Robert Bryant
//
//Handles the player's stats and death
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : Stats
{
    [Header("Combat Stuff")]
    public bool inCombat;                       //Check if in combat
    public float combatTime;                    //Amount of time spent in combat
    public float regenTime;                     //Amount of time till next regeneration happens

    [Header("Level Up")]
    public float healthIncrease = 10f;          //Amount of health increased on level up
    public float attackIncrease = 5f;           //Amount of attack increased on level up

    [HideInInspector]
    public bool canTakeDamage;                  //Check if the player can take damage

    private float combatTimer;                  //Combat timer
    private float regenTimer;                   //Regeneration timer
    private float damageTimer;                  //Damage Timer
    private PlayerController playerController;  //Reference to the Player Controller script

    public delegate void UpdateUI(PlayerStats stats);
    public static event UpdateUI PlayerUI;

    //Use this for initialization
    public override void Start()
    {
        //Get the required components in the scene
        playerController = GetComponent<PlayerController>(); 

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
        damageTimer = combat.invulnTime;

        //Update the player UI
        UpdatePlayerUI();

        //Allow the player to take damage
        canTakeDamage = true;

        base.Start();
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
                regenTimer = regenTime;
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

        //Count down the damage timer
        if (!canTakeDamage)
        {
            damageTimer -= Time.deltaTime;

            //Reset the damage timer and change the animation state
            if (damageTimer <= 0.0f)
            {
                //anim.SetBool("OnHit", false);
                damageTimer = combat.invulnTime;
                canTakeDamage = true;
            }
        }
    }

    //Gaining Experience
    public override void GainExperience(int amount)
    {
        base.GainExperience(amount);

        //Update the player UI
        UpdatePlayerUI();
    }

    //Gaining Levels
    public override void LevelUp()
    {
        base.LevelUp();
        IncreaseMaxHealth(healthIncrease);
        ChangeAttack(attackIncrease);

        //Update the player UI
        UpdatePlayerUI();
    }

    //Taking Damage
    public override void TakeDamage(float amount, float critChance)
    {
        //If the player can take damage
        if (canTakeDamage)
        {
            //Put the player in combat and reset the combat timer
            inCombat = true;
            combatTimer = combatTime;
            damageTimer = combat.invulnTime;
            base.TakeDamage(amount, critChance);

            //Start the damaged animation
            //anim.SetBool("OnHit", true);

            //Update the player UI
            UpdatePlayerUI();

            //Play the specified sound            
        }
    }

    //Restoring health
    public override void RestoreHealth(float amount)
    {
        base.RestoreHealth(amount);

        //Update the player UI
        UpdatePlayerUI();
    }

    //Restore the shields
    public override void RestoreShields(float amount)
    {
        base.RestoreShields(amount);

        //Update the player UI
        UpdatePlayerUI();
    }

    //Death
    public override void Die()
    {
        base.Die();

        //Disable the PlayerController script
        transform.GetComponent<PlayerController>().enabled = false;
        
        //Restart the scene
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

    //Updates the health and experience bars
    private void UpdatePlayerUI()
    {
        if (PlayerUI != null)
        {
            PlayerUI.Invoke(this);
        }
    }
}