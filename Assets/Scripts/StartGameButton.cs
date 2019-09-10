//Created by Robert Bryant
//
//Handles the scene transition for starting the game
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartGameButton : MonoBehaviour
{
    public Character player;            //Chosen character for the game
    public int sceneIndex;              //Scene index to load

    private Button playButton;          //Reference to the play game button

    //Starts the game
    private void StartGame()
    {
        //Set the player and load the specified scene
        GameManager.Instance.Player = player;
        LevelLoader.Instance.LoadLevel(sceneIndex);        
    }
    
    //When the object is active
    private void OnEnable()
    {
        //Get the button component and wait for it to be clicked
        playButton = transform.GetComponent<Button>();
        playButton.onClick.AddListener(StartGame);
    }
}
