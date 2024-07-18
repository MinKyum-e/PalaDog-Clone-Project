using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

public class projectile : MonoBehaviour
{

    float atk;
    float atk_speed;
    public float atk_speed_base= 10f;
    GameObject target;


    Rigidbody2D rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    public void SetInfo(GameObject target, float atk, float atk_speed)
    {
        this.target = target;
        this.atk = atk;
        this.atk_speed = atk_speed; 
    }

    void FixedUpdate()
    {
        if(target != null)
        {
            rigid.velocity = Vector2.right * atk_speed * atk_speed_base;
        }
         


        if(!target.activeSelf)
        {
            transform.position = new Vector3(-100, 0, 0);
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            var collision_unit = collision.gameObject.GetComponent<Unit>();
            var target_unit = target.gameObject.GetComponent<Unit>();
            if (collision.gameObject == target )
            {
                if(!target_unit.actor.isDie)
                {
                    target_unit.action.Hit(atk, Chr_job.projectile);
                    transform.position = new Vector3(-100, 0, 0);
                    gameObject.SetActive(false);
                }
            }
            else if (target_unit.actor.isDie && !collision_unit.actor.isDie)
            {
                collision_unit.action.Hit(atk, Chr_job.projectile);
                transform.position = new Vector3(-100, 0, 0);
                gameObject.SetActive(false);

            }
        }
        

    }

/*    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Wall")
        {
            actor.cur_status.moveDir = Vector2.zero;
        }
    }*/
}
