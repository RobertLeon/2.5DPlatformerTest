//Created by Robert Bryant
//
//Standard horizontal projectile
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Projectile
{
    public float speed;               //Speed the projectile moves

    //Used for initialization
    public override void Start()
    {
        base.Start();        
    }

    //Update is called once per frame
    private void Update()
	{
        //Move the projectile
        transform.Translate(speed * direction * Time.deltaTime,0f,0f);

        //Check if the projectile has exceeded it's max range. 
        CalculateDistance();
	}
}
