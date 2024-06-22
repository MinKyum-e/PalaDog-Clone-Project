using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI; 

public class FadeEffect : MonoBehaviour
{
    private static FadeEffect instance = null;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 150f / 255f);
        gameObject.SetActive(false);
        
    }
    
    public static FadeEffect Instance
    {
        get { return instance; }
    }
}
