//Created by Robert Bryant
//Based off of a tutorial by: Brackeys
//Youtube: https://www.youtube.com/watch?v=JivuXdrIHK0
//Handles the pause menu in the game
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPaused = false;        //Check for the game being paused
    public GameObject pauseMenu;                    //Reference to the pause menu game object
    public GameObject confirmationMenu;             //Reference to the confirmation screen
    public TMP_Text confirmationText;               //Text to display in the confirmation menu
    public Button yesButton;                        //Reference to the confirmation button

    private PlayerInput playerInput;                //Reference to the PlayerInput script
    private CollisionController collision;          //Reference to the CollisionController script

    private void Start()
    {
        //Find the player game object
        GameObject player = GameObject.FindGameObjectWithTag("Player");

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
		if(Input.GetKeyDown(KeyCode.Escape))
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

    //Resume gameplay
    public void Resume()
    {
        //Hides the pause menu
        pauseMenu.SetActive(false);

        //Enable the player's CollisionController script
        collision.enabled = true;

        //Set the game's time scale back to full
        Time.timeScale = 1f;

        //Game is no longer paused
        gameIsPaused = false;
    }

    //Pause gameplay
    private void Pause()
    {
        //Show the pause menu
        pauseMenu.SetActive(true);

        //Disable the player's CollisionController
        collision.enabled = false;

        //Set the game's time scale to 0.
        Time.timeScale = 0f;

        //Game is paused
        gameIsPaused = true;
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
        Debug.Log("Load Main Menu");
    }

    //Exit the game application
    void ExitGame()
    {
        Debug.Log("Exited Game");
        Application.Quit();
    }
}