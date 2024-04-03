using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    public int ID;
    public UnitInfo unitInfo;
    
    public int curHP;
    public Vector2 moveDir;
    public bool isWalk = false;

    public Rigidbody2D rigid;
    public SpriteRenderer spriteRenderer; 
    public Animator animator;
    public float ray_max;
    //추상
    public abstract void Die();
    public abstract void setStatus();


    // 공통함수
    public void Move()
    {
        animator.SetBool("isWalk", isWalk);
        if(isWalk)
        {
            Vector3 nextPos = rigid.position + moveDir * unitInfo.moveSpeed * Time.fixedDeltaTime;
            rigid.MovePosition(nextPos);
            rigid.velocity = Vector2.zero;
        }
        
    }
    public void Hit(int Damage)
    {
        curHP -= Damage;
    }

    public void setWalk(bool x)
    {
        isWalk = x;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Wall")
        {
            moveDir = Vector2.zero;
        }
            
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Wall")
        {
            moveDir = Vector2.zero;
        }
    }


}

