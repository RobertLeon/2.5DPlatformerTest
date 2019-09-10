//Created by Robert Bryant
//Based on a tutorial by: Sebastian Lague
//Github: https://github.com/SebLague/Object-Pooling
//Handles the use and reuse of gameobjects in a scene
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    #region Instance
    public static ObjectPoolManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject poolManager = new GameObject("ObjectPool");
                poolManager.AddComponent<ObjectPoolManager>();
                instance = poolManager.GetComponent<ObjectPoolManager>();
            }
            return instance;
        }
    }

    private static ObjectPoolManager instance;
    #endregion
    
    //Dictionary of items in a pool
    private Dictionary<int, Queue<GameObject>> poolDictionary = new Dictionary<int, Queue<GameObject>>();

    //Creates a pool of objects to be used
    public void CreatePool(Transform parent, GameObject prefab, int poolSize)
    {
        int poolKey = prefab.GetInstanceID();

        //Check if the prefab is in the dictionary
        if (!poolDictionary.ContainsKey(poolKey))
        {
            //Add the prefab to the dictionary
            poolDictionary.Add(poolKey, new Queue<GameObject>());

            //Create the specified amount of items for the object pool
            for (int i = 0; i < poolSize; i++)
            {
                GameObject poolObj = Instantiate(prefab, parent) as GameObject;
                poolObj.SetActive(false);
                poolDictionary[poolKey].Enqueue(poolObj);
            }
            
            //Scene view debug and tidiness
            parent.name += " " + poolSize;
            parent.parent = transform;
        }
    }


    //Uses an object from a created pool
    public void UsePoolObject(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        int poolKey = prefab.GetInstanceID();

        //Check the dictonary for the prefab
        if (poolDictionary.ContainsKey(poolKey))
        {
            //Put the object in use at the end of the queue
            GameObject useObj = poolDictionary[poolKey].Dequeue();
            poolDictionary[poolKey].Enqueue(useObj);

            //Set the object active in the scene at the specified position
            useObj.SetActive(true);
            useObj.transform.localPosition = position;
            useObj.transform.localRotation = rotation;
        }
    }

}
