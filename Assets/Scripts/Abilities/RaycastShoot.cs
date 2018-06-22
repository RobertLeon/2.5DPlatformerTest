//Created by Robert Bryant
//
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastShoot : MonoBehaviour
{
    [HideInInspector]
    public float abilityDamage;
    [HideInInspector]
    public float abilityRange;
    [HideInInspector]
    public float abilityDuration;
    [HideInInspector]
    public Transform abilityUser;
    public LayerMask layerMask;       
    [HideInInspector]
    public LineRenderer lineRenderer;

    private CollisionController collision;
    private float damage;

    public void Initialize()
    {
        //Get the components for the line renderer and collision controller
        lineRenderer = GetComponent<LineRenderer>();
        collision = GetComponent<CollisionController>();

        //If there is no ability user set the attached transform to be the ability user.
        if (abilityUser == null)
        {
            abilityUser = transform;
        }

        //If there is no line renderer on the game object add one and get the line renderer component.
        if(lineRenderer == null)
        {
            gameObject.AddComponent<LineRenderer>();
            lineRenderer = GetComponent<LineRenderer>();
        }
    }


    public void Shoot()
    {
        //
        Ray shootRay = new Ray();
        //Raycast information
        RaycastHit hit;

        //Set the origin and direction of the ray being shot
        shootRay.origin = abilityUser.position;
        shootRay.direction = abilityUser.right * collision.collisions.faceDir;

        //Start the ability's effect
        StartCoroutine(ShotEffect());
        
        //Set the first position of the line renderer
        lineRenderer.SetPosition(0, shootRay.origin);

        //If the ability hits something
        if (Physics.Raycast(shootRay.origin, shootRay.direction, out hit, abilityRange, layerMask))
        {
            EnemyStats enemyStats = hit.collider.GetComponent<EnemyStats>();
            PlayerStats playerStats = hit.collider.GetComponent<PlayerStats>();

            // Playe hits an enemy
            if (enemyStats != null)
            {
                //Get the player's stats and calculate the ability's damage
                playerStats = transform.GetComponent<PlayerStats>();
                damage = playerStats.combat.attack + damage;
                enemyStats.TakeDamage(damage, playerStats.combat.critChance);

                //Connect the line
                lineRenderer.SetPosition(1, hit.transform.position);
            }

            //Enemy hits a player
            if (playerStats != null)
            {
                //Get the enemy's stats and calculate the ability's damage
                enemyStats = transform.GetComponent<EnemyStats>();
                damage = enemyStats.combat.attack + abilityDamage;
                playerStats.TakeDamage(damage, enemyStats.combat.critChance);

                //Connect the line
                lineRenderer.SetPosition(1, hit.transform.position);
            }
        }
        //Ability hit nothing
        else
        {
            lineRenderer.SetPosition(1, shootRay.origin + shootRay.direction * abilityRange);
        }
    }

    //Shows the linerenderer for the ability effect
    private IEnumerator ShotEffect()
    {
        lineRenderer.enabled = true;

        yield return new WaitForSeconds(abilityDuration);

        lineRenderer.enabled = false;

    }
}
