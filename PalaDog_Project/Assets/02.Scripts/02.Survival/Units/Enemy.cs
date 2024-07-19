using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Enemy: Unit
{
/*    Actor actor;
  *//*  Actions action;*//*
    PoolManager poolManager;*/
    protected GameObject player;
    public int grade;
    public int gold;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        
    }

  public override void setStatus()
    {
        try
        {
            actor.status = Parser.enemy_status_dict[actor.ID].common;
            actor.cur_status = Parser.enemy_status_dict[actor.ID].common;
            gold = Parser.enemy_status_dict[actor.ID].gold;
        }
        catch { Debug.Log("status Setting Error"); }
    }



    public override GameObject setAttackTarget(GameObject cur_target, float range, UnitType unitType)
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

        PoolManager targetPool = ((unitType == UnitType.Enemy) ? actor.enemy_poolManager : actor.minion_poolManager);
        foreach (List<GameObject> units in targetPool.pools)
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


    public override void Die()
    {
        actor.spriteRenderer.color = Color.white;
        gameObject.SetActive(false);
        gameObject.transform.position = new Vector3(100, 0, gameObject.transform.position.z);
        GameManager.Instance.cur_gold +=gold;
    }

}
