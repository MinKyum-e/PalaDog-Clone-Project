using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Monster
{
    int skill;
    int grade;
    int gold;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

    }
    private void OnEnable()
    {
        setStatus();
        curHP = HP;
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
        List<Dictionary<string, object>> enemy_status_list = Parser.data_EnemyTable;
        try
        {
            name = enemy_status_list[ID]["Monster_GameName"].ToString();
            grade = (int)enemy_status_list[ID]["Monster_Grade"];
            gold = (int)enemy_status_list[ID]["Monsater_Gold"];
            HP = (int)enemy_status_list[ID]["Monster_HP"];
            atk = (int)enemy_status_list[ID]["Monster_Atk"];
            atkRange = float.Parse(enemy_status_list[ID]["Monster_AtkRange"].ToString());
            atkSpeed = float.Parse(enemy_status_list[ID]["Monster_AtkSpeed"].ToString());
            moveSpeed = float.Parse(enemy_status_list[ID]["Monster_MoveSpeed"].ToString());
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
            if (dist <= atkRange)
            {
                return main_target.GetComponent<Unit>();
            }

        }
        catch
        {
            print("SetAttackTarget: maintarget missing set diff 99999");
            dist = 9999999;
        }

        if (atkTarget != null && DistanceToTarget(atkTarget.transform.position, transform.position) <= atkRange)
        {
            return atkTarget;
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
