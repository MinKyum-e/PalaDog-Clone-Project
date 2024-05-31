using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase: MonoBehaviour
{
    public Actor actor;
    public Actions action;
    public static EnemyBase instance = null;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        actor = GetComponent<Actor>();
        action = GetComponent<Actions>();
    }
    public static EnemyBase Instance()
    {
        return instance;
    }

    private void OnEnable()
    {
        setStatus();
        actor.cur_status.HP = actor.status.HP;
    }

    private void Update()
    {
        if (actor.cur_status.HP <= 0)
        {
            actor.isDie = true;
            Die();
        }
        else if (actor.cur_status.HP <= (actor.status.HP * 0.25f) && GameManager.Instance.wave == 2)
        {
            GameManager.Instance.WaveChange();
        }
        else if(actor.cur_status.HP <= (actor.status.HP * 0.5f) && GameManager.Instance.wave == 1)
        {
            GameManager.Instance.WaveChange();
        }
        

        
    }

    public void Die()
    {
        if(WaveManager.Instance.CheckBossStage())
        {
            GameManager.Instance.WaveChange();

        }
        else
        {

            GameManager.Instance.state = GameState.GAME_STAGE_CLEAR;

        }
        gameObject.SetActive(false);
    }

    public void setStatus()
    {
        actor.status = new CommonStatus();
        actor.cur_status = new CommonStatus();
        actor.ID = 98;
        actor.status.name= "enemyBase";
        actor.status.HP = 2000;
        actor.status.moveSpeed = 0;
    }
}
