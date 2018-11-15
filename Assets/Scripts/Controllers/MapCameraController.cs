//Created by Robert Bryant
//
//Handles the map camera in the map menu screen
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class MapCameraController : MonoBehaviour
{
    public Transform cameraTarget;                  //Target for the camera to focus on
    public GameObject mapMenu;                      //Map menu game object
    public GameObject pauseMenu;                    //Pause menu game object
    public GameObject pauseMenuOptions;             //Pause menu options game object
    public bool isMapOpen = false;                  //Is the map open?
    public float moveSpeed = 2f;                    //Speed the camera moves when
    public float minSize = 50f;                     //How far out you can zoom the camera
    public float maxSize = 200f;                    //How close in you can zoom the camera
    public float zoomAmount = 50f;                  //Amount you move the camera in/out


    private InputManager inputManager;              //Reference to the Input Manager
    private PauseMenu pause;                        //Reference to the Pause Menu script
    private Vector3 dragOrigin;                     //Origin of the mouse drag
    private Camera cam;                             //Reference to the camera component
    private Vector2 directionalInput;               //
    private float deadZone;

    //Use this for initialization
    void Start()
    {
        //Get the Pause menu and Camera components from the scene
        pause = pauseMenu.GetComponentInParent<PauseMenu>();
        cam = GetComponent<Camera>();
        inputManager = FindObjectOfType<InputManager>();
        deadZone = GameManager.Instance.LoadInputs().deadZone;
    }

    //Update is called once per frame
    void Update()
    {
        //If the map is not open
        if (!isMapOpen && !pause.paused)
        {
            //If a map button was pressed open the map and pause the game
            if (inputManager.GetKey("Open Map") || inputManager.GetButtonDown("Open Map"))
            {
                pause.Pause();
                mapMenu.SetActive(true);
                pauseMenuOptions.SetActive(false);
                pauseMenu.SetActive(true);
                isMapOpen = true;
            }

        }
        //The map is open
        else if (isMapOpen)
        {
            //If the map button was pressed close the map and resume playing
            if (inputManager.GetKey("Open Map") || inputManager.GetButtonDown("Open Map"))
            {
                pause.Resume();
                mapMenu.SetActive(false);
                pauseMenu.SetActive(false);
                pauseMenuOptions.SetActive(true);
                isMapOpen = false;
            }
        }
    }


    //Updates after each frame
    private void LateUpdate()
    {
        //Check if the map is open
        if (isMapOpen)
        {
            //Moving the camera with keyboard or controller input            
            GamepadAxisMovement(deadZone);
            KeyBoardMovement();

            //Moves the camera
            transform.Translate(directionalInput);

            //Zooms the camera out
            if (inputManager.GetKeyDown("Zoom Out") || inputManager.GetButtonDown("Zoom Out"))
            { 
                ZoomOut();
            }

            //Zooms the camera in
            if(inputManager.GetKeyDown("Zoom In") || inputManager.GetButtonDown("Zoom In"))
            {
                ZoomIn();
            }

            //DO NOT PUT CODE BELOW THIS FUNCTION
            //Moves the camera with mouse drag
            MouseMovement();
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
        if (inputManager.GetKeyUp("Move Up") || inputManager.GetKeyUp("Move Down"))
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
        if (Mathf.Abs(xInput) < dZone)
        {
            directionalInput.x = 0f;
        }
        else
        {
            directionalInput.x = xInput;
        }

        //Check for dead zone on the y axis
        if (Mathf.Abs(yInput) < dZone)
        {
            directionalInput.y = 0;
        }
        else
        {
            directionalInput.y = yInput;
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

    //Mouse drag moves the camera
    private void MouseMovement()
    {
        //Get the mouse position when button is pressed
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = Input.mousePosition;

            return;
        }

        //Do nothing if mouse button is not pressed
        if (!Input.GetMouseButton(0))
        {
            return;
        }

        //Get the movement of the mouse from the origin and calculate the amount to move the camera
        Vector2 camPos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
        Vector2 mouseMove = new Vector2(camPos.x * moveSpeed, camPos.y * moveSpeed);

        //Move the camera
        transform.Translate(mouseMove, Space.World);
    }

    //Zoom the camera in
    public void ZoomIn()
    {
        //Check if the camera can be zoomed in
        if (cam.orthographicSize >= minSize)
        {
            cam.orthographicSize -= zoomAmount;
        }
    }

    //Zoom the camera out
    public void ZoomOut()
    {
        //Check if the camera can be zoomed out
        if (cam.orthographicSize <= maxSize)
        {
            cam.orthographicSize += zoomAmount;
        }
    }

    //Set the map being open
    public void OpenMap()
    {
        isMapOpen = true;
    }

    //Set the map being closed
    public void CloseMap()
    {
        isMapOpen = false;
    }
}
