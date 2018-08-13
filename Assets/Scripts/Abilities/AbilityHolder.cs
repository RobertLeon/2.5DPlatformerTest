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
    private PlayerInput playerInput;                    //Reference to the Player Input script
    private float globalTimer;                          //Global cool down timer
    private bool globalCoolDown;                        //Check if the global cool sown is active

	//Use this for initialization
	void Start()
	{
        //Set
        playerChar = GameManager.Instance.Player;
        playerInput = GetComponent<PlayerInput>();
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
        if((Input.GetKey(abilityButtons[0].controllerInput) 
            || Input.GetKey(abilityButtons[0].keyboardInput)) 
            && abilityButtons[0].coolDownComplete && globalCoolDown)
        {
            abilityButtons[0].ActivateAbility();
            GlobalCoolDown();
        }

        //Activate the second ability
        if ((Input.GetKey(abilityButtons[1].controllerInput)
            || Input.GetKey(abilityButtons[1].keyboardInput))
            && abilityButtons[1].coolDownComplete && globalCoolDown)
        {
            abilityButtons[1].ActivateAbility();
            GlobalCoolDown();
        }

        //Activate the third ability
        if ((Input.GetKey(abilityButtons[2].controllerInput)
            || Input.GetKey(abilityButtons[2].keyboardInput))
            && abilityButtons[2].coolDownComplete && globalCoolDown)
        {
            abilityButtons[2].ActivateAbility();
            GlobalCoolDown();
        }

        //Activate the fourth ability
        if ((Input.GetKey(abilityButtons[3].controllerInput)
            || Input.GetKey(abilityButtons[3].keyboardInput))
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
                        playerInput.kbAbility1, playerInput.ctAbility1);
                    break;

                case 1:
                    abilityButtons[num].Initialize(abilities[1], transform.gameObject,
                        playerInput.kbAbility2, playerInput.ctAbility2);
                    break;

                case 2:
                    abilityButtons[num].Initialize(abilities[2], transform.gameObject,
                        playerInput.kbAbility3, playerInput.ctAbility3);
                    break;

                case 3:
                    abilityButtons[num].Initialize(abilities[3], transform.gameObject,
                        playerInput.kbAbility4, playerInput.ctAbility4);
                    break;

                default:
                    Debug.LogError("Error initializing ability buttons");
                    break;
            }
        }
    }
}
