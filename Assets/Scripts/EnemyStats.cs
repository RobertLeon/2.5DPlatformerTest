//Created by Robert Bryant
//
//Handles the enemy's stats and death
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyStats : Stats
{
    public int expAmount;
    public Color expColor;

    private PlayerStats playerStats;

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


    public override void Die()
    {
        base.Die();

        TMP_Text expText = Instantiate(floatingText);
        expText.text = expAmount.ToString();
        expText.color = new Color(expColor.r, expColor.g, expColor.b);
        expText.transform.SetParent(damageCanvas.transform);
        expText.transform.position = textSpawn.position;

        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        playerStats.GainExperience(expAmount);
        gameObject.SetActive(false);
    }

}