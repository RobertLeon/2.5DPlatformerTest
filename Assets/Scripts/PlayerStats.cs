using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : Stats
{
    [Header("Combat Stuff")]
    public bool inCombat;
    public float combatTime;
    public float regenTime;

    [Header("Level Up")]
    public float healthIncrease = 10f;
    public float attackIncrease = 5f;

    private float combatTimer;
    private float regenTimer;
    private PlayerController playerController;


	//Use this for initialization
	void Start()
	{
        playerController = GetComponent<PlayerController>();

        if (exp.currentLevel == 0)
        {
            exp.currentLevel = 1;
        }
        
        health.currentHealth = health.maxHealth;
        health.currentShields = health.maxShields;
        exp.maxLevel = exp.expLevels.Length;
        combatTimer = combatTime;
        regenTimer = regenTime;
	}

	//Update is called once per frame
	void Update()
	{
        if (inCombat)
        {
            combatTimer -= Time.deltaTime;            

            if (combatTimer <= 0)
            {
                combatTimer = combatTime;
                inCombat = false;
            }
        }
        else
        {
            regenTimer -= Time.deltaTime;

            if (regenTimer <= 0)
            {
                RestoreHealth(health.healthRegen);
                RestoreShields(health.shieldRegen);
                regenTimer = regenTime;
            }
        }
	}

    //Gaining Experince
    public override void LevelUp()
    {
        base.LevelUp();
        IncreaseMaxHealth(healthIncrease);
        ChangeAttack(attackIncrease);
    }

    //Taking Damage
    public override void TakeDamage(float amount, float critChance)
    {
        inCombat = true;
        combatTimer = combatTime;
        base.TakeDamage(amount, critChance);
    }

    public override void Die()
    {
        base.Die();
        StartCoroutine(RestartScene());
    }

    //Restarts current scene
    private IEnumerator RestartScene()
    {
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public override void IncreaseJumpHeight(float amount)
    {
        base.IncreaseJumpHeight(amount);
        playerController.UpdateMovement();
    }

    public override void IncreaseSpeed(float amount)
    {
        base.IncreaseSpeed(amount);
        playerController.UpdateMovement();
    }

    //For Testing Purposes
    private void OnGUI()
    {
        GUILayout.Label("HP: " + health.currentHealth + " / " + health.maxHealth);
        GUILayout.Label("Shields: " + health.currentShields + " / " + health.maxShields);
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