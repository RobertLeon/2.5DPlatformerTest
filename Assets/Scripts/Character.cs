using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Character")]
public class Character : ScriptableObject
{
    public string characterName = "";
    public GameObject playerPrefab;
    public Ability[] characterAbilities;
}
