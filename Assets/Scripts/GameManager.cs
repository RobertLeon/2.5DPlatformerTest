//Created by Robert Bryant
//
//Game Manager
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using XboxCtrlrInput;



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
    private string binarySaveFile;                  //

    //
    private void Awake()
    {
        //Singleton
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
  
    //Used for initialization
    private void Start()
    {
    }

}

[System.Serializable]
public class PlayerData
{
    public string PlayerName;
    public float CompletionPercent;
    public float TimePlayed;
    public int Scene;
    public int SaveRoom;

}

