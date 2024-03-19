using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    public UnitType Type;
    public int ID;
    public string Name;
    public int HP;
    public int curHP;
    public int Damage;
    public float Range;
    public float MoveSpeed;
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
        Vector3 nextPos = rigid.position + moveDir * MoveSpeed * Time.fixedDeltaTime;
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
            Name = "player";
            HP = 100;
            MoveSpeed = 10;
        }
        else if(Type == UnitType.EnemyBase)
        {
            ID = 98;
            Name = "enemyBase";
            HP = 500;
            MoveSpeed = 0;
        }
        else if(Type == UnitType.Enemy || Type == UnitType.Boss)
        {
            List<Dictionary<string, object>> enemy_status_list = GameManager.Instance.parser.data_EnemyTable;
            try
            {
                Name = enemy_status_list[ID]["Name"].ToString();
                HP = (int)enemy_status_list[ID]["HP"];
                Damage = (int)enemy_status_list[ID]["Damage"];
                Range = float.Parse(enemy_status_list[ID]["Range"].ToString());
                MoveSpeed = float.Parse(enemy_status_list[ID]["MoveSpeed"].ToString());
            }
            catch { Debug.Log("status Setting Error"); }
        }
        else if(Type == UnitType.Minion)
        {
            List<Dictionary<string, object>> Minion_status_list = GameManager.Instance.parser.data_MinionTable;
            try
            {
                Name = Minion_status_list[ID]["Name"].ToString();
                HP = (int)Minion_status_list[ID]["HP"];
                Damage = (int)Minion_status_list[ID]["Damage"];
                Range = float.Parse(Minion_status_list[ID]["Range"].ToString());
                MoveSpeed = float.Parse(Minion_status_list[ID]["MoveSpeed"].ToString());
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

