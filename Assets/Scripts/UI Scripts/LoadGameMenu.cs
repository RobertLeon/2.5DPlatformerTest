using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadGameMenu : MonoBehaviour
{
    public GameObject loadGamePrefab;
    public GameObject noSavesPrefab;

    private int numSaves = 0;

    private void OnEnable()
    {
        //Attempt to load save files

        if(numSaves == 0)
        {
            GameObject noSaves = Instantiate(noSavesPrefab, transform);
            noSaves.GetComponent<TMP_Text>().text = "No Saves Found.";
        }
    }

    private void OnDisable()
    {
        foreach(Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}
