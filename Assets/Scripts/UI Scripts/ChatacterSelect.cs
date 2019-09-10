//Created by Robert Bryant
//
//Selecting a character from a roster
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatacterSelect : MonoBehaviour
{
    public int sceneIndex;                  //The scene to be loaded

    //Use this for initialization
    private void Start()
    {        
    }

    //Selects the specified character
    public void SelectCharacter(Character selectedCharacter)
    {
        GameManager.Instance.Player = selectedCharacter;
        LevelLoader.Instance.LoadLevel(sceneIndex);
    }
}
