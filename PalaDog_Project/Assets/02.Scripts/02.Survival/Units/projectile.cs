using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using System.ComponentModel.Design;

public class projectile : MonoBehaviour
{

    float atk;
    float atk_speed;
    float range;
    public float atk_speed_base= 10f;
    GameObject target;
    bool can_attack = true;
    float start_x;
    bool hero_skill;


    Rigidbody2D rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    public void SetInfo(GameObject target, float atk, float atk_speed, float range, bool hero_skill)
    {
        this.target = target;
        this.atk = atk;
        this.atk_speed = atk_speed;
        this.range  = range;
        can_attack = true;
        start_x = transform.position.x;
        this.hero_skill = hero_skill;
    }

    private void Update()
    {
        if(GameManager.Instance.state == GameState.GAME_STAGE_CLEAR)
        {
            gameObject.SetActive(false);
        }
        if(Mathf.Abs(start_x - transform.position.x)  > range)
        {
            gameObject.SetActive(false);
        }
        if(!can_attack)
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

        /*        if (can_attack)
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
                }*/


        if (can_attack)
        {
            if (collision.gameObject.tag == "Enemy" && !collision_unit.actor.isDie)
            {
                if(!hero_skill)
                    can_attack = false;
                collision_unit.Hit(atk, Chr_job.projectile);

            }
        }
    }

}
