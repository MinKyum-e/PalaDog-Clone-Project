using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    public UnitType Type;
    public int ID;
    public string unit_name;
    public int group;
    public int cost;
    public int HP;
    public int curHP;
    public int atk;
    public float atkRange;
    public float atkSpeed;
    public float moveSpeed;
    public int skill;
    public Vector2 moveDir;
    public bool isWalk = false;

    public Rigidbody2D rigid;
    public SpriteRenderer spriteRenderer; 
    public Animator animator;
    //추상
    public abstract void Die();


    // 공통함수
    public void Move()
    {
        animator.SetBool("isWalk", isWalk);
        Vector3 nextPos = rigid.position + moveDir * moveSpeed * Time.fixedDeltaTime;
        rigid.MovePosition(nextPos);
        rigid.velocity = Vector2.zero;
    }
    public void Hit(int Damage)
    {
        curHP -= Damage;
    }
    public void setStatus()
    {
        if(Type == UnitType.Player)
        {
            ID = 99;
            unit_name = "player";
            HP = 100;
            moveSpeed = 10;
        }
        else if(Type == UnitType.EnemyBase)
        {
            ID = 98;
            unit_name = "enemyBase";
            HP = 500;
            moveSpeed = 0;
        }
        else if(Type == UnitType.Enemy || Type == UnitType.Boss)
        {
            List<Dictionary<string, object>> enemy_status_list = Parser.Instance.data_EnemyTable;
            try
            {
                unit_name = enemy_status_list[ID]["Name"].ToString();
                HP = (int)enemy_status_list[ID]["HP"];
                atk = (int)enemy_status_list[ID]["Damage"];
                atkRange = float.Parse(enemy_status_list[ID]["Range"].ToString());
                moveSpeed = float.Parse(enemy_status_list[ID]["MoveSpeed"].ToString());
            }
            catch { Debug.Log("status Setting Error"); }
        }
        else if(Type == UnitType.Minion)
        {
            List<Dictionary<string, object>> Minion_status_list = Parser.Instance.data_MinionTable;
            try
            {
                unit_name = Minion_status_list[ID]["Unit_GameName"].ToString();
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
    }
    public void setWalk(bool x)
    {
        isWalk = x;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Wall")
            moveDir = Vector2.zero;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Wall")
            moveDir = Vector2.zero;
    }

}

