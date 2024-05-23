using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    public Actor actor;
    public Actions action;
    public UnitType atkType;

    private void OnEnable()
    {
        actor = GetComponent<Actor>();
        action = GetComponent<Actions>();

        atkType = (gameObject.tag == "Enemy") ? UnitType.Minion : UnitType.Enemy;
        setStatus();
        if (Parser.skill_info_dict.TryGetValue(actor.cur_status.skill[0], out actor.skill_info) == false)
        {
            actor.can_use_skill = false;
        }
        else
        {
            actor.can_use_skill = true;
        }
        actor.cur_status.HP = actor.status.HP;
        actor.atkTarget = null;

        actor.is_faint = false;
        actor.isDie = false;
        actor.can_action = true;

        /*        StartCoroutine(NormalAttack());*/

        //GameManager.Instance.UpdateCost(info.cost); //cost 추가
    }


    private void Update()
    {

        if (actor.cur_status.HP <= 0)
        {
            actor.isDie = true;
            actor.can_action = false;
            actor.atkTarget = null;
            actor.animator.Play("Die");
        }

        AnimatorStateInfo stateInfo = actor.animator.GetCurrentAnimatorStateInfo(0);

        // 현재 애니메이터 상태가 타겟 애니메이션 상태와 일치하는지 확인합니다.
        if (stateInfo.IsName("Attack"))
        {
            // 타겟 애니메이션 상태의 속도를 설정합니다.
            actor.animator.speed = actor.cur_status.atkSpeed;
        }
        else
        {
            // 타겟 애니메이션 상태가 아닐 때 기본 속도로 되돌립니다.
            actor.animator.speed = 1.0f;
        }


        //필요없을수도
        if (actor.is_faint)
        {
            //기절
        }
        else
        {


        }


        if (actor.can_action)
        {
            if (actor.can_use_skill)
            {
                //타켓 지정 필요?
                if (actor.skill_info.target_check)
                {
                    //스킬 고유 타겟 지정 방식 채택
                    actor.skillTarget = setAttackTarget(actor.skillTarget, actor.skill_info.cast_range, atkType);
                    if (actor.can_use_skill && actor.skillTarget != null && actor.skillTarget.GetComponent<Actor>().isDie == false)
                    {
                        actor.can_action = false;
                        actor.can_use_skill = false;
                        actor.animator.SetTrigger("Skill");
                    }
                }
                else
                {
                    actor.can_action = false;
                    actor.animator.SetTrigger("Skill");
                }
            }
            else
            {
                actor.atkTarget = setAttackTarget(actor.atkTarget, actor.cur_status.atkRange, atkType);
                if (actor.atkTarget != null && actor.atkTarget.GetComponent<Actor>().isDie == false)
                {
                    actor.can_action = false;
                    actor.animator.SetTrigger("Attack");
                }
            }
        }

    }

    void FixedUpdate()
    {
        if (actor.can_action)
        {
           
            action.Move();
            if(gameObject.tag == "Enemy")
            {
                action.Move(); action.SetMoveDir("Player");
            }
        }


    }

    public abstract void setStatus();
    public abstract void Die();
    public abstract GameObject setAttackTarget(GameObject cur_target, float range, UnitType unitType);

}
