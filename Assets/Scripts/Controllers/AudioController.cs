//Created by Robert Bryant
//Based off of a tutorial by: Brackeys
//Youtube:https://www.youtube.com/watch?v=6OT43pvUyfY&t=584s
//Handles the playing of specified sounds in the game

using System;
using UnityEngine.Audio;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    #region Singleton
    private static AudioController _instance;
    public static AudioController Instance
    {
        get
        {
            if(_instance == null)
            {
                GameObject audioManager = new GameObject("AudioManager");
                audioManager.AddComponent<AudioController>();
            }
            return _instance;
        }
    }
    #endregion

    public Sound[] sounds;                      //List of sounds to play


	//Use this for initialization
	void Awake()
	{
        //If there is no instance in the scene set this as the instance
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        //If there is an instance destroy this game object
        else
        {
            Destroy(gameObject);
        }

        //Add an audio source for each sound
        foreach (Sound sound in sounds)
        {
            sound.source =  gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.outputAudioMixerGroup = sound.mixer;

            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
        }
	}

    //Plays the specified sound
    public void Play(string name)
    {
        //Find the specified sound
        Sound s = Array.Find(sounds, sound => sound.name == name);

        //If the sound does not exist show a warning and return
        if(s == null)
        {
            Debug.LogWarning("Sound: " + name + " was not found.");
            return;
        }

        //Plays the sound
        s.source.Play();
    }

}
