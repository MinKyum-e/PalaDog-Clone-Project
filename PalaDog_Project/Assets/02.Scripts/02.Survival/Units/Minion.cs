using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Minion: MonoBehaviour
{
    public Actor actor;
    Actions action;
    PoolManager poolManager;
    GameObject atkTarget;
    GameObject skillTarget;
    GameObject enemyBase;
    public SkillInfo skill_info;
    public int cost;
    
    

    private void Awake()
    {
        actor = GetComponent<Actor>();
        poolManager = GameObject.FindGameObjectWithTag("EnemyPool").GetComponent<PoolManager>();
        enemyBase = GameObject.Find("EnemyBase");
        action = GetComponent<Actions>();
        
        
    }



    private void OnEnable()
    {
        setStatus();
        if (Parser.skill_info_dict.TryGetValue(actor.cur_status.skill[0], out skill_info) == false)
        {
            actor.can_use_skill = false;
        }
        else
        {
            actor.can_use_skill = true;
        }

        StartCoroutine(NormalAttack());
        
        //GameManager.Instance.UpdateCost(info.cost); //cost 추가
    }


    private void Update()
    {
        if (actor.cur_status.HP <= 0)
        {
            Die();
        }
        else
        {

            if (actor.can_use_skill) 
            {
                //일반 유닛은 스킬 1개
                skillTarget = setAttackTarget(skill_info.cast_range);
                if (skillTarget != null)
                {
                    print("스킬사용!!!!!!!" + actor.cur_status.skill[0]);
                    action.PlaySkill(actor.cur_status.skill[0], skillTarget);
                    StartCoroutine(BoolTimer(skill_info.cool_time));
                    
                }

            }
        }
    }

    void FixedUpdate()
    {
        if (actor.isWalk)
        {
            if (actor.animator != null)
                actor.animator.SetBool("isWalk", actor.isWalk); 
            action.Move();
        }

    }
    public IEnumerator BoolTimer(int time)
    {
        actor.can_use_skill = false;
        yield return new WaitForSeconds(time);
        actor.can_use_skill = true;
    }

    public void setStatus()
    {
        try
        {
            actor.status = Parser.minion_status_dict[actor.ID].common;
            actor.cur_status = Parser.minion_status_dict[actor.ID].common;
            cost = Parser.minion_status_dict[actor.ID].cost;
        }
        catch { Debug.Log("status Setting Error Minion"); }
    }

    public void Die()
    {

        actor.isWalk = false;
        actor.spriteRenderer.color = Color.white;
        atkTarget = null;
        gameObject.SetActive(false);
        gameObject.transform.position = new Vector3(100, 0, 0);
        StopCoroutine(NormalAttack());
        GameManager.Instance.UpdateCost(-cost);
    }


    public GameObject setAttackTarget(float range)
    {
        //기존 타켓이 존재하면 그냥 return
        if ( atkTarget != null && atkTarget.gameObject.activeSelf && Utils.DistanceToTarget(atkTarget.transform.position, transform.position) <= range)
        {
            return atkTarget;
        }

        GameObject target = null;

        float dist;
        try
        {
            dist = Utils.DistanceToTarget(enemyBase.transform.position, transform.position);
            if (dist <= range)
                target = enemyBase;
        }
        catch
        {
            print("SetAttackTarget: maintarget missing set diff 99999");
            dist = 9999999;
        }

        foreach (List<GameObject> units in poolManager.pools)
        {
            foreach (GameObject u in units)
            {
                if (!u.activeSelf) { continue; }
                float tmp_dist = Utils.DistanceToTarget(u.transform.position, transform.position);
                if (tmp_dist < dist && tmp_dist <= range)
                {
                    dist = tmp_dist;
                    target = u;
                }
            }
        }
        if (target != null)
        {
            atkTarget = target;
            return atkTarget;
        }
        else { return null; }
    }

    public IEnumerator NormalAttack()
    {
        while (true)
        {
            //attack
            atkTarget = setAttackTarget(actor.cur_status.atkRange);
            if (atkTarget != null && actor.can_attack && atkTarget.activeSelf)
            {

                    actor.isWalk = false;
                    Color c = atkTarget.GetComponent<SpriteRenderer>().color;
                    atkTarget.GetComponent<SpriteRenderer>().color = Color.red;//추후 애니메이션 적용
                                                                               //공격속성무시 확인
                    if (Buff.CheckAttackIgnore(atkTarget.GetComponent<Actor>().cur_buff, actor.cur_status.job))
                        atkTarget.GetComponent<Actions>().Hit(actor.cur_status.atk);
                    yield return new WaitForSeconds(actor.cur_status.atkSpeed);
                    atkTarget.GetComponent<SpriteRenderer>().color = c;

            }
            else if(actor.can_attack)
            {
                actor.isWalk = true;
            }
            yield return null;
        }
    }
}
