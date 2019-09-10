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
using XboxCtrlrInput;
using XInputDotNetPure;
using TMPro;


public class KeyRebinding : MonoBehaviour {

    public TMP_Text inputTitle;                             //Reference to the menu title
    public Transform scrollViewContent;                     //Reference to the content viewer holding the buttons
    public GameObject keyBoardBindingPrefab;                //Prefab of the key board rebind buttons
    public GameObject gamePadBindingPrefab;                 //Prefabe of the game pad rebind buttons
    public GameObject resetInputPrefab;                     //Prefab for the button to reset the inputs
    public GameObject sliderPrefab;                         //Prefab for the vibration and deadzone sliders
    public Button backButton;                               //Reference to the UI button for menu navigation

    private InputType inputType;                            //Type of input to display    
    private InputManager inputManager;                      //Reference to the Input Manager Script   
    private Button lastGamepadButton;                       //Reference to the last button in the gamepad rebind list
    private string rebindButton = null;                     //String to check which button is being rebinded   
    private Dictionary<string, TMP_Text> buttonToLabel;     //Dictionary of keyboard inputs to change
    private Dictionary<string, Image> buttonToImage;        //Dictionary of gamepad inputs to change
    private Dictionary<string, Slider> sliders;             //Dictionary of sliders for controller input
    private float vibrationAmount;                          //Amount of vibration on the controller
    private float deadZone;                                 //Deadzone of the controller's movement axis


    private void Start()
    {
        //Get the inputManager from the scene
        inputManager = FindObjectOfType<InputManager>();
    }

    //Removes the UI elements from the Scroll View
    public void RemoveButtons()
    {
        foreach(Transform child in scrollViewContent)
        {
            Destroy(child.gameObject);
        }
    }

