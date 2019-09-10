//Created by Robert Bryant
//Based on a tutorial by: Sebastian Lague
//Github: https://github.com/SebLague/2DPlatformer-Tutorial
//Handles the player's input

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class PlayerInput : MonoBehaviour
{
    private PauseMenu pauseMenu;                    //Reference to the Pause Menu script     
    private InputManager inputManager;              //Reference to the Input Manager script
    private Vector2 directionalInput;               //The amount the player moves in a certain direction
    private float deadZone;                         //Deadzone for gamepad thumb sticks

    public delegate void JumpInuptUp();
    public delegate void JumpInputDown();
    public delegate void DirectionalInput(Vector2 input);
    public delegate void Interact();
    public static event JumpInuptUp JumpButtonUp;
    public static event JumpInputDown JumpButtonDown;
    public static event DirectionalInput MovementInput;
    public static event Interact ActivateObject;

    //Use this for initialization
    void Start()
    {
        //Get references in the scene       
        pauseMenu = FindObjectOfType<PauseMenu>();
        inputManager = FindObjectOfType<InputManager>();

        //Activate the Camera Controller script
        Camera.main.GetComponent<CameraController>().enabled = true;

        if (inputManager != null)
        {
            //Load deadzone from file
            deadZone = inputManager.GetDeadZone();
        }
        else
        {
            Debug.LogError("Error:Input Manager not found. \n Current Scene Number: " + UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings);
        }

        //Initialize the pause menu
        if (pauseMenu != null)
        {
            pauseMenu.InitializePauseMenu();
        }
    }

    //Updates the deadzone during play
    public void UpdateDeadZone(float dZone)
    {
        deadZone = dZone;
    }

    //Update is called once per frame
    void Update()
    {
        if (inputManager != null)
        {
            //Gamepad  input movement
            GamepadAxisMovement(deadZone);

            //Keyboard input movmenet
            KeyBoardMovement();

            //Set the input in the PlayerController input
            MovementInput.Invoke(directionalInput);

            //Jump input being pressed               
            if (inputManager.GetKeyDown("Jump") || inputManager.GetButtonDown("Jump"))
            {
                JumpButtonDown.Invoke();
            }

            //Jump input being released
            if (inputManager.GetKeyUp("Jump") || inputManager.GetButtonUp("Jump"))
            {
                JumpButtonUp.Invoke();
            }

            if (inputManager.GetKeyDown("Interact"))
            {
                ActivateObject();
            }
        }       
    }
    //Check for keyboard movment inputs
    private void KeyBoardMovement()
    {
        //Moving left via button input
        if (inputManager.GetKey("Move Left"))
        {
            directionalInput.x = -1f;
        }

        //Moving right via button input
        if (inputManager.GetKey("Move Right"))
        {
            directionalInput.x = 1f;
        }

        //Moving up via button input
        if (inputManager.GetKey("Move Up"))
        {
            directionalInput.y = 1f;
        }

        //Moving down via button input
        if (inputManager.GetKey("Move Down"))
        {
            directionalInput.y = -1f;
        }

        //Stop moving up or down when releasing the movement button
        if(inputManager.GetKeyUp("Move Up") || inputManager.GetKeyUp("Move Down"))
        {
            directionalInput.y = 0f;
        }

        //Stop moving left or right when releasing the movement button button
        if (inputManager.GetKeyUp("Move Left") || inputManager.GetKeyUp("Move Right"))
        {
            directionalInput.x = 0f;
        }
    }
    
    //Check for Gamepad movement inputs
    private void GamepadAxisMovement(float dZone)
    {
        //Gamepad Thumb stick input
        float xInput = XCI.GetAxis(XboxAxis.LeftStickX);
        float yInput = XCI.GetAxis(XboxAxis.LeftStickY);

        //Check for dead zone on the x axis
        if(Mathf.Abs(xInput) < dZone)
        {
            directionalInput.x = 0f; 
        }
        else
        {
            if (xInput > 0.5f)
            {
                directionalInput.x = 1f;
            }
            else if (xInput < -0.5f)
            {
                directionalInput.x = -1f;
            }
            else
            {
                directionalInput.x = 0f;
            }
        }

        //Check for dead zone on the y axis
        if(Mathf.Abs(yInput) < dZone)
        {
            directionalInput.y = 0;
        }
        else
        {
            if (yInput > 0.5f)
            {
                directionalInput.y = 1f;
            }
            else if (yInput < -0.5f)
            {
                directionalInput.y = -1f;
            }
            else
            {
                directionalInput.y = 0;
            }
        }


        //Check to move left
        if (inputManager.GetButton("Move Left"))
        {
            directionalInput.x = -1f;
        }
        
        //Check to move right
        if (inputManager.GetButton("Move Right"))
        {
            directionalInput.x = 1f;
        }

        //Check to move up
        if (inputManager.GetButton("Move Up"))
        {
            directionalInput.y = 1f;
        }

        //Check to move down
        if (inputManager.GetButton("Move Down"))
        {
            directionalInput.y = -1f;
        }

        //Check if the vertical movement buttons have been released
        if (inputManager.GetButtonUp("Move Up") || inputManager.GetButtonUp("Move Down"))
        {
            directionalInput.y = 0f;
        }

        //Check if the horizontal movement buttons have been released
        if (inputManager.GetButtonUp("Move Left") || inputManager.GetButtonUp("Move Right"))
        {
            directionalInput.x = 0f;
        }
    }
}