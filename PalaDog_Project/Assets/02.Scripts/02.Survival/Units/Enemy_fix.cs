using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Enemy_fix : Monster
{
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        main_target = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();

    }
    private void OnEnable()
    {
        setStatus();
        curHP = HP;
        StartCoroutine(NormalAttack("Minion"));
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
            if (main_target.activeSelf != false)
            {
                moveDir = new Vector2(main_target.transform.position.x - rigid.position.x, 0).normalized;
            }
            else
            {
                moveDir = Vector2.left;
            }
            Move();
        }

    }
}
