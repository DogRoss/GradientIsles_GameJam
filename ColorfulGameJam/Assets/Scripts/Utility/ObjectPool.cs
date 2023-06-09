using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Author: Justin Scruggs
 *
 * Object pooling script with a Type parameter.
 */ 
public class ObjectPool<T>
{
    public int size;
    public GameObject[] objects;
    public GameObject prefab;

    public ObjectPool(GameObject prefab, int size)
    {
        this.prefab = prefab;
        if (prefab.GetComponent<T>() == null)
            throw new System.Exception("The prefab doesn't have the component type!");
        this.size = size;
        objects = new GameObject[size];
        InitializeObjectPool();
    }

    private void InitializeObjectPool()
    {
        for (int i = 0; i < size; i++)
        {
            objects[i] = Object.Instantiate(prefab);
            objects[i].name = prefab.name + "(Pooled) ID:" + i;
            objects[i].SetActive(false);
        }
    }

    public T ActivateObject()
    {
        foreach (GameObject go in objects)
        {
            if (go.activeSelf == false)
            {
                //Debug.Log("Enabling pooled object: " + go.name + ".");
                go.SetActive(true);
                return go.GetComponent<T>();
            }
        }
        return default;
    }

    public bool DeactivateObject(GameObject go)
    {
        //Debug.Log("Deactivating pooled object: " + go.name + ".");
        go.SetActive(false);
        return true;
    }

}
