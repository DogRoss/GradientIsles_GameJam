using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Author: Justin Scruggs
 * 
 * SingletonManager. Used for creating, destroying, and getting singletons.
 */
public class SingletonManager
{
    public static Dictionary<System.Type, GameObject> singletons = new Dictionary<System.Type, GameObject>();

    public static GameObject GetSingleton(System.Type objectType)
    {
        singletons.TryGetValue(objectType, out GameObject value);
        return value;
    }

    public static bool CreateSingleton(GameObject gameObject, System.Type type)
    {
        return CreateSingleton(gameObject, type, true);
    }

    public static bool CreateSingleton(GameObject gameObject, System.Type type, bool dontDestroyOnLoad)
    {
        if (GetSingleton(type) != null)
        {
            Object.Destroy(gameObject);
            return false;
        }
        singletons.Add(type, gameObject);
        if (dontDestroyOnLoad || gameObject.transform.parent == null)
        {
            Object.DontDestroyOnLoad(gameObject);
        }
        return true;
    }

    public static bool DestroySingleton(System.Type type)
    {
        GameObject go = GetSingleton(type);
        if (go != null)
        {
            Object.Destroy(go);
            singletons.Remove(type);
            return true;
        }
        return false;
    }
}
