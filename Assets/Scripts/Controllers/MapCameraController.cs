//Created by Robert Bryant
//
//Handles the map camera in the map menu screen
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCameraController : MonoBehaviour
{
    public Transform cameraTarget;                  //Target for the camera to focus on
    public GameObject mapMenu;                      //Map menu game object
    public GameObject pauseMenu;                    //Pause menu game object
    public GameObject pauseMenuOptions;             //Pause menu options game object
    public bool isMapOpen = false;                  //Is the map open?
    public float moveSpeed = 2f;                    //Speed the camera moves when
    public float minSize = 50f;
    public float maxSize = 200f;
    public float zoomAmount = 50f;


    private PlayerInput playerInput;                //Reference to the Player Input script
    private PauseMenu pause;                        //Reference to the Pause Menu script
    private Vector3 dragOrigin;                     //Origin of the mouse drag
    private Camera cam;                             //Reference to the camera component

    //Use this for initialization
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        //Check if the player exists in the scene
        if (player != null)
        {
            playerInput = player.GetComponent<PlayerInput>();
        }

        pause = pauseMenu.GetComponentInParent<PauseMenu>();
        cam = GetComponent<Camera>();
    }

    //Update is called once per frame
    void Update()
    {
        //If the map is not open
        if (!isMapOpen && !pause.paused)
        {
            //Check for player input
            if (playerInput != null)
            {
                //If a map button was pressed open the map and pause the game
                if (Input.GetKeyDown(playerInput.ctMap) || Input.GetKeyDown(playerInput.kbMap))
                {
                    pause.Pause();
                    mapMenu.SetActive(true);
                    pauseMenuOptions.SetActive(false);
                    pauseMenu.SetActive(true);
                    isMapOpen = true;
                }
            }
        }
        //The map is open
        else if (isMapOpen)
        {
            //Check for player input
            if (playerInput != null)
            {
                //If the map button was pressed close the map
                if (Input.GetKeyDown(playerInput.ctMap) || Input.GetKeyDown(playerInput.kbMap))
                {
                    pause.Resume();
                    mapMenu.SetActive(false);
                    pauseMenu.SetActive(false);
                    pauseMenuOptions.SetActive(true);
                    isMapOpen = false;
                }
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
            Vector2 move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            //Moves the camera
            transform.Translate(move);

            //Zooms the camera out
            if(Input.GetKeyDown(KeyCode.P) && cam.orthographicSize <= maxSize)
            {
                cam.orthographicSize += zoomAmount;
            }

            //Zooms the camera in
            if(Input.GetKeyDown(KeyCode.O) && cam.orthographicSize >= minSize)
            {
                cam.orthographicSize -= zoomAmount;
            }


            //DO NOT PUT CODE BELOW THIS FUNCTION
            //Moves the camera with mouse drag
            MouseMovement();
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
