//Created by Robert Bryant
//
//Holds the player's abilities and handles the global cool down
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AbilityHolder : MonoBehaviour
{
    public Ability[] abilities;                         //The abilities the player can use
    public float globalCooldownTime = 1f;               //Amount of time inbetween each ability activation    

    private AbilityCooldown[] abilityButtons;           //References to the Ability Cool Down Scripts
    private Character playerChar;                       //Reference to the Player Character script
    private InputManager inputManager;                    //Reference to the Player Input script
    private float globalTimer;                          //Global cool down timer
    private bool globalCoolDown;                        //Check if the global cool down is active

	//Use this for initialization
	void Start()
	{
        //Set
        playerChar = GameManager.Instance.Player;
        inputManager = FindObjectOfType<InputManager>();
        abilities = playerChar.characterAbilities;
        abilityButtons = FindObjectsOfType<AbilityCooldown>();

        //Assign each ability to a specified button
        for(int i = 0; i < abilityButtons.Length; i++)
        {
            AssignAbility(i, abilities[i]);
        }
        
	}

    //
    private void Update()
    {
        globalCoolDown = (Time.time > globalTimer);

        //Count down the global cool down
        if(!globalCoolDown)
        {
            globalTimer -= Time.deltaTime;
        }

        //Check for ability input
        ActivateAbility();
    }


    //Detect ability input
    private void ActivateAbility()
    {
        //Activate the first ability
        if((inputManager.GetKey(abilityButtons[0].inputName) 
            || inputManager.GetButtonDown(abilityButtons[0].inputName)) 
            && abilityButtons[0].coolDownComplete && globalCoolDown)
        {
            abilityButtons[0].ActivateAbility();
            GlobalCoolDown();
        }

        //Activate the second ability
        if ((inputManager.GetKey(abilityButtons[1].inputName)
            || inputManager.GetButtonDown(abilityButtons[1].inputName))
            && abilityButtons[1].coolDownComplete && globalCoolDown)
        {
            abilityButtons[1].ActivateAbility();
            GlobalCoolDown();
        }

        //Activate the third ability
        if ((inputManager.GetKey(abilityButtons[2].inputName)
            || inputManager.GetButtonDown(abilityButtons[2].inputName))
            && abilityButtons[2].coolDownComplete && globalCoolDown)
        {
            abilityButtons[2].ActivateAbility();
            GlobalCoolDown();
        }

        //Activate the fourth ability
        if ((inputManager.GetKey(abilityButtons[3].inputName)
            || inputManager.GetButtonDown(abilityButtons[3].inputName))
            && abilityButtons[3].coolDownComplete && globalCoolDown)
        {
            abilityButtons[3].ActivateAbility();
            GlobalCoolDown();
        }
    }

    
    //Set the timer for the global cool down
    public void GlobalCoolDown()
    {
        globalTimer = globalCooldownTime + Time.time;
        globalCoolDown = false; 
    }


    //Assigns an ability for the player to use
    public void AssignAbility(int num, Ability ability)
    {
        if (ability != null)
        {
            //Assign the ability to the correct button
            switch (abilityButtons[num].abilityNumber)
            {
                case 0:
                    abilityButtons[num].Initialize(abilities[0], transform.gameObject,
                        "Ability 1");
                    break;

                case 1:
                    abilityButtons[num].Initialize(abilities[1], transform.gameObject,
                        "Ability 2");
                    break;

                case 2:
                    abilityButtons[num].Initialize(abilities[2], transform.gameObject,
                        "Ability 3");
                    break;

                case 3:
                    abilityButtons[num].Initialize(abilities[3], transform.gameObject,
                        "Ability 4");
                    break;

                default:
                    Debug.LogError("Error initializing ability buttons");
                    break;
            }
        }
    }
}
