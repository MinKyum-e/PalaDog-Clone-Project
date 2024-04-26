
using UnityEngine;

public class Actions: MonoBehaviour
{
    Actor actor;
    private void Awake()
    {
        actor = GetComponent<Actor>();    
    }

    public void Move()
    {
        actor.animator.SetBool("isWalk", actor.isWalk);
        if (actor.isWalk)
        {
            Vector3 nextPos = actor.rigid.position + actor.cur_status.moveDir * actor.cur_status.moveSpeed * Time.fixedDeltaTime;
            actor.rigid.MovePosition(nextPos);
            actor.rigid.velocity = Vector2.zero;
        }

    }
    public void Hit(int Damage)
    {
        actor.cur_status.HP -= Damage;
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

    public bool Skill(int skill_idx, GameObject target)
    {
        //TODO
        switch((SkillName)skill_idx)
        {
            case SkillName.A:
                break;
            default:
                return false;
        }
        return true;
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
