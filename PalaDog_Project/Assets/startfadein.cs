using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startfadein : MonoBehaviour
{
    public GameObject fadein;
    // Start is called before the first frame update
    private void OnEnable()
    {
        fadein.SetActive(true);
    }

}
