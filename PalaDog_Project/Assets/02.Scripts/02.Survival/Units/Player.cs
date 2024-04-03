using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : Unit
{
    private static Player instance;
    int auraLV;
    int[] skill;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            animator = GetComponent<Animator>();
            rigid = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            unitInfo = new UnitInfo();
            DontDestroyOnLoad(gameObject);
            
        }
        else
        {
            Destroy(gameObject);
        }
        
    }
    private void Start()
    {
        setStatus();
        curHP = unitInfo.HP;
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

    public static Player Instance //게임매니저 인스턴스 접근
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
        isWalk = false;
        moveDir = Vector2.zero;
        GameManager.Instance.GameOver();
    }

    public override void setStatus()
    {
        try
        {
            unitInfo = Parser.minion_info_dict[ID];
        }
        catch { Debug.Log("status Setting Error Player"); }
        
    }


}
