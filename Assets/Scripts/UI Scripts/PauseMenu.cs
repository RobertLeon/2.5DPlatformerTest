using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPaused = false;
    public GameObject pauseMenu;
    public GameObject confirmationMenu;
    public TMP_Text confirmationText;
    public Button yesButton;

    private PlayerInput playerInput;
    private CollisionController collision;

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerInput = player.GetComponent<PlayerInput>();
        collision = player.GetComponent<CollisionController>();
    }

    //Update is called once per frame
    void Update()
	{
		if(Input.GetKeyDown(playerInput.kbPause) || Input.GetKeyDown(playerInput.ctPause))
        {
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

    public void Resume()
    {
        pauseMenu.SetActive(false);
        collision.enabled = true;
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    private void Pause()
    {
        pauseMenu.SetActive(true);
        collision.enabled = false;
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    public void LoadMainMenu(string confirmationQuestion)
    {
        confirmationMenu.SetActive(true);
        confirmationText.text = confirmationQuestion;

        yesButton.onClick.AddListener(LoadScene);
    }

    public void QuitGame(string confirmationQuestion)
    {
        confirmationMenu.SetActive(true);
        confirmationText.text = confirmationQuestion;

        yesButton.onClick.AddListener(ExitGame);
    }

    void LoadScene()
    {
        Debug.Log("Load Main Menu");
    }

    void ExitGame()
    {
        Debug.Log("Exited Game");
    }
}