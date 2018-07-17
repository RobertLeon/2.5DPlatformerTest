//Created by Robert Bryant
//
//Handles loading the different scenes of the game
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelLoader : MonoBehaviour
{
    public GameObject loadingScreen;            //Reference to the loading screen
    public Slider progressBar;                  //Reference to the loading bar
    public TMP_Text progressText;               //Reference to the progress text

    //Loads the specified scene
    public void LoadLevel(int sceneIndex)
    {
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    //Loads a scene asynchronously
    private IEnumerator LoadAsynchronously(int sceneIndex)
    {
        //
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        //Activate the loading screen
        loadingScreen.SetActive(true);

        //While the operation is active show the progress
        while(!operation.isDone)
        {
            //The numerical value of the loading process
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            //Display the loading progress as a percentage value
            progressBar.value = progress;
            progressText.text = progress * 100 + "%";

            yield return null;
        }

        //Hide the loading screen
        loadingScreen.SetActive(false);
    }
}
