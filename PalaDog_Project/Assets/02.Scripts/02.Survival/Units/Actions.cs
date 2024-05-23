
using System;
using System.Collections;
using UnityEngine;


public class Actions: MonoBehaviour
{
    Actor actor;
    Skill skill;



    private void Awake()
    {
        actor = GetComponent<Actor>();    
        skill = GetComponent<Skill>();
    }

    public void StartUnitAction()
    {
        actor.can_action = false;
    }

    public int CalDagamge()
    {
        //버프
        //디버프
        return actor.cur_status.atk;
    }

    public void NormalAttack()
    {
        if(actor.atkTarget != null && actor.atkTarget.activeSelf)/* && Utils.DistanceToTarget(actor.transform.position, actor.atkTarget.transform.position) <= actor.cur_status.atkRange)*/
        {
            actor.atkTarget.GetComponent<Actions>().Hit(CalDagamge());
            
        }
    }

    public void EndUnitAction()
    {
        actor.can_action = true;
    }

    public IEnumerator SkillTimer(int skill_slot_idx)
    {
        yield return new WaitForSeconds(actor.skills[skill_slot_idx].entry.coolTime);
        actor.skills[skill_slot_idx].can_use_skill = true;
    }



    public void Move()
    {
        Vector3 nextPos = actor.rigid.position + actor.cur_status.moveDir * actor.cur_status.moveSpeed * Time.fixedDeltaTime;
        actor.rigid.MovePosition(nextPos);
        actor.rigid.velocity = Vector2.zero;

    }
    public void Hit(int Damage)
    {
        actor.spriteRenderer.color = Color.red;
        actor.cur_status.HP -= Damage;
        StartCoroutine(WaitHit());
    }
    private IEnumerator WaitHit()
    {
        yield return new WaitForSeconds(0.05f);
        actor.spriteRenderer.color = Color.white;
    }

    public void SetMoveDir(string main_target_tag)
    {
        GameObject main_target = GameObject.FindGameObjectWithTag(main_target_tag);
        try
        {
            actor.cur_status.moveDir = new Vector3((main_target.transform.position.x - transform.position.x), 0, 0).normalized;
        }
        catch
        {

            print("SetAttackTarget: maintarget missing");

        }
    }

    public bool PlaySkill(int skill_slot_idx)
    {
        //TODO
        StartCoroutine(SkillTimer(skill_slot_idx));
        return skill.UseSkill((SkillName)actor.cur_status.skill[skill_slot_idx], actor, actor.skills[skill_slot_idx].target);
    }

    public bool AddBuff(int buff_idx)
    {
        //TODO
        switch((BuffName)buff_idx)
        {

            default:
                return false;
        }
        return true;
    }

/*    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Wall")
        {
            actor.cur_status.moveDir = Vector2.zero;
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Wall")
        {
            actor.cur_status.moveDir = Vector2.zero;
        }
    }*/


}
