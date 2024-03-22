using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player_fix : Unit
{
    private static Player_fix instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            animator = GetComponent<Animator>();
            rigid = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            DontDestroyOnLoad(gameObject);
            setStatus();
        }
        else
        {
            Destroy(gameObject);
        }
        
    }
    private void OnEnable()
    {
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

    public static Player_fix Instance //게임매니저 인스턴스 접근
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }
    public override void Die()
    {
        GameManager.Instance.GameOver();
    }


}
