//Created by Robert Bryant
//
//Game Manager
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;
    public AudioClip[] audioClips;

    private AudioSource audioSource;
    
    private void Awake()
    {
        if(gameManager == null)
        {
            DontDestroyOnLoad(gameObject);
            gameManager = this;
        }
        else if( gameManager != this)
        {
            Destroy(gameObject);
        }

        audioSource = GetComponent<AudioSource>();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        audioSource.Stop(); 

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        Debug.Log("Current scene index is: " + currentSceneIndex);

        audioSource.clip = audioClips[currentSceneIndex];
        audioSource.Play();
    }
}
