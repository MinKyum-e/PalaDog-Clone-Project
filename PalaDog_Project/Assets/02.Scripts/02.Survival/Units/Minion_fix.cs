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
        main_target = GameObject.FindGameObjectWithTag("EnemyBase");
    }
    private void OnEnable()
    {
        setStatus();
        curHP = HP;
        StartCoroutine(NormalAttack("Enemy"));
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
            if(main_target.activeSelf != false)
            {
                moveDir = new Vector2(main_target.transform.position.x - rigid.position.x, 0).normalized;
            }
            else
            {
                moveDir = Vector2.right;
            }
            
            Move();
        }

    }
}
