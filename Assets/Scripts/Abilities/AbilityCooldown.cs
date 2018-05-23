//Created by Robert Bryant
//
//

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
    
    [SerializeField]
    private Ability ability;                //Ability being used    
    private Image abilityImage;             //Image for the ability
    private KeyCode kbInput;                //Keyboard input to use the ability
    private KeyCode ctInput;                //Controller input for the ability
    private float coolDownDuration;         //How long inbetween attacks
    private float coolDownTimeLeft;         //Timer for the cooldown
    private float nextReadyTime;            //Time for next ability use
    private float userAttackSpeed;          //Attack speed of the user
    private int abilityNumber;


    //Initalize the ability
    public void Initialize(Ability selectedAbilty, GameObject user, int abilityNum)
    {
        ability = selectedAbilty;
        abilityImage = cooldownIcon.GetComponent<Image>();
        abilityImage.sprite = ability.abilitySprite;
        cooldownMask.sprite = ability.abilitySprite;
        userAttackSpeed = user.GetComponent<Stats>().combat.attackSpeed;
        abilityNumber = abilityNum;


        switch (abilityNumber)
        {
            case 0:
                kbInput = user.GetComponent<PlayerInput>().kbAbility1;
                ctInput = user.GetComponent<PlayerInput>().ctAbility1;
                break;

            case 1:
                kbInput = user.GetComponent<PlayerInput>().kbAbility2;
                ctInput = user.GetComponent<PlayerInput>().ctAbility2;
                break;

            case 2:
                kbInput = user.GetComponent<PlayerInput>().kbAbility3;
                ctInput = user.GetComponent<PlayerInput>().ctAbility3;
                break;

            case 3:
                kbInput = user.GetComponent<PlayerInput>().kbAbility4;
                ctInput = user.GetComponent<PlayerInput>().ctAbility4;
                break;

            default:
                throw new System.Exception("Ability number not recognized");
        }

        coolDownDuration = ability.abilityCooldown / userAttackSpeed;
        ability.Initialize(user);
        AbilityReady();
    }

    private void Update()
    {
        bool coolDownComplete = (Time.time > nextReadyTime);

        //If the ability is not on cooldown
        if (coolDownComplete)
        {
            AbilityReady();
            
            //Activate the ability on input
            if (Input.GetKeyDown(kbInput) || Input.GetKeyDown(ctInput))
            {
                ActivateAbility();
            }
        }
        //Count down till the ability is ready again
        else
        {
            CoolDown();
        }
    }

    //Hide the cooldown mask and text
    void AbilityReady()
    {
        cooldownDisplay.enabled = false;
        cooldownMask.enabled = false;
    }


    //Using the ability
    void ActivateAbility()
    {
        nextReadyTime = coolDownDuration + Time.time;
        coolDownTimeLeft = coolDownDuration;
        cooldownMask.enabled = true;
        cooldownDisplay.enabled = true;

        ability.TriggerAbility();
    }

    //Set the cooldown for the ability
    void CoolDown()
    {
        coolDownTimeLeft -= Time.deltaTime;
        float roundedCD = Mathf.Round(coolDownTimeLeft);
        cooldownDisplay.text = roundedCD.ToString();
        coolDownDuration = ability.abilityCooldown / userAttackSpeed;
        cooldownMask.fillAmount = (coolDownTimeLeft / coolDownDuration);
    }
}