using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

//범위, 데미지, 캐스팅시간, 쿨타임 알고있다고 치고
public class Skill:MonoBehaviour
{
    private struct Candidate
    {
        public float dist;
        public GameObject target;
    }
    /*  private static SkillManager _instance;
      public static SkillManager Instance { get { return _instance; } }*/
    private PoolManager skillPool;
    private Actor actor;

    private void Awake()
    {
       /* _instance = this;*/
        skillPool = GameObject.FindGameObjectWithTag("SkillPool").GetComponent<PoolManager>();
        actor = GetComponent<Actor>();
    }

    public List<Actor> SearchingTargets(SkillName index)
    {
        SkillEntry s = Parser.skill_table_dict[(int)index];
        switch (s.target_search_type)
        {
            case TargetSearchType.Target:
                return NearTargets(s);
            case TargetSearchType.Range:
                return RangeTargets(s);
            default:
                return null;
        }
    }

    public bool UseSkill(SkillName index, Actor target)//스킬 인덱스, 시전자, 타겟
    {
        SkillEntry s = Parser.skill_table_dict[(int)index];
        List<Actor> targets = SearchingTargets(index);
        switch (s.base_stat)
        {
            case BaseStat.None:
                NoneTypeBuff(s, targets);
                break;
            case BaseStat.Atk:
                AttackTypeBuff(s, targets);
                break;
            default:

                return false;
        }
        return true;
    }

/*    public void SelfHealing(SkillEntry s)
    {
        List<Actor> target_list = NearTargets(s);
        print("자가치유!!! : " + target_list[0].name);
        //list 첫 유닛한태 버프 적용하기
        if(target_list.Count > 0)
        {
            BuffSystem buffSystem = target_list[0].transform.Find("Buff").GetComponent<BuffSystem>();

            for (int i = 0; i < s.skill_effects.Length; i++)
            {
                if (s.skill_effects[i].index == -1) continue;
                buffSystem.Apply((BuffName)s.skill_effects[i].index, s.skill_effects[i].value, s.skill_effects[i].duration, 0);
            }
        }
        
    }*/

    public void NoneTypeBuff(SkillEntry s, List<Actor> target_list)
    {
        if(target_list.Count > 0)
        {
            for (int i = 0; i < target_list.Count; i++)
            {

                print("방패 펼치기!! : " + target_list[i].name);
                BuffSystem buffSystem = target_list[i].transform.Find("Buff").GetComponent<BuffSystem>();
                for (int j = 0; j < s.skill_effects.Length; j++)
                {
                    if (s.skill_effects[j].index == -1) continue;
                    buffSystem.Apply((BuffName)s.skill_effects[j].index, s.skill_effects[j].value, s.skill_effects[j].duration, 0);
                }
                
                
            }
        }
        

    }

    public void AttackTypeBuff(SkillEntry s, List<Actor> target_list)
    {
        if (target_list.Count > 0)
        {
            for (int i = 0; i < target_list.Count; i++)
            {
                BuffSystem buffSystem = target_list[i].transform.Find("Buff").GetComponent<BuffSystem>();
                for (int j = 0; j < s.skill_effects.Length; j++)
                {
                    if (s.skill_effects[j].index == -1) continue;
                    target_list[i].gameObject.GetComponent<Actions>().Hit((int)((float)actor.cur_status.atk * s.DMGCoeff), actor.cur_status.job);
                    buffSystem.Apply((BuffName)s.skill_effects[j].index, s.skill_effects[j].value, s.skill_effects[j].duration, 0);
                }


            }
        }


    }



    public List<Actor> NearTargets(SkillEntry s)
    {
        List<Actor> result = new List<Actor>();
        List<Candidate> candidates = new List<Candidate>();
        PoolManager targetPool = ((s.target_type == UnitType.Enemy) ? actor.enemy_poolManager : actor.minion_poolManager);
        print(targetPool.name);
        //스킬 범위 내에 있는 애들 찾기
        foreach (List<GameObject> units in targetPool.pools)
        {
            foreach (GameObject u in units)
            {
                if (u.GetComponent<Actor>().isDie) { continue; }
                float tmp_dist = Utils.DistanceToTarget(u.transform.position, transform.position);
                if (tmp_dist <= s.searching_range)
                {
                    Candidate t = new Candidate();
                    t.dist = tmp_dist;
                    t.target = u;
                    candidates.Add(t);
                }
            }
        }

        //sort
        List<Candidate> sorted_candidates = candidates.OrderBy(x=>x.dist).ToList();

        for(int i=0;i<sorted_candidates.Count;i++)
        {
            if (s.target_search_num <= i) break;
            result.Add(sorted_candidates[i].target.GetComponent<Actor>());  
        }
        return result;

    }


    public void OraTargets()
    {

    }


    public List<Actor> RangeTargets(SkillEntry s)
    {
        List<Actor> result = new List<Actor>();

        PoolManager targetPool = ((s.target_type == UnitType.Enemy) ? actor.enemy_poolManager : actor.minion_poolManager);

        //스킬 범위 내에 있는 애들 찾기
        foreach (List<GameObject> units in targetPool.pools)
        {
            foreach (GameObject u in units)
            {
                if (u.GetComponent<Actor>().isDie) { continue; }
                float tmp_dist = Utils.DistanceToTarget(u.transform.position, transform.position);
                if (tmp_dist <= s.searching_range)
                {
                    if(actor.cur_status.moveDir.x > 0  && (u.transform.position.x - actor.transform.position.x) <= 0)
                        continue;
                    if (actor.cur_status.moveDir.x < 0 && (u.transform.position.x - actor.transform.position.x) >= 0)
                        continue;
             
                    result.Add(u.GetComponent<Actor>());   
                }
            }
        }
        return result;
    }




}
