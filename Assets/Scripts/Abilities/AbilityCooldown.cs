//Created by Robert Bryant
//Based off of Unity Tuttorial
//https://unity3d.com/learn/tutorials/topics/scripting/ability-system-scriptable-objects
//Activates the ability and handles the ability's cooldown
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AbilityCooldown : MonoBehaviour
{
    public Image cooldownIcon;              //Icon of the ability
    public Image cooldownMask;              //Cooldown mask
    public TMP_Text cooldownDisplay;        //Cooldown text
    public GameObject descriptionBox;       //Ability description box
    public int abilityNumber;               //Number of the ability
    [HideInInspector]
    public bool coolDownComplete;           //Check if the cool down has completed
    [HideInInspector]
    public string inputName;                //Name of the input

    [SerializeField]
    private Ability ability;                //Ability being used    
    private Image abilityImage;             //Image for the ability
    private float coolDownDuration;         //How long inbetween attacks
    private float coolDownTimeLeft;         //Timer for the cooldown
    private float nextReadyTime;            //Time for next ability use
    

    //Initalize the ability
    public void Initialize(Ability selectedAbilty, GameObject user, string input)
    {
        //Assign the ability's information
        ability = selectedAbilty;
        abilityImage = cooldownIcon.GetComponent<Image>();
        abilityImage.sprite = ability.abilitySprite;
        cooldownMask.sprite = ability.abilitySprite;
        inputName = input;

        //Set the cooldown, initialize the ability and ready it for use
        coolDownDuration = ability.abilityCooldown;
        ability.Initialize(user);
        AbilityReady();
    }

    //
    private void Update()
    {
        //Check if the ability cooldown has completed
        coolDownComplete = (Time.time > nextReadyTime);

        //If the ability is not on cooldown
        if (coolDownComplete)
        {
            AbilityReady();
        }
        //Count down till the ability is ready again
        else
        {
            CoolDown();
        }
    }

    //Hide the cooldown mask and text
    private void AbilityReady()
    {
        cooldownDisplay.enabled = false;
        cooldownMask.enabled = false;
    }


    //Using the ability
    public void ActivateAbility()
    {
        nextReadyTime = coolDownDuration + Time.time;
        coolDownTimeLeft = coolDownDuration;
        cooldownMask.enabled = true;
        cooldownDisplay.enabled = true;


        ability.TriggerAbility();
    }

    //Set the cooldown for the ability
    private void CoolDown()
    {
        coolDownTimeLeft -= Time.deltaTime;
        float roundedCD = Mathf.Round(coolDownTimeLeft);
        cooldownDisplay.text = roundedCD.ToString();
        coolDownDuration = ability.abilityCooldown;
        cooldownMask.fillAmount = (coolDownTimeLeft / coolDownDuration);
    }

    //Shows the Ability's description on mouse over
    public void MouseEnter()
    {
        //Show the description box at a set position above the ability
        descriptionBox.SetActive(true);
        descriptionBox.transform.position = transform.position + new Vector3(0,100,0);

        //Change the text in the description box to show the current ability
        TMP_Text descriptionText = descriptionBox.GetComponentInChildren<TMP_Text>();
        descriptionText.text = ability.abilityName +"\n" + ability.DescribeAbility();
    }

    //Hides the abilty's description
    public void MouseExit()
    {
        descriptionBox.SetActive(false);
    }
}