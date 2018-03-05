using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//When the player falls off of the playable area respawn them at a specified point.
public class VoidRespawn : MonoBehaviour
{
    public Vector3 spawnPoint;       //Position to move the player for falling off the map

    private void OnTriggerEnter(Collider other)
    {

        if(other.tag == "Player")
        {
            //Move the player to the spawn point
            other.transform.position = spawnPoint;
        }
    }


    //Restarts current scene
    private IEnumerator RestartScene()
    {
        //Wait to restart scene
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    //Draws objects in the scene view
    private void OnDrawGizmos()
    {
        //Spawn point
        Gizmos.color = new Color(0.6f, 0.7f, 1, .5f);
        Gizmos.DrawCube(spawnPoint, Vector3.one);

        //Void Zone
        Gizmos.color = Color.grey;
        Gizmos.DrawCube(transform.position, transform.localScale);
    }
}