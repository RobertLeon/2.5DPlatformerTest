//Created by Robert Bryant
//Based off of a tutorial by: Brackeys
//Youtube:https://www.youtube.com/watch?v=6OT43pvUyfY&t=584s
//Holds information of each sound for the Audio Controller
using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;                 //Name of the sound clip to play
    public AudioClip clip;              //Audio clip to play
    [Range(0f,1f)]
    public float volume;                //Volume of the audio clip
    [Range(0.1f,3f)]
    public float pitch;                 //Pitch of the audio clip
    public AudioMixerGroup mixer;       //Audio mixer group the sound belongs in
    public bool loop;                   //Does the audio clip loop?
    [HideInInspector]
    public AudioSource source;          //Source of the audio in the game world
}
