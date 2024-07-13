using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Minion: Unit
{
    GameObject enemyBase;
   
    public int cost;

    private void Awake()
    {
        enemyBase = GameObject.Find("EnemyBase");
    }

    public override void setStatus()
    {
        try
        {
            actor.status = Parser.minion_status_dict[actor.ID].common;
            actor.cur_status = Parser.minion_status_dict[actor.ID].common;
            cost = Parser.minion_status_dict[actor.ID].cost;
        }
        catch { Debug.Log("status Setting Error Minion"); }
    }

    public override void Die()
    {
        actor.spriteRenderer.color = Color.white;
        gameObject.SetActive(false);
        gameObject.transform.position = new Vector3(100, 0, gameObject.transform.position.z);
        GameManager.Instance.cur_cost -= cost;
        GameManager.Instance.DeleteHeroUnit((MinionUnitIndex)actor.ID);
    }


    public override GameObject setAttackTarget(GameObject cur_target, float range, UnitType unitType)
    {

        
        //기존 타켓이 존재하면 그냥 return
        if (cur_target != null && ! cur_target.GetComponent<Actor>().isDie && Utils.DistanceToTarget(cur_target.transform.position, transform.position) <= range)
        {
            return cur_target;
        }

        GameObject target = null;

        float dist;
        try
        {
            dist = Utils.DistanceToTarget(enemyBase.transform.position, transform.position);
            if (dist <= range && !enemyBase.GetComponent<Actor>().isDie)
                target = enemyBase;
        }
        catch
        {
            print("SetAttackTarget: maintarget missing set diff 99999");
            dist = 9999999;
        }

        PoolManager targetPool = ((unitType == UnitType.Enemy) ? actor.enemy_poolManager : actor.minion_poolManager);
        foreach (List<GameObject> units in targetPool.pools)
        {
            foreach (GameObject u in units)
            {
                if (u.GetComponent<Actor>().isDie) { continue; }
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
            cur_target = target;
            return cur_target;
        }
        else { return null; }

       
    }

}
