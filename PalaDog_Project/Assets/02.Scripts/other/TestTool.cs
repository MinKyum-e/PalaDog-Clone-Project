using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTool : MonoBehaviour
{
    private GameObject player;

    [Header("Player Variables")]
    [Header("Player Speed")]
    [SerializeField]
    [Range(0, 50)]private float movementSpeed = 10f;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    private void Update()
    {
        player.GetComponent<MovementObject>().setMoveSpeed(movementSpeed);
    }
}
