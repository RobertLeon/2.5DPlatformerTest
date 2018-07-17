//Created by Robert Bryant
//
//Displays the players current health and experience
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    public Image healthBar;                 //Reference to the health bar
    public Image shieldBar;                 //Reference to the shield bar
    public Image expBar;                    //Reference to the exp bar
    public TMP_Text healthText;             //Text display for the health bar
    public TMP_Text expText;                //Text display for the experience bar
    public TMP_Text levelText;              //Text display for the players's current level
    public Gradient healthGradient;         //Gradient for the health bar
    public Color shieldColor;               //Color for the shield bar
    public Color expColor;                  //Color for the experince bar

    private float playerMaxHP;              //Player's maximum health value
    private float playerCurrentHP;          //Player's current health value
    private float playerMaxShield;          //Player's maximum shield value
    private float playerCurrentShield;      //Player's current shield value
    private float playerCurrentExp;         //Player's current experience value
    private float playerMaxExp;             //Player's maximum experience value

    //Used for initialization
    private void Start()
    {
        //Set the shield bar's and experience bar's color
        shieldBar.color = shieldColor;
        expBar.color = expColor;
    }

    //Update the information the health bar shows
    public void UpdateHealthBar(PlayerStats stats)
	{
        //Set the player's health and shield values
        playerMaxHP = stats.health.maxHealth;
        playerCurrentHP = stats.health.currentHealth;
        playerMaxShield = stats.health.maxShields;
        playerCurrentShield = stats.health.currentShields;

        //Set the fill amount based on the current values
        healthBar.fillAmount = playerCurrentHP / playerMaxHP;
        shieldBar.fillAmount = playerCurrentShield / playerMaxShield;
        healthBar.color = healthGradient.Evaluate(playerCurrentHP / playerMaxHP);
        
        //Display the player's total health values
        healthText.text = (playerCurrentShield + playerCurrentHP) + " / " + (playerMaxHP + playerMaxShield);
    }

    //Update the information on the experience bar
    public void UpdateExpBar(PlayerStats stats)
    {
        //Set the players current experience values
        playerCurrentExp = stats.exp.currentExp;

        //Show the maximum experience based on the player's current level        
        playerMaxExp = stats.exp.expLevels[stats.exp.currentLevel - 1];
        levelText.text = stats.exp.currentLevel.ToString();        

        //Set the experience text and fill amount
        expBar.fillAmount = (playerCurrentExp / playerMaxExp);
        expText.text = playerCurrentExp + "/ " + playerMaxExp;
        
    }
}
