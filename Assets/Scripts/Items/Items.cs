﻿//Created by Robert Bryany
//
//Allows the creation of items that modify entity's stats
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Items : ScriptableObject
{
    //Values for the item's rarity
    public enum ItemRarity
    {
        Common,
        Uncommon,
        Rare,
        Boss,
        Artifact
    }

    public string itemName = "New Item";        //The item's name
    public ItemRarity itemRarity;               //Item rarity
    public Sprite itemIcon;                     //Item sprite to show
    public bool isStackable;                    //Does the item stack
    public int maxStacks = 99;                  //Maximum amount the item stack
    public ItemAbility itemAbility;             //Ability of the item
    [TextArea]
    public string itemDescription;              //Description of the item

    [HideInInspector]
    public Color[] itemColor = {                //Color of the item based on rarity
    };
    
    //Initialization of each item
    public abstract void OnPickUp(Stats stats);    
}