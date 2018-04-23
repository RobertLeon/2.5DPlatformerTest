using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTransition : MonoBehaviour
{

    public Transform exitDoor;
    public Animation transitionAnimation;
    public float transitionTime;

	//Use this for initialization
	void Start()
	{
		
	}


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            StartCoroutine(DoorTransition(other));
        }
    }

    private IEnumerator DoorTransition(Collider col)
    {
        exitDoor.GetComponent<SphereCollider>().enabled = false;
        col.transform.position = exitDoor.position;        
        GetComponent<SphereCollider>().enabled = false;

        yield return new WaitForSeconds(transitionTime);

        exitDoor.GetComponent<SphereCollider>().enabled = true;
        GetComponent<SphereCollider>().enabled = true;
    }
}
