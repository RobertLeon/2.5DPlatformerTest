//Created by Robert Bryant
//
//Handles the transparency of walls while a player is behind them
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideWalls : MonoBehaviour {

    public Color transparent;           //Color for transparency
    private Color defaultColor;         //The default color of the wall
    private Renderer wall;              //Reference to the Renderer component on the wall


    //use this for initialization.
    private void Start()
    {
        wall = transform.parent.GetComponent<Renderer>();
        defaultColor = wall.material.color;
        transparent = new Color(defaultColor.r, defaultColor.g, defaultColor.b, 0);
    }

    //Player enters the area set the wall's transparency
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            wall.material.color = transparent;
        }
    }

    //Player leaves the area reset the wall's transparency
    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            wall.material.color = defaultColor;
        }
    }
}
