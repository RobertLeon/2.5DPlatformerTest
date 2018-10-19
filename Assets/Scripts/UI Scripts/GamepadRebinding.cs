//Created by: Robert Bryant
//
//Handles the creation of the gamepad rebinding UI and some controller specific customization.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using XInputDotNetPure;

public class GamepadRebinding : KeyRebinding
{
    public GameObject gamePadSlider;            //Prefab for the UI SLiders
    
    private float deadZone;                     //Deadzone of the controller's thumbsticks
    private float vibrationAmount;              //Vibration strength of the gamepad
    private Dictionary<string,Slider> sliders;

	// Use this for initialization
	public override void Start ()
    {
        base.Start();

        sliders = new Dictionary<string, Slider>();

        PlayerInputData inputData = GameManager.Instance.LoadInputs();

        vibrationAmount = inputData.vibrationStrength;
        deadZone = inputData.deadZone;

        //Create the vibration slider in the Scroll Rect
        GameObject vibrationSlider = Instantiate(gamePadSlider, transform);
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
        GameObject deadZoneSlider = Instantiate(gamePadSlider, transform);
        deadZoneSlider.transform.localScale = Vector3.one;
        TMP_Text dLabel = deadZoneSlider.GetComponentInChildren<TMP_Text>();
        dLabel.text = "Dead Zone";
        Slider dSlider = deadZoneSlider.GetComponentInChildren<Slider>();
        TMP_Text dDisplay = dSlider.GetComponentInChildren<TMP_Text>();
        dSlider.value = deadZone;
        dDisplay.text = (deadZone * 100).ToString("N0");
        dSlider.onValueChanged.AddListener(delegate { SetDeadZone(dSlider.value, dDisplay); });
        sliders.Add(dLabel.text, dSlider);

        //Create a new navigation and set the select on up to the last rebind button
        Navigation nav = new Navigation();
        nav.mode = Navigation.Mode.Explicit;
        nav.selectOnUp = dSlider;

        //Set the navigation on the inputMenu Button
        inputMenuButton.navigation = nav;
    }

    //When the script is enables
    private void OnEnable()
    {
        //Load the gamepad inputs from file
        GameManager.Instance.SetGamePadInputs(GameManager.Instance.LoadInputs());
    }

    //Return the controller deadzone
    public float GetDeadZone()
    {
        return deadZone;
    }

    //Return the controller vibration strength
    public float GetVibration()
    {
        return vibrationAmount;
    }

    //Sets the deadzone via UI Slider
    private void SetDeadZone(float amount, TMP_Text display)
    {
        //Change the display for the amount changed
        display.text = (amount * 100).ToString("N0");

        //Set the deadzone amount to the amount passed in
        deadZone = amount;
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
    }

    //Gives a test vibration for the player to calibrate
    private IEnumerator TestVibration(float amount)
    {
        GamePad.SetVibration(PlayerIndex.One, amount, amount);

        yield return new WaitForSeconds(1.0f);

        GamePad.SetVibration(PlayerIndex.One, 0, 0);
    }
}
