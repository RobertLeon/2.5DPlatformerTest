//Created by Robert Bryant
//
//Handles the scene transition for starting the game
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartGameButton : MonoBehaviour
{
    public LevelLoader levelLoader;
    public Character player;
    public int sceneIndex;              //Scene index to load

    private Button playButton;          //Reference to the play game button

    //Calls the Level Loader script on the Game Manager game object
    private void StartGame()
    {
        GameManager.Instance.Player = player;
        levelLoader.LoadLevel(sceneIndex);        
    }
    
    //When the onject is active
    private void OnEnable()
    {
        //Get the button component and wait for it to be clicked
        playButton = transform.GetComponent<Button>();
        playButton.onClick.AddListener(StartGame);
    }
}
