using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathZone : MonoBehaviour
{
    public Vector3 spawnPoint;
    public float outOfBoundsDamage = 10f;

    private PlayerStats playerStats;

    private void Start()
    {
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            playerStats.TakeDamage(outOfBoundsDamage, -1f);
            other.transform.position = spawnPoint;
        }
    }


    //Restarts current scene
    private IEnumerator RestartScene()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0.6f, 0.7f, 1, .5f);
        Gizmos.DrawCube(spawnPoint, Vector3.one);

        Gizmos.color = Color.grey;
        Gizmos.DrawCube(transform.position, transform.localScale);
    }
}