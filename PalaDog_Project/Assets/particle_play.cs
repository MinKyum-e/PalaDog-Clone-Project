using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particle_play : MonoBehaviour
{
    ParticleSystem ps;
    // Start is called before the first frame update
    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
    }
    private void OnEnable()
    {
        ps.Play();
    }
}
