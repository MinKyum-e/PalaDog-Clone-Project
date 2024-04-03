using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : Unit
{
    public static EnemyBase instance = null;
    private void Awake()
    {
        unitInfo = new UnitInfo();
        if(instance == null)
        {
            instance = this;
        }

    }
    public static EnemyBase Instance()
    {
        return instance;
    }


    private void OnEnable()
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
        else if (curHP <= (unitInfo.HP * 0.25f) && GameManager.Instance.wave == 2)
        {
            GameManager.Instance.WaveChange();
        }
        else if(curHP <= (unitInfo.HP * 0.5f) && GameManager.Instance.wave == 1)
        {
            GameManager.Instance.WaveChange();
        }
        

        
    }

    public override void Die()
    {
        if(WaveManager.Instance.wave_type == WaveType.Boss)
        {
            GameManager.Instance.WaveChange();

        }
        else
        {

            GameManager.Instance.state = GameState.GAME_STAGE_CLEAR;

        }
        gameObject.SetActive(false);
    }

    public override void setStatus()
    {
        ID = 98;
        unitInfo.name= "enemyBase";
        unitInfo.HP = 2000;
        unitInfo.moveSpeed = 0;
    }
}
