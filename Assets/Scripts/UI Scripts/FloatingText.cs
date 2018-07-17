//Created by Robert Bryant
//
//Handles the movement and destruction of floating text

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    public Vector3 movement;                    //Direction and speed of the floating text
    public float delayTime;                     //Time until the text is destroyed

	//Update is called once per frame
	void Update()
	{
        transform.position += movement * Time.deltaTime;
        Destroy(gameObject, delayTime);
	}
}
