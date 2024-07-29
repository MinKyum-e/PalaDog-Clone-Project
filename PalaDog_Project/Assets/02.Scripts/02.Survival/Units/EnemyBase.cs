using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class EnemyBase: MonoBehaviour
{
    public Actor actor;
    public Actions action;
    public static EnemyBase instance = null;
    public SpriteResolver spriteResolver;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        actor = GetComponent<Actor>();
        action = GetComponent<Actions>();
        spriteResolver = GetComponent<SpriteResolver>();
        DontDestroyOnLoad(gameObject);
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
            GameManager.Instance.WaveChange(3);
            spriteResolver.SetCategoryAndLabel("base", "2");
        }
        else if(actor.cur_status.HP <= (actor.status.HP * 0.5f) && GameManager.Instance.wave == 1)
        {
            GameManager.Instance.WaveChange(2);
            spriteResolver.SetCategoryAndLabel("base", "1");
        }

    }

    public void Die()
    {
        if(WaveManager.Instance.CheckBossStage())
        {
            GameManager.Instance.WaveChange(4);
            spriteResolver.SetCategoryAndLabel("base", "0");
            GetComponent<SpriteRenderer>().color = Color.white;
        }
        else
        {

            GameManager.Instance.StageClear();
            spriteResolver.SetCategoryAndLabel("base", "0");
            GetComponent<SpriteRenderer>().color = Color.white;

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
