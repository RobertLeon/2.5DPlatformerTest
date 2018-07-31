//Created by Robert Bryant
//
//Handles the movement of a homing projectile
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingProjectile : Projectile
{
    public Transform target;                //Target of the projectile
    public float speed = 1f;                //Speed the projectile moves
    public float waitTime = 0.5f;           //Time from spawning the projectile to 
    public float hangTime = 1f;             //Time the projectile stays still in the air

    private bool initTargeting = false;     //Check if the projectile has looked for a target once
    private bool noTarget = false;

	//Use this for initialization
	public override void Start()
	{
        base.Start();

        StartCoroutine(Launch());
	}

	//Update is called once per frame
	private void Update()
	{
        //Find a new target
        if (target == null)
        {
            //Check ig there are no more targets or if the projectile has just been created
            if (!initTargeting || noTarget)
            {
                //Set rotation to (0,0,0) and move upwards
                transform.rotation = Quaternion.identity;
                transform.Translate(Vector3.up * speed * Time.deltaTime);
            }
            //Look for a new target
            else
            {
                StartCoroutine(FindNearestTarget());
            }
        }
        //Move the projectile towards the target
        else
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
        
        //Check if the projectile has exceeded the maximum distance
        CalculateDistance();
	}

    //
    private IEnumerator Launch()
    {
        yield return new WaitForSeconds(waitTime);        
        initTargeting = true;
        StartCoroutine(FindNearestTarget());        
    }


    //Finds the nearest target for the projectile
    private IEnumerator FindNearestTarget()
    {
        //Set the closest distance to infinity
        float closest = Mathf.Infinity;
        Transform nearest = null;

        //Finds a target for a player projectile
        if (playerProjectile)
        {
            EnemyStats[] targets = FindObjectsOfType<EnemyStats>();

            //Loop through each enemy
            foreach (EnemyStats enemy in targets)
            {
                //Calculate the distance to the current enemy
                float enemyDst = Vector2.Distance(transform.position, enemy.transform.position);

                //Set the closest enemy
                if (enemyDst < Mathf.Abs(closest))
                {
                    closest = enemyDst;
                    nearest = enemy.transform;
                }
            }
        }

        //Finds a target for an enemy projectile
        if (enemyProjectile)
        {
            PlayerStats player = FindObjectOfType<PlayerStats>();

            nearest = player.transform;
        }
        
        if (nearest != null)
        {
            yield return new WaitForSeconds(hangTime);
            transform.LookAt(nearest);
            target = nearest;
        }
        else
        {
            noTarget = true;
        }
    }
}
