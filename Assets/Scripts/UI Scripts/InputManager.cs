//Created by: Robert Bryant
//Based off of a tutorial by: quill18Creates
//https://www.youtube.com/watch?v=HkmP7raUYi0&t=1358s
//Manages the players input and allows it to be rebindable
using System.IO;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using XboxCtrlrInput;

public class InputManager : MonoBehaviour
{
    private Dictionary<string, KeyCode> kbInputs;           //Dictionary of key board input
    private Dictionary<string, object> gpInputs;            //Dictionary of gamepad inputs
    private List<Sprite> gamepadButtons;                    //Sprites for the gamepad rebinding
    private float deadZone;                                 //
    private float vibration;                                //
    private string jsonPath;                                //File path for the player's inputs

    private void Awake()
    {
        kbInputs = new Dictionary<string, KeyCode>();
        gpInputs = new Dictionary<string, object>();

        //Load in the sprites for the xbox gamepad from resources
        gamepadButtons = new List<Sprite>()
        {
            Resources.Load<Sprite>("XboxInput/DpadDown"),
            Resources.Load<Sprite>("XboxInput/DpadLeft"),
            Resources.Load<Sprite>("XboxInput/DpadRight"),
            Resources.Load<Sprite>("XboxInput/DpadUp"),
            Resources.Load<Sprite>("XboxInput/ButtonA"),
            Resources.Load<Sprite>("XboxInput/ButtonB"),
            Resources.Load<Sprite>("XboxInput/ButtonX"),
            Resources.Load<Sprite>("XboxInput/ButtonY"),
            Resources.Load<Sprite>("XboxInput/BumperLeft"),
            Resources.Load<Sprite>("XboxInput/BumperRight"),
            Resources.Load<Sprite>("XboxInput/Start"),
            Resources.Load<Sprite>("XboxInput/Back"),
            Resources.Load<Sprite>("XboxInput/StickLeft"),
            Resources.Load<Sprite>("XboxInput/StickRight"),
            Resources.Load<Sprite>("XboxInput/TriggerLeft"),
            Resources.Load<Sprite>("XboxInput/TriggerRight")
        };

        jsonPath = Application.streamingAssetsPath + "/PlayerInputs.json";

        PlayerInputData inputData = LoadInputs();

        if (inputData != null)
        {
            if (inputData.KeyBoardInputNames.Count != 0)
            {
                for (int i = 0; i < inputData.KeyBoardInputNames.Count; i++)
                {
                    SetKeyBoardButton(inputData.KeyBoardInputNames[i], inputData.KeyBoardInputs[i]);
                }
            }
            else
            {
                Debug.LogWarning("No Keyboard Inputs found in file. Reseting inputs.");
                ResetInput(InputType.Keyboard);
            }

            if (inputData.GamePadButtonNames.Count != 0 && inputData.GamePadAxisNames.Count != 0)
            {

                for (int i = 0; i < inputData.GamePadButtonNames.Count; i++)
                {
                    SetGamePadButton(inputData.GamePadButtonNames[i], inputData.GamePadButtons[i]);
                }

                for (int i = 0; i < inputData.GamePadAxisNames.Count; i++)
                {
                    SetGamePadButton(inputData.GamePadAxisNames[i], inputData.GamePadAxes[i]);
                }
            }
            else
            {
                Debug.LogWarning("No XBox Inputs found in file. Reseting inputs.");
                ResetInput(InputType.Gamepad);
            }
        }       
        else
        {
            //Default the inputs for rebinding
            ResetInput(InputType.Gamepad);
            ResetInput(InputType.Keyboard);
        }               
    }   

