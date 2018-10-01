//Created by: Robert Bryant
//Based off of a tutorial by: quill18Creates
//https://www.youtube.com/watch?v=HkmP7raUYi0&t=1358s
//Manages the players input and allows it to be rebindable
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public Sprite[] gamepadButtons;                      //Sprites for the gamepad rebinding

    private Dictionary<string, KeyCode> kbInputs;       //Dictionary of key board input
    private Dictionary<string, KeyCode> gpInputs;       //Dictionary of gamepad inputs

	// Use this for initialization
	private void OnEnable ()
    {
        //Assign new dictionaries for the Inputs
        kbInputs = new Dictionary<string, KeyCode>();
        gpInputs = new Dictionary<string, KeyCode>();
        
        //Need to put these into player preferences and read them from there.
        kbInputs.Add("Move Left", KeyCode.A);        
        kbInputs.Add("Move Right",KeyCode.D);
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

        //Also need to figue out how to store axis input
        gpInputs.Add("Jump", KeyCode.JoystickButton0);
        gpInputs.Add("Interact", KeyCode.JoystickButton1);
        gpInputs.Add("Use Item", KeyCode.JoystickButton2);
        gpInputs.Add("Swap Item", KeyCode.JoystickButton3);
        gpInputs.Add("Ability 1", KeyCode.JoystickButton4);
        gpInputs.Add("Ability 3", KeyCode.JoystickButton5);
        gpInputs.Add("Open Map", KeyCode.JoystickButton6);
        gpInputs.Add("Pause", KeyCode.JoystickButton7);
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
    public void SetGamePadButton(string button, KeyCode key)
    {
        gpInputs[button] = key;
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

        return kbInputs[button].ToString(); ;
    }

    //Gets the gamepad button based on key code
    public Sprite GetGamePadButton(KeyCode key)
    {
        //Returns a sprite based on key code
        switch (key)
        {
            case KeyCode.JoystickButton0:
                return gamepadButtons[0];

            case KeyCode.JoystickButton1:
                return gamepadButtons[1];

            case KeyCode.JoystickButton2:
                return gamepadButtons[2];

            case KeyCode.JoystickButton3:
                return gamepadButtons[3];

            case KeyCode.JoystickButton4:
                return gamepadButtons[4];

            case KeyCode.JoystickButton5:
                return gamepadButtons[5];

            case KeyCode.JoystickButton6:
                return gamepadButtons[6];

            case KeyCode.JoystickButton7:
                return gamepadButtons[7];
            
            //Error
            default:
                Debug.LogError("Key code given does not have a image associated with it.");
                return null;
        }
    }

    //Overloaded GetGamePadButton
    //Used for intializing the rebind menu
    public Sprite GetGamePadButton(string button)
    {
        KeyCode key = gpInputs[button];

        return GetGamePadButton(key);        
    }

    //Checks for input from the keyboard
    public bool GetKeyDown(string button)
    {
        //Return the specified input
        if(kbInputs.ContainsKey(button) && !gpInputs.ContainsKey(button))
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

    //Checks for input from the game pad
    public bool GetButtonDown(string button)
    {
        if(gpInputs.ContainsKey(button) && !kbInputs.ContainsKey(button))
        {
            return Input.GetKeyDown(gpInputs[button]);
        }
        else
        {
            Debug.LogError("No button named: " + button + " was found.");
            return false;
        }
    }
}
