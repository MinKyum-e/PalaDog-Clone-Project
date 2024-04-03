using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Monster
{
    EnemyInfo info;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

    }
    private void OnEnable()
    {
        setStatus();

        curHP = info.HP;
        moveDir = Vector2.left;
        StartCoroutine(NormalAttack("Player", "Minion"));
        isWalk = true;
        
    }
    private void Update()
    {
        if (curHP <= 0)
        {
            if(GameManager.Instance.chapter ==  GameManager.Instance.MAX_CHAPTER)
            {
                GameManager.Instance.state = GameState.GAME_CLEAR;
            }
            else
            {
                GameManager.Instance.state = GameState.GAME_CHAPTER_CLEAR;
            }
            
            Die();
        }
    }

    void FixedUpdate()
    {
        if (isWalk)
        {
            SetMoveDir("Player");
            Move();
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
    public override void Die()
    {
        isWalk = false;
        atkTarget = null;
        gameObject.SetActive(false);
        gameObject.transform.position = new Vector3(100, 0, 0);
        
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
            dist = 99999999;
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

}
