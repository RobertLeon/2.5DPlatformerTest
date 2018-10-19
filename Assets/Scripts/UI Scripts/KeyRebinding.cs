//Created by: Robert Bryant
//Based off of a tutorial by: quill18Creates
//https://www.youtube.com/watch?v=HkmP7raUYi0&t=1358s
//Allows for the rebinding of keys in a menu
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using XboxCtrlrInput;

public class KeyRebinding : MonoBehaviour {

    public GameObject keyBindingPrefab;                     //Prefab that populates the input menu
    public Button inputMenuButton;                          //Reference to the UI button for menu navigation
    public InputType inputType;                             //Type of input to display
   

    private string rebindButton = null;                     //String to check which button is being rebinded
    private InputManager inputManager;                      //Reference to the Input Manager Script
    private Dictionary<string, TMP_Text> buttonToLabel;     //Dictionary of keyboard inputs to change
    private Dictionary<string, Image> buttonToImage;        //Dictionary of gamepad inputs to change
    private List<Button> buttons;

    // Use this for initialization
    public virtual void Start()
    {
        //Get the inputManager from the scene
        inputManager = FindObjectOfType<InputManager>();

        //Initialize the menu depending on input type
        switch (inputType)
        {
            case InputType.Keyboard:
                InitializeKeyBoardInputs();
                break;

            case InputType.Gamepad:
                InitializeGamePadInputs();
                break;

            //Error
            default:
                Debug.LogError("Input type not specified or accounted for.");
                break;
        }
    }

    //When the script is enabled
    private void OnEnable()
    {
        //Load the keyboard bindings from file
        GameManager.Instance.SetKeyBoardInputs(GameManager.Instance.LoadInputs());
    }

    //Initializes the input menu for gamepad inputs
    private void InitializeGamePadInputs()
    {
        buttonToImage = new Dictionary<string, Image>();

        //Create a button for each input
        foreach (string button in inputManager.GetGamePadInputNames())
        {
            string btnName;
            btnName = button;

            //Create a button in the menu for a key bind
            GameObject bind = Instantiate(keyBindingPrefab, transform);
            bind.transform.localScale = Vector3.one;

            //Get the label, button, and button text objects
            TMP_Text bindLabel = bind.GetComponentInChildren<TMP_Text>();
            Button bindButton = bind.GetComponentInChildren<Button>();
            Image bindBtnImage = bindButton.transform.GetChild(0).GetComponent<Image>();

            //Set the text of the label
            bindLabel.text = btnName;

            //Set the text of the button
            bindBtnImage.sprite = inputManager.GetGamePadButton(btnName);
            
            //Add the button to the dictionary
            buttonToImage.Add(btnName, bindBtnImage);

            //Add a listener for the button
            bindButton.onClick.AddListener(delegate { RebindKey(btnName); });            
        }
    }

    //Initializes the input menu for keyboard inputs
    private void InitializeKeyBoardInputs()
    {
        //Create a new dictionary of buttons
        buttonToLabel = new Dictionary<string, TMP_Text>();

        //Create a button for each input
        foreach (string button in inputManager.GetKeyBoardInputNames())
        {
            string btnName;
            btnName = button;

            //Create a button in the menu for a key bind
            GameObject bind = Instantiate(keyBindingPrefab, transform);
            bind.transform.localScale = Vector3.one;

            //Get the label, button, and button text objects
            TMP_Text bindLabel = bind.GetComponentInChildren<TMP_Text>();
            Button bindButton = bind.GetComponentInChildren<Button>();
            TMP_Text bindBtnText = bindButton.GetComponentInChildren<TMP_Text>();

            //Set the text of the label
            bindLabel.text = btnName;

            //Set the text of the button
            bindBtnText.text = inputManager.GetKeyBoardButtonName(btnName);

            //Add the button to the dictionary
            buttonToLabel.Add(btnName, bindBtnText);

            //Add a listener for the button
            bindButton.onClick.AddListener(() => { RebindKey(btnName); });
        }

        //Create a new navigation and set the select on up to the last rebind button
        Navigation nav = new Navigation();
        nav.mode = Navigation.Mode.Explicit;
        nav.selectOnUp = buttonToLabel.LastOrDefault().Value.transform.GetComponentInParent<Button>();

        //Set the navigation on the inputMenu Button
        inputMenuButton.navigation = nav;
    }

    //Selects the first button in the scroll list
    public void SelectFirstButton()
    {
        StartCoroutine(SelectButton());
    }

    //Selects the key to be rebind
    private void RebindKey(string button)
    {
        rebindButton = button;
    }

    //Selects the first button in the scroll list
    private IEnumerator SelectButton()
    {
        //Wait a small amount of time to access the button
        yield return new WaitForSeconds(0.1f);

        //Selects the first button created for menu movement
        switch (inputType)
        {
            case InputType.Gamepad:
                buttonToImage.FirstOrDefault().Value.transform.GetComponentInParent<Button>().Select();
                break;

            case InputType.Keyboard:
                buttonToLabel.FirstOrDefault().Value.transform.GetComponentInParent<Button>().Select();
                break;

            default:
                Debug.LogError("");
                break;
        }
    }

