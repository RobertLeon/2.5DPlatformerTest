//Created by Robert Bryant
//Based off of a tutorial by: Brackeys
//Youtube: https://www.youtube.com/watch?v=zc8ac_qUXQY
//Handles the main menu for the game

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public AudioMixer audioMixer;               //Reference to the audio mixer
    public Slider masterVolumeSlider;           //Slider for the master volume
    public TMP_Text masterVolumeAmountText;     //Text display for the master volume
    public Slider musicVolumeSlider;            //Slider for the music volume
    public TMP_Text musicVolumeAmountText;      //Text display for the music volume
    public Slider seVolumeSlider;               //Slider for the sound effects voluem
    public TMP_Text seVolumeAmountText;         //Text display for the sound effects volume
    public TMP_Dropdown graphicsDropdown;       //Drop down for different graphic settings
    public TMP_Dropdown resolutionDropdown;     //Drop down for the different resolutions
    public GameObject confirmationMenu;         //Reference to the confirmation screen
    public TMP_Text confirmationText;           //Text to display in the confirmation screen
    public Button yesButton;                    //Reference to the confirmation button


    private Resolution[] resolutions;           //Holds all available screen resolutions
    private int masterVolumeAmount;             //Shows the master volume amount in the menu
    private int musicVolumeAmount;              //Shows the music volume amount in the menu
    private int seVolumeAmount;                 //Shows the soud effect volume amount in the menu


    private void Start()
    {
        //Set the drop down menu to the current quality level
        graphicsDropdown.value = QualitySettings.GetQualityLevel();

        //Set the volume text to an amount between 0 and 100
        masterVolumeAmount = (int)(masterVolumeSlider.normalizedValue * 100);
        masterVolumeAmountText.text = masterVolumeAmount.ToString();
        musicVolumeAmount = (int)(musicVolumeSlider.normalizedValue * 100);
        musicVolumeAmountText.text = musicVolumeAmount.ToString();
        seVolumeAmount = (int)(seVolumeSlider.normalizedValue * 100);
        seVolumeAmountText.text = seVolumeAmount.ToString();

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


    //Continues a saved game
    public void ContinueGame()
    {
        Debug.Log("Continuing a Game");
    }

    //Opens a confirmation menu for quitting the game
    public void QuitGame(string confirmationQuestion)
    {
        confirmationMenu.SetActive(true);
        confirmationText.text = confirmationQuestion;
        yesButton.Select();
        yesButton.onClick.AddListener(ExitGame);
    }

    //Set the game's master volume
    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", volume);
        masterVolumeAmount = (int)(masterVolumeSlider.normalizedValue * 100);
        masterVolumeAmountText.text = masterVolumeAmount.ToString();
    }

    //Set the game's music volume
    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", volume);
        musicVolumeAmount = (int)(musicVolumeSlider.normalizedValue * 100);
        musicVolumeAmountText.text = musicVolumeAmount.ToString();
    }

    //Set the games sound effect volume
    public void SetSoundEffectVolume(float volume)
    {
        audioMixer.SetFloat("SoundEffectVolume", volume);
        seVolumeAmount = (int)(seVolumeSlider.normalizedValue * 100);
        seVolumeAmountText.text = seVolumeAmount.ToString();
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