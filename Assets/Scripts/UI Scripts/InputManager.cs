//Created by: Robert Bryant
//Based off of a tutorial by: quill18Creates
//https://www.youtube.com/watch?v=HkmP7raUYi0&t=1358s
//Manages the players input and allows it to be rebindable
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using XboxCtrlrInput;

public class InputManager : MonoBehaviour
{
    public Sprite[] gamepadButtons;                         //Sprites for the gamepad rebinding

    private Dictionary<string, KeyCode> kbInputs;           //Dictionary of key board input
    private Dictionary<string, object> gpInputs;            //Dictionary of gamepad inputs
    

	// Use this for initialization
	private void OnEnable ()
    {
        //Default the inputs for rebinding
        ResetInput(InputType.Gamepad);
        ResetInput(InputType.Keyboard);

        //Get the player's input data from JSON file
        PlayerInputData inputData = GameManager.Instance.LoadInputs();

        //Check for input data
        if (inputData != null)
        {
            //Set all the keyboard inputs from 
            for(int i = 0;i < inputData.inputNames.Count;i++)
            {
                SetKeyBoardButton(inputData.inputNames[i], inputData.kbInputs[i]);
            }

            //Set all gamepad button inputs
            for(int i = 0; i < inputData.gpButtonNames.Count; i++)
            {
                SetGamePadButton(inputData.gpButtonNames[i], inputData.gpButtons[i]);
            }

            //Set all gamepad axis inputs
            for(int i = 0; i < inputData.gpAxisNames.Count; i++)
            {
                SetGamePadButton(inputData.gpAxisNames[i], inputData.gpAxisInputs[i]);
            }

        }
        //Error
        else
        {
            //Save the current input data as a JSON file.
            Debug.LogWarning("PlayerInput.json was not found.");
            GameManager.Instance.SaveInputs();
        }
    }

    //Hard coded inputs for use as reset point.
    public void ResetInput(InputType type)
    {
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
        }

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
            gpInputs.Add("Ability 1", XboxButton.LeftBumper);
            gpInputs.Add("Ability 2", XboxAxis.LeftTrigger);
            gpInputs.Add("Ability 3", XboxButton.RightBumper);
            gpInputs.Add("Ability 4", XboxAxis.RightTrigger);
            gpInputs.Add("Open Map", XboxButton.Back);
            gpInputs.Add("Pause", XboxButton.Start);
        }
    }

    //Returns the gamepad inputs as an array of strings
    public string[] GetGamePadInputNames()
    {
        return gpInputs.Keys.ToArray();
    }

    //Returns the key board inputs as an array of strings
    public string[] GetKeyBoardInputNames()
    {
        return kbInputs.Keys.ToArray();
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
    public List<XboxAxis> SaveGamePadAxis()
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
        switch(xBoxAxis)
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

    //Overloaded Function
    //Gets the gamepad button based on button
    public Sprite GetGamePadButton(XboxButton xButton)
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
}
