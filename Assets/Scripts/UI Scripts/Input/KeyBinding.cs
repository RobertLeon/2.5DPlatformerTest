using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBinding : MonoBehaviour
{
    public string keyName;
    public Texture2D keyIcon;
    public KeyCode currentKey;

	public void AssignKey(InputKey key)
    {
        keyIcon = key.keyIcon;
        currentKey = key.input;
    }

    public bool IsKeyDown()
    {
        if (Input.GetKeyDown(currentKey))
        {
            Debug.Log(keyName + " was pressed");
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool IsKeyUp()
    {
        if(Input.GetKeyUp(currentKey))
        {
            Debug.Log(keyName + " was released");
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool IsKeyHeld()
    {
        if(Input.GetKey(currentKey))
        {
            Debug.Log(keyName + " is being held");
            return true;
        }
        else{
            return false;
        }
    }

    public Texture2D GetKeyIcon()
    {
        return keyIcon;
    }
}
