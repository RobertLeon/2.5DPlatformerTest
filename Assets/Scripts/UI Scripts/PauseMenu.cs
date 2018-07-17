//Created by Robert Bryant
//Based off of a tutorial by: Brackeys
//Youtube: https://www.youtube.com/watch?v=JivuXdrIHK0
//Handles the pause menu in the game
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPaused = false;        //Check for the game being paused
    public GameObject pauseMenu;                    //Reference to the pause menu game object
    public GameObject confirmationMenu;             //Reference to the confirmation screen
    public TMP_Text confirmationText;               //Text to display in the confirmation menu
    public Button yesButton;                        //Reference to the confirmation button
    public GameObject miniMap;                      //Reference to the mini map game object
    public GameObject pauseMenuOptions;             //Reference to the pauseMenuOptions game object
    public Button resumeButton;                     //Reference to the resume button
    [HideInInspector]
    public bool paused = false;                     //Is the game paused?

    private LevelLoader levelLoader;                //Reference to the Loading Screen
    private PlayerInput playerInput;                //Reference to the PlayerInput script
    private CollisionController collision;          //Reference to the CollisionController script
    private GameObject currentMenu;                 //Current menu game object

    public void InitializePauseMenu()
    {
        //Find the player game object
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        levelLoader = FindObjectOfType<LevelLoader>();
        currentMenu = pauseMenu;    

        //If the player exists get the PlayerInput and CollisionController components
        if (player != null)
        {
            playerInput = player.GetComponent<PlayerInput>();
            collision = player.GetComponent<CollisionController>();
        }
    }

    //Update is called once per frame
    void Update()
	{

        //Check for specified input
		if(playerInput != null)
        {
            if (Input.GetKeyDown(playerInput.ctPause) || Input.GetKeyDown(playerInput.kbPause))
            {
                //If the game is paused resume play otherwise pause the game
                if (gameIsPaused)
                {
                    Resume();
                }
                else
                {
                    Pause();
                }
            }
        }
        //If there is no player input script use hardcoded values
        else
        {
            if(Input.GetKeyDown(KeyCode.Escape)||Input.GetKeyDown(KeyCode.Joystick1Button7))
            {
                //If the game is paused resume play otherwise pause the game
                if (gameIsPaused)
                {
                    Resume();
                }
                else
                {
                    Pause();
                }
            }
        }
	}

    //Resume gameplay
    public void Resume()
    {
        //Hides the pause menu
        pauseMenu.SetActive(false);

        //Shows the minimap
        miniMap.SetActive(true);

        //Enable the player's CollisionController script
        if (collision != null)
        {
            collision.enabled = true;
            playerInput.enabled = true;
        }
            
        //Set the game's time scale back to full
        Time.timeScale = 1f;

        //Game is no longer paused
        gameIsPaused = false;
        paused = gameIsPaused;
    }

    //Pause gameplay
    public void Pause()
    {
        //Show the pause menu
        pauseMenu.SetActive(true);

        //Hide the mini map
        miniMap.SetActive(false);

        //Check if the current menu is not the pause menu
        if (currentMenu != pauseMenu)
        {
            pauseMenuOptions.SetActive(true);
            currentMenu.SetActive(false);
        }

        //Selects the resume button for controller input
        resumeButton.Select();

        //Disable the player's CollisionController
        if (collision != null)
        {
            collision.enabled = false;
            playerInput.enabled = false;
        }
        //Set the game's time scale to 0.
        Time.timeScale = 0f;

        //Game is paused
        gameIsPaused = true;
        paused = gameIsPaused;
    }

   
    //Quit to main menu
    public void LoadMainMenu(string confirmationQuestion)
    {
        //Show the confirmation menu
        confirmationMenu.SetActive(true);

        //Assign the confirmation question to be shown on screen
        confirmationText.text = confirmationQuestion;

        //If the yes button is clicked load scene
        yesButton.onClick.AddListener(LoadScene);
    }

    //Exit game completely
    public void QuitGame(string confirmationQuestion)
    {
        //Show the confirmation menu
        confirmationMenu.SetActive(true);

        //Assign the confirmation question to be shown on screen
        confirmationText.text = confirmationQuestion;

        //If the yes button is clicked exit the game.
        yesButton.onClick.AddListener(ExitGame);
    }

    //Load the main menu
    void LoadScene()
    {
        //Unpause the game
        Resume();

        //Check for the level loader and load the main menu
        if (levelLoader != null)
        {
            levelLoader.LoadLevel(0);
        }
        //Find the level loader and load the main menu
        else
        {
            levelLoader = FindObjectOfType<LevelLoader>();
            levelLoader.LoadLevel(0);
        }
    }

    //Exit the game application
    void ExitGame()
    {
        Debug.Log("Exited Game");
        Application.Quit();
    }

    //Sets the current menu when using the buttons in the pause menu
    public void SetCurrentMenu(GameObject menu)
    {
        currentMenu = menu;
    }
}