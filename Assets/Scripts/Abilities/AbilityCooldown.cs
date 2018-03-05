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
    public int abilityNumber;               //Number of the ability on the user

    [SerializeField]
    private Ability ability;                //Ability being used
    [SerializeField]
    private GameObject abilityUser;         //The user of the ability
    private Image abilityImage;             //Image for the ability
    private KeyCode kbInput;                //Input to use the ability
    private float coolDownDuration;         //How long inbetween attacks
    private float coolDownTimeLeft;         //Timer for the cooldown
    private float nextReadyTime;            //Time for next ability use
    private float userAttackSpeed;          //Attack speed of the user


	//Use this for initialization
	void Start()
	{
        
        Initialize(ability, abilityUser);
	}

    //Initalize the ability
    void Initialize(Ability selectedAbilty, GameObject user)
    {
        ability = selectedAbilty;
        abilityImage = cooldownIcon.GetComponent<Image>();
        abilityImage.sprite = ability.abilitySprite;
        cooldownMask.sprite = ability.abilitySprite;
        userAttackSpeed = user.GetComponent<Stats>().combat.attackSpeed;

        coolDownDuration = ability.abilityCooldown / userAttackSpeed;
        
        kbInput = user.GetComponent<PlayerInput>().kbAbility[abilityNumber];
        ability.Initialize(user);

    }

    private void Update()
    {
        bool coolDownComplete = (Time.time > nextReadyTime);

        //If the ability is not on cooldown
        if (coolDownComplete)
        {
            AbilityReady();
            
            //Activate the ability on input
            if (Input.GetKeyDown(kbInput))
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
        cooldownMask.fillAmount = (coolDownTimeLeft / coolDownDuration);
    }
}