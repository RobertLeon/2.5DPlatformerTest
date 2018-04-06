using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager inputManager;
    public KeyBinding[] currentKeyBindings;
    public InputKey[] inputKeys;
    public KeyCode keyChange;
    public Event keyEvent;

    private KeyBinding keyToAssign;

    private bool assigningKey = false;

    private void Awake()
    {
        if (inputManager == null)
        {
            DontDestroyOnLoad(gameObject);
            inputManager = this;
        }
        else
        {
            if (inputManager != this)
            {
                Destroy(gameObject);
            }
        }
    }

    //Update is called once per frame
    void Update()
    {

    }

    public bool IsKeyBeingPressed(string keyName)
    {
        foreach (KeyBinding key in currentKeyBindings)
        {
            if (keyName.Equals(key.keyName) == true)
            {
                if (key.IsKeyDown() == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                continue;
            }
        }

        return false;
    }

    //
    public bool IsKeyBeingHeld(string keyName)
    {
        foreach (KeyBinding key in currentKeyBindings)
        {
            if (keyName.Equals(key.keyName) == true)
            {
                if (key.IsKeyHeld() == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                continue;
            }
        }

        return false;
    }

    //
    public bool IsKeyReleased(string keyName)
    {
        foreach (KeyBinding key in currentKeyBindings)
        {
            if (keyName.Equals(key.keyName) == true)
            {
                if (key.IsKeyUp() == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                continue;
            }
        }

        return false;
    }


    private KeyBinding GetKeyFromName(string name)
    {
        foreach (KeyBinding key in currentKeyBindings)
        {
            if (name.Equals(key.keyName) == true)
            {
                return key;
            }
        }
        return null;
    }

    private InputKey FindInputFromKeyCode(KeyCode currentKey)
    {
        foreach (InputKey key in inputKeys)
        {
            if (currentKey.CompareTo(key.input) == 0)
            {
                return key;
            }
        }

        return null;
    }

    private void AssignNewKey(KeyBinding key)
    {
        if (key == null)
        {
            Debug.LogError("No key being pressed.");
        }
        else
        {
            if (keyChange != KeyCode.None)
            {
                InputKey newKey = FindInputFromKeyCode(keyChange);

                if(newKey != null)
                {
                    key.AssignKey(newKey);
                    keyChange = KeyCode.None;
                    assigningKey = false;
                }
                else
                {
                    assigningKey = false;
                    keyChange = KeyCode.None;
                    Debug.LogError("Could not assign this key");
                }

                assigningKey = false;
            }
        }
    }
}
