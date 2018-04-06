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
    public KeyCode kbPause;
    public KeyCode kbInteract;

    //Xbox Controller input
    [Header("Controller Input")]
    public KeyCode ctJump;
    public KeyCode ctAbility1;
    public KeyCode ctPause;
    public KeyCode ctInteract;

    private PlayerController player;        //Reference to the PlayerController script


    //Use this for initialization
    void Start()
    {
        //Get the player controller script
        player = GetComponent<PlayerController>();
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