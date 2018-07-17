//Created by Robert Bryant
//
//Flying movement that resembles a sin wave

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medusa : MonoBehaviour
{

    public float offset;                    //Offset of the movement on the y-axis
    public Vector2 movement;                //The enemy's movement

    private Vector3 startPos;               //Start position of the enemy

    //Use this for initialization
    void Start()
    {
        startPos = transform.position;
    }

    //Update is called once per frame
    void Update()
    {
        //Reverse the movement on the y-axis if the enemy exceeds the offset
        if (transform.position.y >= startPos.y + offset ||
            transform.position.y <= startPos.y - offset)
        {
           movement.y *= -1;
        }

        //Moves the enemy
        transform.Translate(movement * Time.deltaTime);

    }
}