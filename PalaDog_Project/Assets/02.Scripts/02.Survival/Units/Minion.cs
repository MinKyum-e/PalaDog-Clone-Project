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
    GameObject enemyBase;
    public int cost;

    private void Awake()
    {
        actor = GetComponent<Actor>();
        poolManager = GameObject.FindGameObjectWithTag("EnemyPool").GetComponent<PoolManager>();
        enemyBase = GameObject.FindGameObjectWithTag("EnemyMainTarget");
        action = GetComponent<Actions>();
    }
    private void OnEnable()
    {
        
        setStatus();
        actor.cur_status.HP = actor.status.HP;
        actor.cur_status.moveDir = Vector2.right;
        StartCoroutine(NormalAttack());
        actor.isWalk = true;
        //GameManager.Instance.UpdateCost(info.cost); //cost 추가
    }
    private void Update()
    {
        if (actor.cur_status.HP <= 0)
        {
            Die();
        }
    }

    void FixedUpdate()
    {
        if (actor.isWalk)
        {
            action.Move();
        }

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
        atkTarget = null;
        gameObject.SetActive(false);
        gameObject.transform.position = new Vector3(100, 0, 0);
        GameManager.Instance.UpdateCost(-cost);
    }
    public GameObject setAttackTarget()
    {
        //기존 타켓이 존재하면 그냥 return
        if ( atkTarget != null && atkTarget.gameObject.activeSelf && Utils.DistanceToTarget(atkTarget.transform.position, transform.position) <=  actor.cur_status.atkRange)
        {
            return atkTarget;
        }

        GameObject target = null;

        float dist;
        try
        {
            dist = Utils.DistanceToTarget(enemyBase.transform.position, transform.position);
            if (dist <= actor.cur_status.atkRange)
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
                if (tmp_dist < dist && tmp_dist <= actor.cur_status.atkRange)
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
            //타켓지정
            atkTarget = setAttackTarget();

            //attack
            if (atkTarget != null)
            {
                actor.isWalk = false;
                atkTarget.GetComponent<SpriteRenderer>().color = Color.red;//추후 애니메이션 적용
                atkTarget.GetComponent<Actions>().Hit(actor.cur_status.atk);
                yield return new WaitForSeconds(actor.cur_status.atkSpeed);
                atkTarget.GetComponent<SpriteRenderer>().color = Color.white;
            }
            else
            {
                actor.isWalk = true;
            }
            yield return null;
        }
    }
}
