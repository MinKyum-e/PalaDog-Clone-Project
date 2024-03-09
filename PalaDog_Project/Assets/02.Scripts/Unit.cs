using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{

    [SerializeField]
    private float moveSpeed;

    [SerializeField]
    private Vector3 moveDir = Vector3.right;

    void Start()
    {
        
    }

    void FixedUpdate()
    {
        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }
}
