using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatacterSelect : MonoBehaviour
{

    public int sceneIndex;

    private LevelLoader levelLoader;

    private void Start()
    {
        levelLoader = FindObjectOfType<LevelLoader>();
        GetComponent<Button>().Select();
    }

    public void SelectCharacter(Character selectedCharacter)
    {
        GameManager.Instance.Player = selectedCharacter;
        levelLoader.LoadLevel(sceneIndex);
    }
}
