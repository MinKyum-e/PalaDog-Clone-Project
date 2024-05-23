using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

//범위, 데미지, 캐스팅시간, 쿨타임 알고있다고 치고
public class Skill:MonoBehaviour
{
  /*  private static SkillManager _instance;
    public static SkillManager Instance { get { return _instance; } }*/
    private PoolManager skillPool;

    private void Awake()
    {
       /* _instance = this;*/
        skillPool = GameObject.FindGameObjectWithTag("SkillPool").GetComponent<PoolManager>();
    }

    public List<Actor> SearchingMainTarget(SkillName index, Actor actor)
    {
        switch (index)
        {
            case SkillName.KnockBack:
                return null;
            case SkillName.SelfHealing:
                return NearTargets((int)index, actor);
            default:
                return null;
        }
    }

    public bool UseSkill(SkillName index, Actor actor, GameObject target)//스킬 인덱스, 시전자, 타겟
    {
        switch (index)
        {
            case SkillName.KnockBack:
                StartCoroutine(RangeSKill((int)index, actor, target));
                break;
            case SkillName.SelfHealing:
                SelfHealing((int)index, actor);
                break;
            default:

                return false;
        }
        return true;
    }

    public void SelfHealing(int index, Actor actor)
    {
        SkillEntry s = Parser.skill_table_dict[(int)index];
        List<Actor> target_list = NearTargets((int)index, actor);
        print("자가치유!!! : " + target_list[0].name);
        //list 첫 유닛한태 버프 적용하기
        BuffSystem buffSystem = target_list[0].transform.Find("Buff").GetComponent<BuffSystem>();
        for(int i=0;i<s.skill_effects.Length;i++)
        {
            if (s.skill_effects[i].index == -1) continue;
            buffSystem.Apply((BuffName)s.skill_effects[i].index, s.skill_effects[i].value, s.skill_effects[i].duration);
        }
        
        
    }

    private struct Candidate
    {
        public float dist;
        public GameObject target;
    }
    public List<Actor> NearTargets(int index, Actor actor)
    {
        SkillEntry s = Parser.skill_table_dict[index];
        List<Actor> result = new List<Actor>();

        List<Candidate> candidates = new List<Candidate>();
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
                    Candidate t = new Candidate();
                    t.dist = tmp_dist;
                    t.target = u;
                    candidates.Add(t);
                }
            }
        }

        //sort
        List<Candidate> sorted_candidates = candidates.OrderBy(x=>x.dist).ToList();

        for(int i=0;i<s.target_search_num;i++)
        {
            result.Add(sorted_candidates[i].target.GetComponent<Actor>());  
        }
        return result;

    }

    public void RangeTargets(int index, Actor actor, GameObject main_target)
    {
        SkillInfo s = Parser.skill_info_dict[index];

    }

    public void OraTargets()
    {

    }

    public IEnumerator RangeSKill(int index, Actor actor, GameObject target)
    {
        SkillEntry s = Parser.skill_table_dict[index];
        if (target.activeSelf == true)
        {
            GameObject skill_clone = skillPool.Get(index);
            skill_clone.GetComponent<Actor>().cur_status.atk = actor.cur_status.atk;
            skill_clone.transform.localScale = new Vector3(5, 5, 1);
            skill_clone.transform.position = new Vector3(actor.transform.position.x + 0.5f, actor.transform.position.y, 0);
            BoxCollider2D clone_collider = skill_clone.GetComponent<BoxCollider2D>();
            clone_collider.enabled = true;
            yield return new WaitForSeconds(0.1f);
            clone_collider.enabled = false;
            skill_clone.transform.position = new Vector3(0, 100, 0);
            skill_clone.SetActive(false);
        }
    }



}
