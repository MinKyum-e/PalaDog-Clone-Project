
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Actions: MonoBehaviour
{
    Actor actor;

    public bool can_action;
    
    private void Awake()
    {
        actor = GetComponent<Actor>();    
        
    }

    public void StartUnitAction()
    {
        actor.isWalk = false;
        actor.can_search = false;
        actor.can_attack = false;
    }

    public int CalDagamge()
    {
        //버프
        //디버프
        return actor.cur_status.atk;
    }

    public void NormalAttack()
    {
        if(actor.atkTarget != null && actor.atkTarget.activeSelf && Utils.DistanceToTarget(actor.transform.position, actor.atkTarget.transform.position) <= actor.cur_status.atkRange)
        {
            actor.atkTarget.GetComponent<Actions>().Hit(CalDagamge());
            
        }
    }

    public void EndUnitAction()
    {
        can_action = true;
        actor.can_search = true;
        actor.can_attack = true;
    }



    public void Move()
    {
/*        if(actor.animator != null) 
            actor.animator.SetBool("isWalk", actor.isWalk);*/
        if (actor.isWalk)
        {
            Vector3 nextPos = actor.rigid.position + actor.cur_status.moveDir * actor.cur_status.moveSpeed * Time.fixedDeltaTime;
            actor.rigid.MovePosition(nextPos);
            actor.rigid.velocity = Vector2.zero;
        }

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

    public bool PlaySkill(int skill_idx, GameObject target)
    {
        //TODO
        print(target.name);
        return SkillManager.Instance.UseSkill((SkillName)skill_idx, actor, target);
    }

    public bool AddBuff(int buff_idx)
    {
        //TODO
        switch((BuffName)buff_idx)
        {
            case BuffName.A:
                break;
            default:
                return false;
        }
        return true;
    }

    private void OnTriggerStay2D(Collider2D collision)
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
    }
}