    //Hard coded inputs for use as reset point.
    public void ResetInput(InputType type)
    {
        //Reset the keyboard inputs
        if(type == InputType.Keyboard)
        {
            kbInputs = new Dictionary<string, KeyCode>();

            //Keyboard inputs
            kbInputs.Add("Move Left", KeyCode.A);
            kbInputs.Add("Move Right", KeyCode.D);
            kbInputs.Add("Move Up", KeyCode.W);
            kbInputs.Add("Move Down", KeyCode.S);
            kbInputs.Add("Jump", KeyCode.Space);
            kbInputs.Add("Interact", KeyCode.E);
            kbInputs.Add("Swap Item", KeyCode.Q);
            kbInputs.Add("Ability 1", KeyCode.Keypad1);
            kbInputs.Add("Ability 2", KeyCode.Keypad2);
            kbInputs.Add("Ability 3", KeyCode.Keypad3);
            kbInputs.Add("Ability 4", KeyCode.Keypad0);
            kbInputs.Add("Use Item", KeyCode.KeypadEnter);
            kbInputs.Add("Open Map", KeyCode.M);
            kbInputs.Add("Pause", KeyCode.Backspace);
            kbInputs.Add("Zoom In", KeyCode.Equals);
            kbInputs.Add("Zoom Out", KeyCode.Minus);            
        }

        //Reset gamepad inputs
        if (type == InputType.Gamepad)
        {
            gpInputs = new Dictionary<string, object>();

            //Gamepad Inputs
            gpInputs.Add("Move Left", XboxButton.DPadLeft);
            gpInputs.Add("Move Right", XboxButton.DPadRight);
            gpInputs.Add("Move Up", XboxButton.DPadUp);
            gpInputs.Add("Move Down", XboxButton.DPadDown);
            gpInputs.Add("Jump", XboxButton.A);
            gpInputs.Add("Interact", XboxButton.B);
            gpInputs.Add("Use Item", XboxButton.X);
            gpInputs.Add("Swap Item", XboxButton.Y);
            gpInputs.Add("Ability 1", XboxButton.RightBumper);
            gpInputs.Add("Ability 2", XboxButton.LeftBumper);
            gpInputs.Add("Ability 3", XboxAxis.RightTrigger);
            gpInputs.Add("Ability 4", XboxAxis.LeftTrigger);
            gpInputs.Add("Open Map", XboxButton.Back);
            gpInputs.Add("Pause", XboxButton.Start);
            gpInputs.Add("Zoom In", XboxButton.LeftStick);
            gpInputs.Add("Zoom Out", XboxButton.RightStick);
            vibration = 1;
            deadZone = 0;            
        }

        SaveInputs(true);
        
    }

    //Returns the gamepad inputs as an array of strings
    public string[] GetGamePadInputNames()
    {
        if(File.Exists(jsonPath))
        {
            PlayerInputData gamepadData = LoadInputs();

            for(int i = 0; i < gamepadData.GamePadButtonNames.Count; i++)
            {
                gpInputs[gamepadData.GamePadButtonNames[i]] = gamepadData.GamePadButtons[i]; 
            }

            for(int i = 0;i <gamepadData.GamePadAxisNames.Count;i++)
            {
                gpInputs[gamepadData.GamePadAxisNames[i]] = gamepadData.GamePadAxes[i];
            }
        }

        return gpInputs.Keys.ToArray();
    }

    //Returns the key board inputs as an array of strings
    public string[] GetKeyBoardInputNames()
    {
        if (kbInputs.Keys.Count() != 0)
        {
            return kbInputs.Keys.ToArray();
        }
        else
        {
            return null;
        }
    }

    //Sets the key board input based on the given parameters
    public void SetKeyBoardButton(string button, KeyCode key)
    {
        kbInputs[button] = key;
    }

    //Sets the gamepad input based on the given parameters
    public void SetGamePadButton(string button, XboxButton xBoxButton)
    {
        gpInputs[button] = xBoxButton;
    }

    //Sets the gamepad axis as input
    public void SetGamePadButton(string button, XboxAxis xBoxAxis)
    {
        gpInputs[button] = xBoxAxis;
    }
    
    //Returns a list of gamepad button names for saving inputs
    public List<string> SaveGamePadButtonNames()
    {
        List<string> s = new List<string>();

        //Loop through each gamepad input
        foreach(string name in gpInputs.Keys)
        {
            //Add the input name if the input is an Xbox Button
            if(gpInputs[name].GetType() == typeof(XboxButton))
            {
                s.Add(name);
            }
        }

        return s;
    }

    //Returns a list of gamepad axis for saving inputs
    public List<string> SaveGamePadAxisNames()
    {
        List<string> s = new List<string>();

        //Loop through each gamepad input
        foreach(string name in gpInputs.Keys)
        {
            //Add the input name if the input is an Xbox Axis
            if(gpInputs[name].GetType()==typeof(XboxAxis))
            {
                s.Add(name);
            }
        }

        return s;
    }

    //Returns a list of Xbox Buttons used for inputs
    public List<XboxButton> SaveGamePadButtons()
    {
        List<XboxButton> btns = new List<XboxButton>();
        
        //Loop through each input to check for the input type
        foreach(object input in gpInputs.Values)
        {
            //If the input is an XboxButton add it to the list
            if(input.GetType() == typeof(XboxButton))
            {
                btns.Add((XboxButton)input);
            }
        }

        return btns;
    }

