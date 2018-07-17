//Created by Robert Bryant
//
//Item Table
using UnityEngine;

[System.Serializable]
public class ItemTable
{
    public Items item;              //The item to be spawned
    [Range(0.0f,1.0f)]
    public float dropChance;        //Chance for the item to drop
}
