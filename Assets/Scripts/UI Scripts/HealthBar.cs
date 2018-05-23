//Created by Robert Bryant
//
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    public Image healthBar;
    public Image shieldBar;
    public Image expBar;
    public TMP_Text healthText;
    public TMP_Text expText;
    public TMP_Text levelText;
    public Gradient healthGradient;
    public Color shieldColor;
    public Color expColor;

    private float playerMaxHP;
    private float playerCurrentHP;
    private float playerMaxShield;
    private float playerCurrentShield;
    private float playerCurrentExp;
    private float playerMaxExp;

    private void Start()
    {
        shieldBar.color = shieldColor;
        expBar.color = expColor;
    }

    //Update the information the health bar shows
    public void UpdateHealthBar(PlayerStats stats)
	{
        playerMaxHP = stats.health.maxHealth;
        playerCurrentHP = stats.health.currentHealth;
        playerMaxShield = stats.health.maxShields;
        playerCurrentShield = stats.health.currentShields;

        healthBar.fillAmount = playerCurrentHP / playerMaxHP;
        shieldBar.fillAmount = playerCurrentShield / playerMaxShield;
        healthBar.color = healthGradient.Evaluate(playerCurrentHP / playerMaxHP);
        
        healthText.text = (playerCurrentShield + playerCurrentHP) + " / " + (playerMaxHP + playerMaxShield);
    }

    //Update the information on the experience bar
    public void UpdateExpBar(PlayerStats stats)
    {
        playerCurrentExp = stats.exp.currentExp;
        if (stats.exp.currentLevel != stats.exp.maxLevel)
        {
            playerMaxExp = stats.exp.expLevels[stats.exp.currentLevel];
            levelText.text = stats.exp.currentLevel.ToString();
        }
        else
        {
            playerMaxExp = stats.exp.expLevels[stats.exp.maxLevel-1];
            levelText.text = stats.exp.maxLevel.ToString();
        }
        expBar.fillAmount = (playerCurrentExp / playerMaxExp);
        expText.text = playerCurrentExp + "/ " + playerMaxExp;
        
    }
}