    //Returns a list of XboxAxis used for input
    public List<XboxAxis> SaveGamePadAxes()
    {
        List<XboxAxis> btns = new List<XboxAxis>();

        //Loop through each input to check for the input type
        foreach(object input in gpInputs.Values)
        {
            //If the input is an XboxAxis add it to the list
            if(input.GetType() == typeof(XboxAxis))
            {
                btns.Add((XboxAxis)input);
            }
        }

        return btns;
    }

    //Returns the keyboard inputs
    public List<KeyCode> SaveKeyboardInputs()
    {
        return kbInputs.Values.ToList();
    }
    
    //Get the keyboard input based on the button name
    public string GetKeyBoardButtonName(string button)
    {
        //Return an error if the button name does not exist
        if(!kbInputs.ContainsKey(button))
        {
            Debug.LogError("No button named: " + button + " was found.");
            return "N/A";
        }

        return kbInputs[button].ToString();
    }

    //Overloaded Function
    //Gets the gamepad sprite for triggers
    public Sprite GetGamePadButton(XboxAxis xBoxAxis)
    {
        if (gamepadButtons.Count != 0)
        {
            switch (xBoxAxis)
            {
                case XboxAxis.LeftTrigger:
                    return gamepadButtons[14];

                case XboxAxis.RightTrigger:
                    return gamepadButtons[15];

                //Error
                default:
                    Debug.LogError("Key code given does not have a image associated with it.");
                    return null;
            }
        }
        else
        {
            return null;
        }
    }

    //Overloaded Function
    //Gets the gamepad button based on button
    public Sprite GetGamePadButton(XboxButton xButton)
    {

        if (gamepadButtons.Count != 0)
        {
            //Returns a sprite based on key code
            switch (xButton)
            {
                case XboxButton.DPadDown:
                    return gamepadButtons[0];

                case XboxButton.DPadLeft:
                    return gamepadButtons[1];

                case XboxButton.DPadRight:
                    return gamepadButtons[2];

                case XboxButton.DPadUp:
                    return gamepadButtons[3];

                case XboxButton.A:
                    return gamepadButtons[4];

                case XboxButton.B:
                    return gamepadButtons[5];

                case XboxButton.X:
                    return gamepadButtons[6];

                case XboxButton.Y:
                    return gamepadButtons[7];

                case XboxButton.LeftBumper:
                    return gamepadButtons[8];

                case XboxButton.RightBumper:
                    return gamepadButtons[9];

                case XboxButton.Start:
                    return gamepadButtons[10];

                case XboxButton.Back:
                    return gamepadButtons[11];

                case XboxButton.LeftStick:
                    return gamepadButtons[12];

                case XboxButton.RightStick:
                    return gamepadButtons[13];

                //Error
                default:
                    Debug.LogError("Key code given does not have a image associated with it.");
                    return null;
            }
        }
        else
        {
            return null;
        }
    }

    //Overloaded Function
    //Used for intializing the rebind menu
    public Sprite GetGamePadButton(string button)
    {
        object gpButton = gpInputs[button];

        //Check if the input is XboxButton
        if(gpButton.GetType() == typeof(XboxButton))
        {
            return GetGamePadButton((XboxButton)gpButton);
        }
        //Check if the input is an XboxAxis
        else if(gpButton.GetType() == typeof(XboxAxis))
        {
            return GetGamePadButton((XboxAxis)gpButton);
        }        
        //Display an error
        else
        {
            Debug.LogError("Input type not supported");
            return null;
        }               
    }

    //Checks for a keyboard input being pressed down
    public bool GetKeyDown(string button)
    {
        //Return the specified input
        if (kbInputs.ContainsKey(button))
        {
            return Input.GetKeyDown(kbInputs[button]);
        }
        //Display an error
        else
        {
            Debug.LogError("No button named: " + button + " was found.");
            return false;
        }
    }

    //Check if a keyboard input is being pressed
    public bool GetKey(string button)
    {
        //Return the specified input
        if(kbInputs.ContainsKey(button))
        {
            return Input.GetKey(kbInputs[button]);
        }
        //Display an error
        else
        {
            Debug.LogError("No button named: " + button + " was found.");
            return false;
        }
    }

    //Checks for keyboard input being released
    public bool GetKeyUp(string button)
    {
        if(kbInputs.ContainsKey(button))
        {
            return Input.GetKeyUp(kbInputs[button]);            
        }
        else
        {
            Debug.LogError("No button named: " + button + " was found");
            return false;
        }
    }

