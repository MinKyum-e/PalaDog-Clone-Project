using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffEvent : MonoBehaviour
{
    Actor actor;
    private void Awake()
    {
        actor = GetComponent<Actor>();
    }

}
