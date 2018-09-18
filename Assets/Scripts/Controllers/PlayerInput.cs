//Created by Robert Bryant
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

    private PlayerController playerController;      //Reference to the Player Controller script
    private PauseMenu pauseMenu;                    //Reference to the Pause Menu script
    private Vector2 directionalInput;               //Input for the direction of the player

    //Use this for initialization
    void Start()
    {
        //Get references in the scene
        playerController = GetComponent<PlayerController>();        
        pauseMenu = FindObjectOfType<PauseMenu>();

        //Activate the Camera Controller script
        //Camera.main.GetComponent<CameraController>().enabled = true;

        //Initialize the pause menu
        if (pauseMenu != null)
        {
            pauseMenu.InitializePauseMenu();
        }
    }

    //Update is called once per frame
    void Update()
    {
        //Set directional input from the horizontal and vertial axis
        directionalInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        //For controller input
        DeadZone(0.5f);

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

    //Handles the amount
    private void DeadZone(float zoneAmount)
    {
        if (directionalInput.y >= zoneAmount)
        {
            directionalInput.y = 1;
        }
        else if (directionalInput.y <= -zoneAmount)
        {
            directionalInput.y = -1;
        }
        else
        {
            directionalInput.y = 0;
        }

        if (directionalInput.x >= zoneAmount)
        {
            directionalInput.x = 1;
        }
        else if (directionalInput.x <= -zoneAmount)
        {
            directionalInput.x = -1;
        }
        else
        {
            directionalInput.x = 0;
        }
    }
}