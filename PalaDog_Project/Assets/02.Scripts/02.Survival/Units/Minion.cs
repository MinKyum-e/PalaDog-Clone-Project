using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Minion: Monster
{
    public MinionInfo info;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        info = new MinionInfo();
    }
    private void OnEnable()
    {
        
        setStatus();
        curHP = info.HP;
        moveDir = Vector2.right;
        StartCoroutine(NormalAttack("EnemyMainTarget", "Enemy"));
        isWalk = true;
        //GameManager.Instance.UpdateCost(info.cost); //cost 추가
    }


    public override void setStatus()
    {
        try
        {
            info = Parser.minion_info_dict[ID];
            unitInfo = info;
            monster_info = info;
        }
        catch { Debug.Log("status Setting Error Minion"); }
    }
    private void Update()
    {
        if (curHP <= 0)
        {
            Die();
        }
    }

    void FixedUpdate()
    {
        if (isWalk)
        {
            Move();
        }

    }
    public override void Die()
    {
        isWalk = false;
        atkTarget = null;
        gameObject.SetActive(false);
        gameObject.transform.position = new Vector3(100, 0, 0);
        GameManager.Instance.cur_cost -= info.cost;
    }
    public override Unit setAttackTarget(string main_target_tag, string target_tag)
    {
        //기존 타켓이 존재하면 그냥 return
        if ( atkTarget != null && atkTarget.gameObject.activeSelf && DistanceToTarget(atkTarget.transform.position, transform.position) <=  info.atkRange)
        {
            return atkTarget;
        }

        GameObject target = null;
        GameObject main_target = GameObject.FindGameObjectWithTag(main_target_tag);

        float dist;
        try
        {
            dist = DistanceToTarget(main_target.transform.position, transform.position);
            if (dist <= info.atkRange)
                target = main_target;
        }
        catch
        {
            print("SetAttackTarget: maintarget missing set diff 99999");
            dist = 9999999;
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
