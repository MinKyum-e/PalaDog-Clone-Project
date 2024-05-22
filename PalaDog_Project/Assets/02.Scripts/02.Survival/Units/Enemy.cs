using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Enemy: MonoBehaviour
{
    Actor actor;
    Actions action;
    PoolManager poolManager;
    GameObject player;
    public int grade;
    public int gold;

    private void Awake()
    {
        actor = GetComponent<Actor>();  
        poolManager = GameObject.FindGameObjectWithTag("MinionPool").GetComponent<PoolManager>();
        player = GameObject.FindGameObjectWithTag("Player");
        action = GetComponent<Actions>();
    }
    private void OnEnable()
    {
        setStatus();
        actor.cur_status.HP = actor.status.HP;
        actor.atkTarget = null;

        actor.isWalk = true;
        actor.is_faint = false;
        actor.hit_time = false;
        actor.can_search = true;
        actor.can_attack = true;
        actor.can_use_skill = false;
        actor.isDie = false;
        actor.can_action = true;
        
    }
    private void Update()
    {

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

        if (actor.atkTarget == null)
        {
            actor.isWalk = true;
        }
        if (actor.cur_status.HP <= 0)
        {
            actor.isDie = true;
            actor.can_action = false;
            actor.isWalk = false;
            actor.can_search = false;
            actor.atkTarget = null;
            actor.animator.Play("Die");
        }

        if(actor.is_faint)
        {
            actor.can_action= true;
        }

        if(actor.can_action)
        {
            if (actor.can_search)
            {
                actor.atkTarget = setAttackTarget();
            }

            if (actor.can_use_skill && actor.atkTarget != null)
            {
                if (actor.skillTarget.activeSelf && actor.skillTarget.GetComponent<Actor>().isDie == false)
                {
                    actor.can_action = false;
                    actor.animator.SetTrigger("Skill");
                }
            }
            else if (actor.can_attack && actor.atkTarget != null)
            {
                if (actor.atkTarget.activeSelf && actor.atkTarget.GetComponent<Actor>().isDie == false)
                {
                    actor.can_action = false;
                    actor.animator.SetTrigger("Attack");
                }
            }
        }
    }
    void FixedUpdate()
    {
        //걷기
        if (actor.isWalk)
        {
            action.SetMoveDir("Player");
            action.Move();
        }
    }

    public void setStatus()
    {
        try
        {
            actor.status = Parser.enemy_status_dict[actor.ID].common;
            actor.cur_status = Parser.enemy_status_dict[actor.ID].common;
            gold = Parser.enemy_status_dict[actor.ID].gold;
        }
        catch { Debug.Log("status Setting Error"); }
    }



    public GameObject setAttackTarget()
    {
       
        GameObject target = null;
        //주인공 공격할 수 있으면 공격
        float dist;
        try
        {
            dist = Utils.DistanceToTarget(player.transform.position, transform.position);
            if (dist <= actor.cur_status.atkRange)
            {
                return player;
            }
                
        }
        catch
        {
            print("SetAttackTarget: maintarget missing set diff 99999");
            dist = 9999999;
        }

        if (actor.atkTarget != null && Utils.DistanceToTarget(actor.atkTarget.transform.position, transform.position) <= actor.cur_status.atkRange)
        {
            return actor.atkTarget;
        }

        foreach(List<GameObject> units in poolManager.pools)
        {
            foreach (GameObject u in units)
            {
                if (!u.activeSelf || u.GetComponent<Actor>().isDie) { continue; }
                float tmp_dist = Utils.DistanceToTarget(u.transform.position, transform.position);
                if (tmp_dist < dist && tmp_dist <= actor.cur_status.atkRange)
                {
                    dist = tmp_dist;
                    target = u;
                }
            }
        }

        if (target != null)
        {
            actor.atkTarget = target;
            return actor.atkTarget;
        }
        else { return null; }
    }
    public void Die()
    {
        actor.spriteRenderer.color = Color.white;
        gameObject.SetActive(false);
        gameObject.transform.position = new Vector3(100, 0, 0);
        GameManager.Instance.UpdateGold(gold);
    }

}
