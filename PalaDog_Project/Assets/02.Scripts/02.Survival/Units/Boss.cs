using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Monster
{
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

    }
    private void OnEnable()
    {
        setStatus();
        curHP = HP;
        moveDir = Vector2.left;
        StartCoroutine(NormalAttack("Player", "Minion"));
        isWalk = true;
    }
    private void Update()
    {
        if (curHP <= 0)
        {
            GameManager.Instance.state = GameState.GAME_STAGE_CLEAR;
            Die();
        }
    }

    void FixedUpdate()
    {
        if (isWalk)
        {
            SetMoveDir("Player");
            Move();
        }

    }

}
