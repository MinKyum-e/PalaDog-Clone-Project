using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_fix : Unit
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
    }

    private void Update()
    {
        if (curHP <= 0)
        {
            Die();
        }
    }
    private void FixedUpdate()
    {
        Move();

    }
    public override void Die()
    {
        GameManager.Instance.state = GameState.GAME_OVER;
    }


}
