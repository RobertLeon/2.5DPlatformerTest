//Created by Robert Bryant
//
//Game Manager
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;



public class GameManager : MonoBehaviour
{
    #region Singleton
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if(_instance == null)
            {
                GameObject gameManager = new GameObject("GameManager");
                gameManager.AddComponent<GameManager>();
            }
            return _instance;
        }
    }
    #endregion

    public Character Player { get; set; }           //The chosen player for the game

    //
    private void Awake()
    {
        //
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    //Used for initialization
    private void Start()
    {
        //If there is no audio playing play the title music
        if(!GetComponent<AudioSource>().isPlaying)
        {
            FindObjectOfType<AudioController>().Play("TitleMusic");
        }
    }

    //Saving the game's progress
    public void SaveGame()
    {
        //If the save data doesn't exist create it
        if (!File.Exists(Application.persistentDataPath + "/SaveData.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/SaveData.dat");

            PlayerData data = new PlayerData();

            data.unlockedItems = new List<Items>();
            data.unlockedCharacters = new List<Character>();

            bf.Serialize(file, data);
            file.Close();
        }
    }

    //Loading a saved game
    public void LoadGame()
    {

    }

    //Called when a scene is changed
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Number of the current scene
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        //Play title music 
        if(currentSceneIndex == 0)
        {
            FindObjectOfType<AudioController>().Play("TitleMusic");
        }
    }
}

//Player's saved data
[Serializable]
class PlayerData
{
    public List<Items> unlockedItems;                   //List of unlocked items
    public List<Character> unlockedCharacters;          //List of unlocked characters
    
}

