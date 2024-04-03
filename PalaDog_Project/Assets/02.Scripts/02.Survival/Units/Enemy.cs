using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Enemy : Monster
{
    EnemyInfo info;
    
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        info = new EnemyInfo();
        
        

    }
    private void OnEnable()
    {
        setStatus();
        curHP = info.HP;

        StartCoroutine(NormalAttack("Player", "Minion"));
        isWalk = true;
        info.skill = new int[3];
        
    }
    private void Update()
    {
        if (curHP <= 0)
        {
            Die();
        }

    }

    public override void setStatus()
    {
        try
        {
            info = Parser.enemy_info_dict[ID];
            unitInfo = info;
            monster_info = info;
        }
        catch { Debug.Log("status Setting Error"); }
    }
    public override Unit setAttackTarget(string main_target_tag, string target_tag)
    {
       
        GameObject target = null;
        GameObject main_target = GameObject.FindGameObjectWithTag(main_target_tag);
        //주인공 공격할 수 있으면 공격
        float dist;
        try
        {
            dist = DistanceToTarget(main_target.transform.position, transform.position);
            if (dist <= info.atkRange)
            {
                return main_target.GetComponent<Unit>();
            }
                
        }
        catch
        {
            print("SetAttackTarget: maintarget missing set diff 99999");
            dist = 9999999;
        }

        if (atkTarget != null && DistanceToTarget(atkTarget.transform.position, transform.position) <= info.atkRange)
        {
            return atkTarget;
        }

        GameObject[] units = GameObject.FindGameObjectsWithTag(target_tag);

        foreach (GameObject u in units)
        {
            if (!u.activeSelf) { continue; }
            float tmp_dist = DistanceToTarget(u.transform.position, transform.position);
            if (tmp_dist < dist && tmp_dist <= info.atkRange)
            {
                dist = tmp_dist;
                target = u;
            }
        }
        if (target != null)
        {
            atkTarget = target.GetComponent<Unit>();
            return atkTarget;
        }
        else { return null; }
    }
    public override void Die()
    {
        isWalk = false;
        atkTarget = null;
        gameObject.SetActive(false);
        gameObject.transform.position = new Vector3(100, 0, 0);

    }
    void FixedUpdate()
    {
        if (isWalk)
        {

            SetMoveDir("Player");
            Move();
        }

    }
}
