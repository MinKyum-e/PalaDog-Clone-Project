using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackgrounnd_single : MonoBehaviour
{
    [SerializeField]
    [Range(-1.0f, 1.0f)]
    private float moveSpeed = 0.1f;
    private Material material;

    private void Awake()
    {
        material = GetComponent<Renderer>().material;   
    }

    private void Update()
    {
        material.SetTextureOffset("_MainTex", new Vector2(material.GetTextureOffset("_MainTex").x,0) * moveSpeed * Time.time);   
    }
}
