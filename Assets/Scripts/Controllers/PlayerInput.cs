﻿//Created by Robert Bryant
//Based on a tutorial by: Sebastian Lague
//Github: https://github.com/SebLague/2DPlatformer-Tutorial
//Handles the player's input

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
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
    public KeyCode kbMap;

    //Xbox Controller input
    [Header("Controller Input")]
    public KeyCode ctJump;
    public KeyCode ctAbility1;
    public KeyCode ctAbility2;
    public KeyCode ctAbility3;
    public KeyCode ctAbility4;
    public KeyCode ctPause;
    public KeyCode ctInteract;
    public KeyCode ctMap;

    private CameraController playerCam;             //
    private Character playerChar;                   //
    private PlayerController playerController;      //Reference to the PlayerController script
    private PauseMenu pauseMenu;                    //
    private AbilityCooldown[] coolDownButtons;      //
    private Ability[] abilities;                    //

    //Use this for initialization
    void Start()
    {
        //Get the player controller script
        playerController = GetComponent<PlayerController>();

        Camera.main.GetComponent<CameraController>().enabled = true;
        playerChar = GameManager.Instance.Player;

        if (playerChar != null)
        {
            coolDownButtons = FindObjectsOfType<AbilityCooldown>();

            abilities = playerChar.characterAbilities;
        }

        pauseMenu = FindObjectOfType<PauseMenu>();

        //Find the pause menu in the scene
        if (pauseMenu != null)
        {
            pauseMenu.InitializePauseMenu();
        }

        if (abilities != null)
        {            
            for (int i = 0; i < abilities.Length; i++)
            {
                coolDownButtons[i].Initialize(playerChar.characterAbilities[i], gameObject, i);                
            }

            for(int i = 0; i < coolDownButtons.Length; i++)
            {
                if(i >= abilities.Length)
                {
                    coolDownButtons[i].gameObject.SetActive(false);
                }
            }
        }
        else
        {
            Debug.LogWarning("No abilities found");
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
        playerController.SetDirectionalInput(directionalInput);

        //Jump input being pressed
        if (Input.GetKeyDown(kbJump) || Input.GetKeyDown(ctJump))
        {
            playerController.OnJumpInputDown();
        }

        //Jump input being released
        if (Input.GetKeyUp(kbJump) || Input.GetKeyUp(ctJump))
        {
            playerController.OnJumpInputUp();
        }
    }
}