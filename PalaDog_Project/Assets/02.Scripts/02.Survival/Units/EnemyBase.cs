using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : Unit
{

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
    public override void Die()
    {
        GameManager.Instance.state = GameState.GAME_STAGE_CLEAR;
    }

}
