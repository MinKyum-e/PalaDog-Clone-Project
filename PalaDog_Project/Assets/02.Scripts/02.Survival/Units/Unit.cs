using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    public Actor actor;
    public Actions action;
    public Skill skill;
    public UnitType atkType;

    private void OnEnable()
    {

        actor = GetComponent<Actor>();
        action = GetComponent<Actions>();
        atkType = (gameObject.tag == "Enemy") ? UnitType.Minion : UnitType.Enemy;
        setStatus();

        actor.cur_status.HP = actor.status.HP;
        actor.atkTarget = null;
        actor.is_faint = false;
        actor.isDie = false;
        actor.can_action = true;
        skill = GetComponent<Skill>();

        /*        StartCoroutine(NormalAttack());*/

        //GameManager.Instance.UpdateCost(info.cost); //cost 추가
        for (int i = 0; i < 3; i++)
        {
            SkillEntry entry;
            if (Parser.skill_table_dict.TryGetValue(actor.cur_status.skill[i], out entry))
            {
                actor.skills[i].entry = entry;
                actor.skills[i].can_use_skill = (entry.act == SkillAct.P);
            }
            else
            {
                actor.skills[i].can_use_skill = false;
            }

        }
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

            //스킬 우선 실행
            for (int i = 0; i < 3; i++)
            {
                if (actor.skills[i].entry.act == SkillAct.A)//패시브만 실행되야함
                    continue;

                if (actor.can_action && actor.skills[i].can_use_skill)
                {
                    //타켓 지정 가능?
                    if (actor.skills[i].entry.need_searching)
                    {
                        //스킬 고유 타겟 지정 방식 선택
                        List<Actor> targets = skill.SearchingTargets((SkillName)actor.skills[i].entry.index);

                        if(targets.Count >0)
                        {
                            actor.skills[i].target =targets[0];
                            print("!!!!!!!!!!!!!!!!!!!!!" + actor.skills[i].target.name);
                            if (actor.can_action && actor.skills[i].target != null && actor.skills[i].target.GetComponent<Actor>().isDie == false)
                            {
                                actor.can_action = false;
                                actor.skills[i].can_use_skill = false;
                                actor.animator.SetTrigger("Skill" + i);

                                print((SkillName)actor.skills[i].entry.index);
                            }
                        }
                        
                    }
                    else
                    {
                        actor.can_action = false;
                        actor.skills[i].can_use_skill = false;
                        actor.animator.SetTrigger("Skill" + i);
                        print((SkillName)actor.skills[i].entry.index);
                    }

                }
            }

            

            //일반공격(차순)
            if (actor.can_action)
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
