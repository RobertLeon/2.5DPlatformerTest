//Created by Robert Bryant
//
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    public Vector3 movement;
    public float delayTime;

	//Update is called once per frame
	void FixedUpdate()
	{
        transform.position += movement * Time.deltaTime;
        Destroy(gameObject, delayTime);
	}
}
