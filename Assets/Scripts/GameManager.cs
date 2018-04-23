//Created by Robert Bryant
//
//Game Manager

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;



public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if(_instance == null)
            {
                GameObject gameManager = new GameObject("GameManager");
                gameManager.AddComponent<GameManager>();
                gameManager.AddComponent<AudioSource>();
            }
            return _instance;
        }
    }

    public Character Player { get; set; }
    public AudioClip CurrentSong { get; set; }
    public AudioClip[] songList;

    private AudioSource audioSource;
    private AudioMixer audioMixer;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void Start()
    {
        audioMixer = Resources.Load("MainMixer") as AudioMixer;
        audioSource = GetComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("Music")[0];
        SceneManager.sceneLoaded += OnSceneLoaded;        
    }

    public void SaveGame()
    {
        if (!File.Exists(Application.persistentDataPath + "/SaveData.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/SaveData.dat");

            PlayerData data = new PlayerData();

            data.unlockedItems = new List<Items>();
            data.unlockedItems = new List<Items>();

            bf.Serialize(file, data);
            file.Close();
        }
    }

    public void LoadGame()
    {

    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        Debug.Log(SceneManager.GetActiveScene().buildIndex);
        audioSource = GetComponent<AudioSource>();

        if(audioSource == null)
        {
            gameObject.AddComponent<AudioSource>();
            audioSource = GetComponent<AudioSource>();
        }

      

        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
        else
        {
            Debug.Log("Audio Source is already playing");
        }
    }
}

[Serializable]
class PlayerData
{
    public List<Items> unlockedItems;
    public List<Character> unlockedCharacters;
    
}

