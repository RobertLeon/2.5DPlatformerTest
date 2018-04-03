//Created by Robert Bryant
//
//Handles the movement of patroling enemies

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolEnemyController : EnemyMovement
{

    public Vector3[] localPatrolPoints;
    public float waitTime;
    public bool cyclic;
    [Range(0,2)] public float easeAmount;

    private Vector3[] globalPatrolPoints;
    
    private int patrolIndex;
    private float nextMoveTime;
    private float percentBetweenPoints;

	// Use this for initialization
	public override void Start ()
    {
        base.Start();

        globalPatrolPoints = new Vector3[localPatrolPoints.Length];

        for(int i = 0; i < globalPatrolPoints.Length; i++)
        {
            globalPatrolPoints[i] = localPatrolPoints[i] + transform.position;
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        velocity = CalculatePatrolMovement();

        collision.Move(velocity, movementDir);
	}


    private Vector3 CalculatePatrolMovement()
    {
        if(Time.time < nextMoveTime)
        {
            return Vector3.zero;
        }

        patrolIndex %= globalPatrolPoints.Length;
        int toPatrolIndex = (patrolIndex + 1) % globalPatrolPoints.Length;
        float distanceBetweenpoints = Vector3.Distance(globalPatrolPoints[patrolIndex],
            globalPatrolPoints[toPatrolIndex]);

        percentBetweenPoints += (Time.deltaTime * moveSpeed) / distanceBetweenpoints;
        percentBetweenPoints = Mathf.Clamp01(percentBetweenPoints);
        float easePercent = CalculateEase(percentBetweenPoints);

        Vector3 newPos = Vector3.Lerp(globalPatrolPoints[patrolIndex],
            globalPatrolPoints[toPatrolIndex],easePercent);

        if (percentBetweenPoints >= 1 )
        {
            percentBetweenPoints = 0;
            patrolIndex++;

            if (!cyclic)
            {
                if (patrolIndex >= globalPatrolPoints.Length - 1)
                {
                    patrolIndex = 0;
                    System.Array.Reverse(globalPatrolPoints);
                }
            }

            nextMoveTime = Time.time + waitTime;
        }

        return newPos - transform.position;
    }

    private float CalculateEase(float x)
    {
        float a = easeAmount + 1;
        return (Mathf.Pow(x, a) / (Mathf.Pow(x, a) + (Mathf.Pow(1 - x, a))));
    }
    
    private void OnDrawGizmos()
    {
        if (localPatrolPoints != null)
        {
            Gizmos.color = Color.yellow;
            float size = 0.5f;

            for (int i = 0; i < localPatrolPoints.Length; i++)
            {
                Vector3 globalPatrolPos = (Application.isPlaying) ? globalPatrolPoints[i] : localPatrolPoints[i] + transform.position;
                Gizmos.DrawLine(globalPatrolPos - Vector3.up * size, globalPatrolPos + Vector3.up * size);
                Gizmos.DrawLine(globalPatrolPos - Vector3.left * size, globalPatrolPos + Vector3.left * size);
            }
        }
    }
}