    //Checks for gamepad input is being pressed
    public bool GetButton(string button)
    {
        if (gpInputs.ContainsKey(button))
        {
            if (gpInputs[button].GetType() == typeof(XboxButton))
            {
                return XCI.GetButton((XboxButton)gpInputs[button]);
            }
            else if (gpInputs[button].GetType() == typeof(XboxAxis))
            {
                if (XCI.GetAxis((XboxAxis)gpInputs[button]) >= 0.5f)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                Debug.LogError("No button named: " + button + " was found.");
                return false;
            }
        }
        else
        {
            Debug.LogError("No button named: " + button + " was found.");
            return false;
        }
    }

    //Checks for gamepad button being pressed down
    public bool GetButtonDown(string button)
    {
        if(gpInputs.ContainsKey(button))
        {
            if (gpInputs[button].GetType() == typeof(XboxButton))
            {
                return XCI.GetButtonDown((XboxButton)gpInputs[button]);
            }
            else if (gpInputs[button].GetType() == typeof(XboxAxis))
            {
                if (XCI.GetAxis((XboxAxis)gpInputs[button]) >= 0.5f)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                Debug.LogError("No button named: " + button + " was found.");
                return false;
            }
        }
        else
        {
            Debug.LogError("No button named: " + button + " was found.");
            return false;
        }
    }

    //Checks for gamepad button being released
    public bool GetButtonUp(string button)
    {
        //Check if the button is in the list of inputs
        if (gpInputs.ContainsKey(button))
        {
            //Check for Xbox Button inputs
            if (gpInputs[button].GetType() == typeof(XboxButton))
            {
                return XCI.GetButtonUp((XboxButton)gpInputs[button]);
            }
            //Check for Xbox Axis inputs
            else if (gpInputs[button].GetType() == typeof(XboxAxis))
            {
                if (XCI.GetAxis((XboxAxis)gpInputs[button]) < 0.5f)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                Debug.LogError("No button named: " + button + " was found.");
                return false;
            }
        }
        else
        {
            Debug.LogError("No button named: " + button + " was found.");
            return false;
        }
    }

    //Loading inputs from JSON file
    public PlayerInputData LoadInputs()
    {
        if (File.Exists(jsonPath))
        {
            string loadedInputs = File.ReadAllText(jsonPath);

            PlayerInputData loaded = JsonUtility.FromJson<PlayerInputData>(loadedInputs);

            if (loaded == null)
            {
               
                SaveInputs();
                return null;
            }
            else
            {
                return loaded;
            }
        }
        else
        {
            return null;
        }
    }


    //Saving inputs to JSON
    public void SaveInputs(bool resetInput = false)
    {
        KeyRebinding rebinds = FindObjectOfType<KeyRebinding>();

        if (rebinds != null && !resetInput)
        {
            deadZone = rebinds.GetGamepadDeadzone();
            vibration = rebinds.GetGamepadVibration();
        }

        PlayerInputData inputData = new PlayerInputData
        {
            KeyBoardInputNames = GetKeyBoardInputNames().ToList(),
            KeyBoardInputs = SaveKeyboardInputs(),
            GamePadButtonNames = SaveGamePadButtonNames(),
            GamePadButtons = SaveGamePadButtons(),
            GamePadAxisNames = SaveGamePadAxisNames(),
            GamePadAxes = SaveGamePadAxes(),
            GamePadDeadZone = deadZone,
            GamePadVibration = vibration
        };

        string jsonData = JsonUtility.ToJson(inputData, true);

        if (!File.Exists(jsonPath))
        {
            Debug.LogError("No file found at: " + jsonPath);
            File.WriteAllText(jsonPath, jsonData);
        }
        else
        {
            File.WriteAllText(jsonPath, jsonData);
        }

    }

    public float GetVibration()
    {
        return LoadInputs().GamePadVibration;
    }

    public float GetDeadZone()
    {
        return LoadInputs().GamePadDeadZone;
    }
}

[System.Serializable]
//Player input data
public class PlayerInputData
{
    public List<string> KeyBoardInputNames;
    public List<KeyCode> KeyBoardInputs;
    public List<string> GamePadButtonNames;
    public List<XboxButton> GamePadButtons;
    public List<string> GamePadAxisNames;
    public List<XboxAxis> GamePadAxes;
    public float GamePadDeadZone;
    public float GamePadVibration;
}

