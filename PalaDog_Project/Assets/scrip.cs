using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrip : MonoBehaviour
{
    private void OnDisable()
    {
        Destroy(gameObject);
    }
}