    //Rebinds the keyboard  input 
    private IEnumerator RebindInput()
    {
        //Wait a small amount of time to not bind the confirmation input
        yield return new WaitForSeconds(0.1f);

        //Rebind keyboard input
        if (inputType == InputType.Keyboard)
        {
            //List of gamepad inputs
            List<KeyCode> gamepadInput = new List<KeyCode>();

            //Loop through each keycode 
            foreach (KeyCode key in Enum.GetValues(typeof(KeyCode)))
            {
                //Check for a joystick input and add it to the list
                if (key.ToString().Contains("Joy"))
                {
                    gamepadInput.Add(key);
                }
            }

            //Cancels the current attempt to rebind the keys
            if (Input.GetButtonDown("Cancel"))
            {
                rebindButton = null;
            }
            //Get the input from the user to change the key binding
            else if (Input.anyKeyDown)
            {
                //Loop through each input in the list of Key Codes for a match
                foreach (KeyCode input in Enum.GetValues(typeof(KeyCode)))
                {
                    //Set the key binding to the input given and reset the check for input
                    if (Input.GetKeyDown(input) && rebindButton != null)
                    {
                        //Check if the input is not from a gamepad
                        if (!gamepadInput.Contains(input))
                        {
                            inputManager.SetKeyBoardButton(rebindButton, input);
                            buttonToLabel[rebindButton].text = input.ToString();
                            rebindButton = null;
                            GameManager.Instance.SaveInputs();
                            break;
                        }
                    }
                }
            }
        }

        //Rebind game pad input
        if (inputType == InputType.Gamepad)
        {
            //Loop through each XboxButton for a match
            foreach (XboxButton input in Enum.GetValues(typeof(XboxButton)))
            {
                //Set the gamepad binding for the input given
                if (XCI.GetButtonDown(input) && rebindButton != null)
                {
                    inputManager.SetGamePadButton(rebindButton, input);
                    buttonToImage[rebindButton].sprite = inputManager.GetGamePadButton(rebindButton);
                    rebindButton = null;
                    GameManager.Instance.SaveInputs();
                    break;                    
                }
            }

            //Check if the left trigger has been pressed
            if (XCI.GetAxisRaw(XboxAxis.LeftTrigger) >= 0.5f && rebindButton != null)
            {
                //Set the gamepad binding to left trigger
                inputManager.SetGamePadButton(rebindButton, XboxAxis.LeftTrigger);
                buttonToImage[rebindButton].sprite = inputManager.GetGamePadButton(rebindButton);
                rebindButton = null;
                GameManager.Instance.SaveInputs();
            }

            //Check if the right trigger has been pressed
            if (XCI.GetAxis(XboxAxis.RightTrigger) >= 0.5f && rebindButton != null)
            {
                //set the gamepad binding to right trigger
                inputManager.SetGamePadButton(rebindButton, XboxAxis.RightTrigger);
                buttonToImage[rebindButton].sprite = inputManager.GetGamePadButton(rebindButton);
                rebindButton = null;
                GameManager.Instance.SaveInputs();
            }
        }
    }

    //Update the buttons from outside rebinding
    public void UpdateButtons()
    {
        //Update the keyboard inputs
        if (inputType == InputType.Keyboard)
        {
            if (buttonToLabel != null)
            {
                //Update each button label
                foreach (string name in buttonToLabel.Keys)
                {
                    buttonToLabel[name].text = inputManager.GetKeyBoardButtonName(name);
                }
            }
        }

        //Updatethe gamepad inputs
        if (inputType == InputType.Gamepad)
        {
            //Update each button image
            foreach (string name in buttonToImage.Keys)
            {
                buttonToImage[name].sprite = inputManager.GetGamePadButton(name);
            }
        }
    }

    //Reset the rebinds to default
    public void ResetToDefault()
    {
        //Reset the rebinds for keboard input
        if (inputType == InputType.Keyboard)
        {
            //Reset the inputs to default
            inputManager.ResetInput(inputType);

            //Update each button
            foreach(string input in buttonToLabel.Keys)
            {
                buttonToLabel[input].text = inputManager.GetKeyBoardButtonName(input);                
            }

            //Save inputs
            GameManager.Instance.SaveInputs();
        }

        //Reset the rebinds for gamepad
        if(inputType == InputType.Gamepad)
        {
            //Reset the inputs to default
            inputManager.ResetInput(inputType);

            //Update each button
            foreach(string input in buttonToImage.Keys)
            {
                buttonToImage[input].sprite = inputManager.GetGamePadButton(input);
            }
            //Save inputs
            GameManager.Instance.SaveInputs();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Check if a key needs rebinding
        if (rebindButton != null)
        {
            StartCoroutine(RebindInput());
        }
    }
}

//Different input types
public enum InputType
{
    Keyboard,
    Gamepad,
    //Xbox, SteamController, PlayStation, Nintendo, Other
}
