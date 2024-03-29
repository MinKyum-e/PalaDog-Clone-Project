using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Minion: Monster
{
    public int cost;
    public int skill;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void OnEnable()
    {
        setStatus();
        curHP = HP;
        moveDir = Vector2.right;
        StartCoroutine(NormalAttack("EnemyMainTarget", "Enemy"));
        isWalk = true;
        GameManager.Instance.UpdateCost(cost); //cost 추가
    }


    public override void setStatus()
    {
        List<Dictionary<string, object>> Minion_status_list = Parser.data_MinionTable;
        try
        {
            unitName = Minion_status_list[ID]["Unit_GameName"].ToString();
            group = (int)Minion_status_list[ID]["Unit_Group"];
            cost = (int)Minion_status_list[ID]["Unit_Cost"];
            HP = (int)Minion_status_list[ID]["Unit_HP"];
            atk = (int)Minion_status_list[ID]["Unit_Atk"];
            atkRange = float.Parse(Minion_status_list[ID]["Unit_AtkRange"].ToString());
            atkSpeed = float.Parse(Minion_status_list[ID]["Unit_AtkSpeed"].ToString());
            moveSpeed = float.Parse(Minion_status_list[ID]["Unit_MoveSpeed"].ToString());
            skill = (int)Minion_status_list[ID]["Unit_Skill"];

        }
        catch { Debug.Log("status Setting Error"); }
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
        GameManager.Instance.cur_cost -= cost;
    }
    public override Unit setAttackTarget(string main_target_tag, string target_tag)
    {
        //기존 타켓이 존재하면 그냥 return
        if (atkTarget != null && DistanceToTarget(atkTarget.transform.position, transform.position) <= atkRange)
        {
            return atkTarget;
        }

        GameObject target = null;
        GameObject main_target = GameObject.FindGameObjectWithTag(main_target_tag);

        float dist;
        try
        {
            dist = DistanceToTarget(main_target.transform.position, transform.position);
            if (dist <= atkRange)
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
            if (tmp_dist < dist && tmp_dist <= atkRange)
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
