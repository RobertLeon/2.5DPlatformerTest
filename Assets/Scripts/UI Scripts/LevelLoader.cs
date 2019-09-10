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
    private static LevelLoader _instance;

    public static LevelLoader Instance
    {

        get
        {
            if (_instance == null)
            {
                GameObject loadingScreen = new GameObject("Level Loader");
                loadingScreen.AddComponent<LevelLoader>();
            }
            return _instance;
        }
    }



    public GameObject loadingScreenPrefab;      //Loading screen prefab
    private Canvas loadingScreenCanvas;         //Reference to the canvas of the loading screen
    private Slider progressBar;                 //Reference to the loading bar
    private TMP_Text progressText;              //Reference to the progress text
    private GameObject loadingScreen;           //


    public void Awake()
    {
        //Singleton
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }


        if (loadingScreenPrefab == null)
        {
            loadingScreen = Instantiate(Resources.Load("LoadingScreen")) as GameObject;
            loadingScreenCanvas = loadingScreen.GetComponent<Canvas>();
            loadingScreenCanvas.enabled = false;
            progressBar = loadingScreen.GetComponentInChildren<Slider>();
            progressText = loadingScreen.GetComponentInChildren<TMP_Text>();
        }
        else
        {
            loadingScreen = transform.gameObject;
            loadingScreenCanvas = loadingScreen.GetComponent<Canvas>();
            loadingScreenCanvas.enabled = false;
            progressBar = loadingScreen.GetComponentInChildren<Slider>();
            progressText = loadingScreen.GetComponentInChildren<TMP_Text>();
        }
        
    }

    //Loads the specified scene
    public void LoadLevel(int sceneIndex)
    {
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    //Loads a scene asynchronously
    private IEnumerator LoadAsynchronously(int sceneIndex)
    {
        //Load the specified scene
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        //Activate the loading screen
        loadingScreenCanvas.enabled = true;

        //While the operation is active show the progress
        while(!operation.isDone)
        {
            //The numerical value of the loading process
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            //Display the loading progress as a percentage value
            progressBar.value = progress;
            progressText.text = "Loading: " +  progress * 100 + "%";

            yield return null;
        }

        //Deactivate the loading screen
        if (loadingScreen != null)
        {
            loadingScreenCanvas.enabled = false;
        }
    }
}
