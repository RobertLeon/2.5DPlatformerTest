//Created by Robert Bryant
//Based on a tutorial by: Sebastian Lague
//Github: https://github.com/SebLague/2DPlatformer-Tutorial
//Handles the player's input

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController),typeof(ProjectileShoot))]
public class PlayerInput : MonoBehaviour
{
    //Keyboard input
    [Header("Keyboard Input")]
    public KeyCode kbJump;
    public KeyCode kbAbility1;
    public KeyCode kbAbility2;
    public KeyCode kbAbility3;
    public KeyCode kbAbility4;
    public KeyCode kbPause;
    public KeyCode kbInteract;

    //Xbox Controller input
    [Header("Controller Input")]
    public KeyCode ctJump;
    public KeyCode ctAbility1;
    public KeyCode ctAbility2;
    public KeyCode ctAbility3;
    public KeyCode ctAbility4;
    public KeyCode ctPause;
    public KeyCode ctInteract;

    private PlayerController player;                //Reference to the PlayerController script
    private PauseMenu pauseMenu;                    //
    private AbilityCooldown[] coolDownButtons;      //
    private Ability[] abilities;                    //

    //Use this for initialization
    void Start()
    {
        //Get the player controller script
        player = GetComponent<PlayerController>();

        //Find the pause menu in the scene
        if (GameObject.FindGameObjectWithTag("PauseMenu") != null)
        {
            pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu").GetComponent<PauseMenu>();

            pauseMenu.InitializePauseMenu();
        }

        if (abilities != null)
        {
            abilities = GameManager.Instance.Player.characterAbilities;

            coolDownButtons = FindObjectsOfType<AbilityCooldown>();

            for (int i = 0; i < abilities.Length; i++)
            {
                coolDownButtons[i].Initialize(GameManager.Instance.Player.characterAbilities[i], gameObject, i);
            }
        }
    }

    //Update is called once per frame
    void Update()
    {
        //Set directional input from the horizontal and vertial axis
        Vector2 directionalInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        
        //For controller input
        if(directionalInput.y >= 0.5f)
        {
            directionalInput.y = 1;
        }
        else if(directionalInput.y <= -0.5f)
        {
            directionalInput.y = -1;
        }
        else
        {
            directionalInput.y = 0;
        }

        if(directionalInput.x >= 0.5f)
        {
            directionalInput.x = 1;
        }
        else if (directionalInput.x <= -0.5f)
        {
            directionalInput.x = -1;
        }
        else
        {
            directionalInput.x = 0;
        }


        //Set the input in the PlayerController input
        player.SetDirectionalInput(directionalInput);

        //Jump input being pressed
        if (Input.GetKeyDown(kbJump) || Input.GetKeyDown(ctJump))
        {
            player.OnJumpInputDown();
        }

        //Jump input being released
        if (Input.GetKeyUp(kbJump) || Input.GetKeyUp(ctJump))
        {
            player.OnJumpInputUp();
        }
    }
}