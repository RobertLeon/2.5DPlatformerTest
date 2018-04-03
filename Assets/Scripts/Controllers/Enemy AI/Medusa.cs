//Created by Robert Bryant
//
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medusa : MonoBehaviour
{

    public float offset;
    public Vector2 movement;

    private Vector3 startPos;

    //Use this for initialization
    void Start()
    {
        startPos = transform.position;
    }

    //Update is called once per frame
    void Update()
    {
       
        if (transform.position.y >= startPos.y + offset ||
            transform.position.y <= startPos.y - offset)
        {
           movement.y *= -1;
        }

        transform.Translate(movement * Time.deltaTime);

    }
}