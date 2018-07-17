//Created by Robert Bryant
//
//Defines a player character for the game
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Character")]
public class Character : ScriptableObject
{
    public string characterName = "";           //Name of the character
    public GameObject playerPrefab;             //The character's game object
    public Ability[] characterAbilities;        //Abilities the character has
}
