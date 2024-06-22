using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (null == instance)
            {
                instance = (T)FindObjectOfType(typeof(T));

                if (null == instance)
                {
                    var singletonObject = new GameObject();
                    instance = singletonObject.AddComponent<T>();
                    singletonObject.name = typeof(T).ToString() + " (Singleton)";
                    DontDestroyOnLoad(singletonObject);
                }
                else DontDestroyOnLoad(instance);
            }
            return instance;
        }
    }

    public void Awake()
    {
        if (null == instance)
        {
            instance = (T)FindObjectOfType(typeof(T));

            if (null == instance)
            {
                var singletonObject = new GameObject();
                instance = singletonObject.AddComponent<T>();
                singletonObject.name = typeof(T).ToString() + " (Singleton)";
                DontDestroyOnLoad(singletonObject);
            }
            else DontDestroyOnLoad(instance);
        }
    }
}