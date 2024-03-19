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
        else if (curHP <= (HP * 0.25f) && GameManager.Instance.wave == 2)
        {
            GameManager.Instance.WaveChange();
        }
        else if(curHP <= (HP*0.5f) && GameManager.Instance.wave == 1)
        {
            GameManager.Instance.WaveChange();
        }
        

        
    }

    public override void Die()
    {
        if(GameManager.Instance.waveManager.wave_type == WaveType.Boss)
        {
            GameManager.Instance.WaveChange();

        }
        else
        {

            GameManager.Instance.state = GameState.GAME_STAGE_CLEAR;

        }
        gameObject.SetActive(false);
    }

}