    //Initializes the input menu for gamepad inputs
    public void InitializeGamePadInputs()
    {
        //Change the title of the screen
        inputTitle.text = "Controller";

        PlayerInputData controllerInput = inputManager.LoadInputs();
        inputType = InputType.Gamepad;

        if (controllerInput == null)
        {
            inputManager.ResetInput(inputType);
        }

        buttonToImage = new Dictionary<string, Image>();

        GameObject resetInputs = Instantiate(resetInputPrefab, scrollViewContent);
        resetInputs.GetComponentInChildren<Button>().Select();
        resetInputs.GetComponentInChildren<Button>().onClick.AddListener(() => ResetToDefault());

        //Create a button for each input
        foreach (string button in inputManager.GetGamePadInputNames())
        {
            string btnName;
            btnName = button;

            //Create a button in the menu for a key bind
            GameObject bind = Instantiate(gamePadBindingPrefab, scrollViewContent);
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

        //Create and set the navigation for the reset inputs button
        Navigation nav = new Navigation();
        nav.mode = Navigation.Mode.Explicit;
        nav.selectOnDown = buttonToImage.FirstOrDefault().Value.GetComponentInParent<Button>();
        resetInputs.GetComponentInChildren<Button>().navigation = nav;

        lastGamepadButton = buttonToImage.LastOrDefault().Value.GetComponentInParent<Button>();

        sliders = new Dictionary<string, Slider>();

        /*Slider Game Object Configutation:            
            GamePadSlider
                TMP_TXT
                Slider
                    TMP_TXT

        CODE DOES NOT FUNCTION IF GAME OBJECT IS NOT SET UP PROPERLY
        */

        //GET DATA FROM FILE
        //For deadzone and vibration    
       

        vibrationAmount = controllerInput.GamePadVibration;
        deadZone = controllerInput.GamePadDeadZone;        

        //Create the vibration slider in the Scroll Rect
        GameObject vibrationSlider = Instantiate(sliderPrefab, scrollViewContent);
        vibrationSlider.transform.localScale = Vector3.one;
        TMP_Text vLabel = vibrationSlider.GetComponentInChildren<TMP_Text>();
        vLabel.text = "Vibration";
        Slider vSlider = vibrationSlider.GetComponentInChildren<Slider>();
        TMP_Text vDisplay = vSlider.GetComponentInChildren<TMP_Text>();
        vSlider.value = vibrationAmount;
        vDisplay.text = (vibrationAmount * 100).ToString("N0");
        vSlider.onValueChanged.AddListener(delegate { SetVibration(vSlider.value, vDisplay, true); });
        sliders.Add(vLabel.text, vSlider);

        //Create the deadzone slider in the Scroll Rect
        GameObject deadZoneSlider = Instantiate(sliderPrefab, scrollViewContent);
        deadZoneSlider.transform.localScale = Vector3.one;
        TMP_Text dLabel = deadZoneSlider.GetComponentInChildren<TMP_Text>();
        dLabel.text = "Dead Zone";
        Slider dSlider = deadZoneSlider.GetComponentInChildren<Slider>();
        TMP_Text dDisplay = dSlider.GetComponentInChildren<TMP_Text>();
        dSlider.value = deadZone;
        dDisplay.text = (deadZone * 100).ToString("N0");
        dSlider.onValueChanged.AddListener(delegate { SetDeadZone(dSlider.value, dDisplay); });
        sliders.Add(dLabel.text, dSlider);

        //Create a navigation for the vibration slider
        nav = new Navigation();
        nav.mode = Navigation.Mode.Explicit;
        nav.selectOnUp = lastGamepadButton;
        nav.selectOnDown = dSlider;
        vSlider.navigation = nav;

        //Create a navigation and set the select on up to the last rebind button
        nav = new Navigation();
        nav.mode = Navigation.Mode.Explicit;
        nav.selectOnUp = dSlider;

        //Set the navigation on the inputMenu Button
        backButton.navigation = nav;
    }

    //Initializes the input menu for keyboard inputs
    public void InitializeKeyBoardInputs()
    {
        inputTitle.text = "Keyboard";

        //Create a new dictionary of buttons
        buttonToLabel = new Dictionary<string, TMP_Text>();
        PlayerInputData keyboardData = inputManager.LoadInputs();
        inputType = InputType.Keyboard;

        if(keyboardData == null)
        {
            inputManager.ResetInput(inputType);
        }

        GameObject resetInputs = Instantiate(resetInputPrefab, scrollViewContent);
        resetInputs.GetComponentInChildren<Button>().Select();
        resetInputs.GetComponentInChildren<Button>().onClick.AddListener(() => ResetToDefault());

        //Create a button for each input
        foreach (string button in inputManager.GetKeyBoardInputNames())
        {
            string btnName;
            btnName = button;

            //Create a button in the menu for a key bind
            GameObject bind = Instantiate(keyBoardBindingPrefab, scrollViewContent);
            bind.transform.name = button + " Rebind";
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
        backButton.navigation = nav;

        
        //Set the navigation for the reset inputs button
        nav = new Navigation();
        nav.mode = Navigation.Mode.Explicit;
        nav.selectOnDown = buttonToLabel.FirstOrDefault().Value.transform.GetComponentInParent<Button>();
        resetInputs.GetComponentInChildren<Button>().navigation = nav;
    }

    //Selects the key to be rebind
    private void RebindKey(string button)
    {
        rebindButton = button;
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
                            inputManager.SaveInputs();
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
                    inputManager.SaveInputs();
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
                inputManager.SaveInputs();
            }

            //Check if the right trigger has been pressed
            if (XCI.GetAxis(XboxAxis.RightTrigger) >= 0.5f && rebindButton != null)
            {
                //set the gamepad binding to right trigger
                inputManager.SetGamePadButton(rebindButton, XboxAxis.RightTrigger);
                buttonToImage[rebindButton].sprite = inputManager.GetGamePadButton(rebindButton);
                rebindButton = null;
                inputManager.SaveInputs();
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
            inputManager.SaveInputs();
        }

        //Reset the rebinds for gamepad
        if (inputType == InputType.Gamepad)
        {
            //Reset the inputs to default
            inputManager.ResetInput(inputType);

            //Update each button
            foreach(string input in buttonToImage.Keys)
            {
                buttonToImage[input].sprite = inputManager.GetGamePadButton(input);
            }
            //Save inputs
            inputManager.SaveInputs();
        }
    }

    public float GetGamepadVibration()
    {
        return vibrationAmount;
    }

    public float GetGamepadDeadzone()
    {
        return deadZone;
    }

    //Sets the deadzone via UI Slider
    private void SetDeadZone(float amount, TMP_Text display)
    {
        //Change the display for the amount changed
        display.text = (amount * 100).ToString("N0");

        //Set the deadzone amount to the amount passed in
        deadZone = amount;

        //Save the deadzone amount to file        
        inputManager.SaveInputs();
    }

    //Sets the vibration strength via UI Slider
    private void SetVibration(float amount, TMP_Text display, bool testVib = false)
    {
        //Change the display for the amount changed
        display.text = (amount * 100).ToString("N0");

        //Set vibration for the gamepad
        vibrationAmount = amount;

        //Test the vibration
        if (testVib)
        {
            StartCoroutine(TestVibration(amount));
        }

        //Save the vibration to file        
        inputManager.SaveInputs();
    }

    //Gives a test vibration for the player to calibrate
    private IEnumerator TestVibration(float amount)
    {
        GamePad.SetVibration(PlayerIndex.One, amount, amount);

        yield return new WaitForSeconds(1.0f);

        GamePad.SetVibration(PlayerIndex.One, 0, 0);
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
