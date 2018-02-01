using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider volumeSlider;
    public TMP_Text volumeAmountText;
    public TMP_Dropdown graphicsDropdown;
    public TMP_Dropdown resolutionDropdown;
    public GameObject confirmationMenu;
    public TMP_Text confirmationText;
    public Button yesButton;


    private Resolution[] resolutions;
    private int volumeAmount;

    private void Start()
    {
        graphicsDropdown.value = QualitySettings.GetQualityLevel();

        volumeAmount = (int)(volumeSlider.normalizedValue * 100);
        volumeAmountText.text = volumeAmount.ToString();

        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if(resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void StartNewGame()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Debug.Log("Starting Game");
    }

    public void QuitGame(string confirmationQuestion)
    {
        confirmationMenu.SetActive(true);
        confirmationText.text = confirmationQuestion;
        yesButton.onClick.AddListener(ExitGame);
    }


    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", volume);
        volumeAmount = (int)(volumeSlider.normalizedValue * 100);
        volumeAmountText.text = volumeAmount.ToString();
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    void ExitGame()
    {
        Application.Quit();
        Debug.Log("Exiting game.");
    }
}