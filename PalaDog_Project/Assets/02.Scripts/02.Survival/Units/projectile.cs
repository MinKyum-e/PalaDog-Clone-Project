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
    bool can_attack = true;


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
        can_attack = true;
    }

    private void Update()
    {
        if(!can_attack || (target != null && !target.activeSelf))
        {
            transform.position = new Vector3(-100, 0, 0);
            gameObject.SetActive(false);
        }
    }

    void FixedUpdate()
    {
        rigid.velocity = Vector2.right * atk_speed * atk_speed_base;
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var collision_unit = collision.gameObject.GetComponent<Actions>();

        if (can_attack)
        {
            if (collision.gameObject.tag == "Enemy")
            {
                if (target != null)
                {
                    var target_unit = target.gameObject.GetComponent<Actions>();
                    if (collision.gameObject == target || (target_unit.actor.isDie && !collision_unit.actor.isDie))
                    {
                        if (!target_unit.actor.isDie)
                        {
                            can_attack = false;
                            target_unit.Hit(atk, Chr_job.projectile);
                            
                        }
                    }
                }
                else
                {
                    if (!collision_unit.actor.isDie)
                    {
                        can_attack = false;
                        collision_unit.Hit(atk, Chr_job.projectile);
                    }
                }
            }
        }
        
        
    }

}
