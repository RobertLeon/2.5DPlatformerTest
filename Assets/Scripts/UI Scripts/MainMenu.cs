using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public AudioMixer audioMixer;               //Reference to the audio mixer
    public Slider volumeSlider;                 //Slider object
    public TMP_Text volumeAmountText;           //Text display of the volume
    public TMP_Dropdown graphicsDropdown;       //Drop down for different graphic settings
    public TMP_Dropdown resolutionDropdown;     //Drop down for the different resolutions
    public GameObject confirmationMenu;         //Reference to the confirmation screen
    public TMP_Text confirmationText;           //Text to display in the confirmation screen
    public Button yesButton;                    //Reference to the confirmation button


    private Resolution[] resolutions;           //Holds all available screen resolutions
    private int volumeAmount;                   //Shows the volume amount in the menu

    private void Start()
    {
        //Set the drop down menu to the current quality level
        graphicsDropdown.value = QualitySettings.GetQualityLevel();

        //Set the volume text to an amount between 0 and 100
        volumeAmount = (int)(volumeSlider.normalizedValue * 100);
        volumeAmountText.text = volumeAmount.ToString();

        //Get the available set of screen resolutions
        resolutions = Screen.resolutions;

        //Cleat the options in the drop down
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        //Loop through each resolution
        for (int i = 0; i < resolutions.Length; i++)
        {
            //Get each resolution pair and add it to a list
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            //If the width and height are the same as screen resolution 
            //set that as the current resolution
            if(resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        //Show the information in the resolution drop down
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    //Starts a new game
    public void StartNewGame()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Debug.Log("Starting Game");
    }

    //Opens a confirmation menu for quitting the game
    public void QuitGame(string confirmationQuestion)
    {
        confirmationMenu.SetActive(true);
        confirmationText.text = confirmationQuestion;
        yesButton.onClick.AddListener(ExitGame);
    }

    //Set the game's volume
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", volume);
        volumeAmount = (int)(volumeSlider.normalizedValue * 100);
        volumeAmountText.text = volumeAmount.ToString();
    }

    //Adjusts the quality of the game to the selected amount
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    //Set the screen
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    //Set the resolution to the selected size
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    //Exits the game
    void ExitGame()
    {
        Application.Quit();
        Debug.Log("Exiting game.");
    }
}