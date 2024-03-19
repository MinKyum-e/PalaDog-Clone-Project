using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion_fix : Monster
{
    private void Awake()
    {
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void OnEnable()
    {
        setStatus();
        curHP = HP;
        moveDir = Vector2.right;
        StartCoroutine(NormalAttack("EnemyMainTarget", "Enemy"));
        isWalk = true;
    }
    private void Update()
    {
        if (curHP <= 0)
        {
            Die();
        }
    }

    void FixedUpdate()
    {
        if (isWalk)
        {
            Move();
        }

    }
}
