
using UnityEngine;

using System.Collections.Generic;


public class Actor : MonoBehaviour
{
    public int ID;
    public CommonStatus status;
    
    public CommonStatus cur_status;
    public BuffStruct cur_buff;
    public PoolManager enemy_poolManager;
    public PoolManager minion_poolManager;


    public GameObject atkTarget;
    public bool isDie = false;
    public bool can_walk = true;
    public bool can_action = true;

    public ActorSkillInfo[] skills;
    public Rigidbody2D rigid;
    public SpriteRenderer spriteRenderer; 
    public Animator animator;
    public EffectPlayer effect_player;
    public HPBarUI hpbar;
    public Skill_Effect skill_effect;
    public int final_damage;

    

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        enemy_poolManager = GameObject.FindGameObjectWithTag("EnemyPool").GetComponent<PoolManager>();
        minion_poolManager = GameObject.FindGameObjectWithTag("MinionPool").GetComponent<PoolManager>();
        effect_player = GetComponentInChildren<EffectPlayer>(); 
        hpbar = GetComponentInChildren<HPBarUI>();

        skills = new ActorSkillInfo[3];
        for(int i=0;i<3;i++)
        {
            skills[i] = new ActorSkillInfo();
        }
    }
    private void OnEnable()
    {
        cur_status.HP = status.HP;
        cur_buff = new BuffStruct();
        /*cur_status.moveDir = Vector2.right;*/


        Transform buf = transform.Find("Buff");
        if(buf != null )
        {
            buf.GetComponent<BuffSystem>().buff_init();
        }        
    }
    //�߻�
    /* public abstract void Die();
     public abstract void setStatus();*/
    private void OnDisable()
    {
        
    }
}

