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
    private InputManager inputManager;              //Reference to the Input Manager Script
    private GamepadRebinding[] gpRebinds;           //Reference to the Game Pad Rebinding Script
    private int currentSceneIndex;                  //Index of the current game scene
    private string jsonInputPath;                   //String path to PlayerInput.json
    private string jsonInputData;                   //String of data from PlayerInput.json

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

        jsonInputPath = Application.streamingAssetsPath + "/PlayerInput.json";
    }

    //
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    //Used for initialization
    private void Start()
    {
        inputManager =FindObjectOfType<InputManager>(); 
    }

    //Saving the player's inputs
    public void SaveInputs()
    {
        if(inputManager == null)
        {
            inputManager = FindObjectOfType<InputManager>();
        }

        gpRebinds = Resources.FindObjectsOfTypeAll<GamepadRebinding>();
        float dZone, vibrate;

        //Check for the gamepad rebinding in the scene
        if(gpRebinds != null)
        {
            dZone = gpRebinds[0].GetDeadZone();
            vibrate = gpRebinds[0].GetVibration();
        }
        else
        {
            //Attempt to load deadzone and vibration strength from file
            PlayerInputData inputData = LoadInputs();

            dZone = inputData.deadZone;
            vibrate = inputData.vibrationStrength;
        }

        //If the save data doesn't exist create it
        if (!File.Exists(jsonInputPath))
        {
            //Error
            Debug.LogError(jsonInputPath + " was not found, Attempting to create one.");

            //Hopefully creates the PlayerInput.json
            File.Create(jsonInputPath);

            //Player input data
            PlayerInputData inputData = new PlayerInputData()
            {
                inputNames = inputManager.GetKeyBoardInputNames().ToList(),
                kbInputs = inputManager.SaveKeyboardInputs(),
                gpButtonNames = inputManager.SaveGamePadButtonNames(),
                gpButtons = inputManager.SaveGamePadButtons(),
                gpAxisNames = inputManager.SaveGamePadAxisNames(),
                gpAxisInputs = inputManager.SaveGamePadAxis(),
                deadZone = dZone,
                vibrationStrength = vibrate
            };

            //Convert the input data to json
            jsonInputData = JsonUtility.ToJson(inputData, true);

            //Write the data to json file
            File.WriteAllText(jsonInputPath, jsonInputData);

            Debug.Log("Saved json file");
        }
        else
        {
            //Create player input data
            PlayerInputData inputData = new PlayerInputData()
            {
                inputNames = inputManager.GetKeyBoardInputNames().ToList(),
                kbInputs = inputManager.SaveKeyboardInputs(),
                gpButtonNames = inputManager.SaveGamePadButtonNames(),
                gpButtons = inputManager.SaveGamePadButtons(),
                gpAxisNames = inputManager.SaveGamePadAxisNames(),
                gpAxisInputs = inputManager.SaveGamePadAxis(),
                deadZone = dZone,
                vibrationStrength = vibrate
            };

            //Convert the input datat to json
            jsonInputData = JsonUtility.ToJson(inputData, true);

            //Write the input data to json file
            File.WriteAllText(jsonInputPath, jsonInputData);

            Debug.Log("Saved json file");
        }
    }

    //Loading the player's inputs
    public PlayerInputData LoadInputs()
    {
        if (!File.Exists(jsonInputPath))
        {
            Debug.LogWarning("Failed to load Inputs from: " +jsonInputPath);
            return null;
        }
        else
        {
            jsonInputData = File.ReadAllText(jsonInputPath);
            PlayerInputData inputData = JsonUtility.FromJson<PlayerInputData>(jsonInputData);
            return inputData;
        }
    }

    //Set the keyboard inputs
    public void SetKeyBoardInputs(PlayerInputData inputData)
    {
       
        //Loop through and set each keyboard button
        for (int i = 0; i < inputData.inputNames.Count; i++)
        {
            inputManager.SetKeyBoardButton(inputData.inputNames[i], inputData.kbInputs[i]);
        }

    }

    //Set the gamepad inputs
    public void SetGamePadInputs(PlayerInputData inputData)
    {
        //Loop through and set each of the gamepad buttons
        for (int i = 0; i < inputData.gpButtonNames.Count; i++)
        {
            inputManager.SetGamePadButton(inputData.gpButtonNames[i], inputData.gpButtons[i]);
        }

        //Loop through and set each of the gamepad axis
        for (int i = 0; i < inputData.gpAxisNames.Count; i++)
        {
            inputManager.SetGamePadButton(inputData.gpAxisNames[i], inputData.gpAxisInputs[i]);
        }
    }

    //Called when a scene is changed
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Number of the current scene
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }
}

//Player's saved input data
[Serializable]
public class PlayerInputData
{
    public List<string> inputNames = new List<string>();
    public List<KeyCode> kbInputs = new List<KeyCode>();
    public List<string> gpButtonNames = new List<string>();
    public List<XboxButton> gpButtons = new List<XboxButton>();
    public List<string> gpAxisNames = new List<string>();
    public List<XboxAxis> gpAxisInputs = new List<XboxAxis>();
    public float deadZone;
    public float vibrationStrength;
}

