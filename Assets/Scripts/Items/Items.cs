using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Items : ScriptableObject
{
    public enum ItemRarity
    {
        Common,
        Uncommon,
        Rare,
        Boss,
        Artifact
    }
    public string ItemName = "New Item";
    public ItemRarity itemRarity;
    public Texture2D itemIcon;
    public bool isStackable;
    public int maxStacks = 99;
    [TextArea]
    public string itemDescription;

    [HideInInspector]
    public Color[] itemColor = {
    };


    public abstract void Initialize(PlayerStats playerStats);
}