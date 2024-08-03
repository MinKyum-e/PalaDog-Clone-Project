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


    public float bass_HP;
    public float diff_HP;





    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        actor = GetComponent<Actor>();
        action = GetComponent<Actions>();
        spriteResolver = GetComponent<SpriteResolver>();
    }
    public static EnemyBase Instance()
    {
        return instance;
    }

    private void OnEnable()
    {
        setStatus();
        if (GameManager.Instance !=null)
        {
            actor.status.HP = bass_HP + bass_HP * (((float)GameManager.Instance.stage - 1) * diff_HP);
            actor.cur_status.HP = bass_HP + bass_HP * (((float)GameManager.Instance.stage - 1) * diff_HP);
        }
        else
        {
            actor.status.HP = bass_HP;
            actor.cur_status.HP = bass_HP;
        }

    }

    private void Update()
    {
        if (actor.cur_status.HP <= 0)
        {
            actor.isDie = true;
            StartCoroutine(Die());
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

    public void DieBtn()
    {
        StartCoroutine(Die());
    }

   IEnumerator Die()
    {
        while(true)
        {

            if (WaveManager.Instance.CheckBossStage())
            {
                GameManager.Instance.WaveChange(4);
                spriteResolver.SetCategoryAndLabel("base", "0");
                GetComponent<SpriteRenderer>().color = Color.white;
                gameObject.SetActive(false);
                break;
            }
            else
            {

                if(GameManager.Instance.StageClear())
                {
                    spriteResolver.SetCategoryAndLabel("base", "0");
                    GetComponent<SpriteRenderer>().color = Color.white;
                    gameObject.SetActive(false);
                    break;
                }
                

            }
            yield return new WaitForSeconds(0.1f);
            
        }
        
    }

    public void setStatus()
    {
        actor.status = new CommonStatus();
        actor.cur_status = new CommonStatus();
        actor.ID = 98;
        actor.status.name= "enemyBase";
        actor.status.HP = bass_HP;

        actor.status.moveSpeed = 0;
    }
}
