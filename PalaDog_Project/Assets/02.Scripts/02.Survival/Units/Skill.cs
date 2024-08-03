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
    private Actor actor;
    private Actions action;

    private void Awake()
    {
       /* _instance = this;*/
        actor = GetComponent<Actor>();
        action = GetComponent<Actions>();
    }

    public Actor FirstSearchingTarget(SkillName index)
    {
        SkillEntry s = Parser.skill_table_dict[(int)index];

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
                    return u.GetComponent<Actor>();
                }
            }
        }

        return null;
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
            case TargetSearchType.Ora:
                return OraTargets(s);
            case TargetSearchType.TargetFar:
                return farTargets(s);
            default:
                return null;
        }
    }



    public bool UseSkill(int skill_slot_idx, SkillName skill_name)//스킬이름, 타겟, 스킬 슬룻 인덱스
    {
        
        SkillEntry s = Parser.skill_table_dict[(int)skill_name];
        List<Actor> targets = SearchingTargets(skill_name);
        if(targets.Count == 0 && actor.skills[skill_slot_idx].target != null)
        {
            targets.Add(actor.skills[skill_slot_idx].target);
        }

        print(targets.Count);
        print(skill_name);
        //특수 제작 스킬 우선 사용해보고 없으면 일반적인 버프 스킬 수행하기
        if(! UniqueSkill(skill_slot_idx,skill_name, targets))
        {
            switch (s.base_stat)
            {
                case BaseStat.None:
                    NoneTypeBuff(s, targets);
                    break;
                case BaseStat.Atk:
                    AttackTypeBuff(s, targets);
                    break;
                case BaseStat.Magic:
                    StartSkillEffect(s);
                    
                    break;
                default:

                    return false;
            }
        }

        switch (skill_name)
        {
            case SkillName.HammerAttack:
                SoundManager.Instance.PlaySFX(SoundManager.SFX_CLIP.Hammer_NormalAttack);
                break;
            case SkillName.LegendHammerAttack:
                SoundManager.Instance.PlaySFX(SoundManager.SFX_CLIP.Hammer_Hero_Skill);
                break;
            case SkillName.Magic:
                SoundManager.Instance.PlaySFX(SoundManager.SFX_CLIP.Wizard_Elite_Skill);
                break;
            case SkillName.EpicMagic:
                SoundManager.Instance.PlaySFX(SoundManager.SFX_CLIP.Wizard_HeroSkill);
                break;

            case SkillName.LifeDrain:
                SoundManager.Instance.PlaySFX(SoundManager.SFX_CLIP.Bat_Boss_Skill2);
                break;
        }

        return true;
    }

    public void StartSkillEffect(SkillEntry s)
    {
        
        actor.skill_effect.gameObject.SetActive(true);
        actor.skill_effect.setInfo(s);
        actor.skill_effect.StartSkill();
    }

    public bool UniqueSkill(int skill_slot_idx, SkillName skill_name, List<Actor> targets)
    {
        switch (skill_name)
        {
            case SkillName.HeroArrow:
                HeroArrow(skill_slot_idx);
                break;
            case SkillName.LifeDrain:
                StartCoroutine(Co_LifeDrain(skill_slot_idx));
                break;
            case SkillName.PoisonFog:
                if (targets.Count != 0 )
                    PoisonFog(skill_slot_idx, targets[0]);
                break;
            default:
                return false;
                
        }
        return true;
    }

    private void PoisonFog(int skill_slot_idx, Actor target)
    {
        if(target!=null)
        {
            PoisonFog fog = gameObject.GetComponent<PoisonFog>();
            fog.enabled = true;
            fog.obj.SetActive(true);
            fog.SetInfo(actor.skills[skill_slot_idx].entry);
            fog.obj.transform.position = target.transform.position;
        }
       
    }


    private void HeroArrow(int skill_slot_idx)
    {
        SkillEntry s = Parser.skill_table_dict[(int)SkillName.HeroArrow];

            var ret = ArrowPool.Instance.Shot(null, transform.position, actor.cur_status.atk * s.DMGCoeff, actor.cur_status.atkSpeed, 100, projectiles.Arrow, true);

    }

    private IEnumerator Co_LifeDrain(int skill_slot_idx)
    {
        SkillEntry s = Parser.skill_table_dict[(int)SkillName.LifeDrain];
        while (! actor.cur_buff.stun)
        {
            List<Actor> targets = SearchingTargets(SkillName.LifeDrain);
            if (targets.Count > 0)
            {
                for (int i = 0; i < targets.Count; i++)
                {
                    float damage = actor.cur_status.atk * s.DMGCoeff;
                    targets[i].gameObject.GetComponent<Actions>().Hit(damage, Chr_job.magic);
                    actor.cur_status.HP = Mathf.Clamp(actor.cur_status.HP + damage, 0, actor.status.HP);
                }
            }
            yield return new WaitForSeconds(1);
        }
        action.StartSkillTimer(skill_slot_idx);
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

    public void NoneTypeBuff(BuffName name, float value, float duration, List<GameObject> target_list)
    {
        if (target_list.Count > 0)
        {
            for (int i = 0; i < target_list.Count; i++)
            {
                BuffSystem buffSystem = target_list[i].transform.Find("Buff").GetComponent<BuffSystem>();

                buffSystem.Apply(name, value, duration, 0);
            }
        }
    }


    public void NoneTypeBuff(SkillEntry s, List<Actor> target_list)
    {
        if(target_list.Count > 0)
        {
            for (int i = 0; i < target_list.Count; i++)
            {
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
                target_list[i].gameObject.GetComponent<Actions>().Hit((int)((float)actor.cur_status.atk * s.DMGCoeff), actor.cur_status.job);
                for (int j = 0; j < s.skill_effects.Length; j++)
                {
                    if (s.skill_effects[j].index == -1) continue;
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

    public List<Actor> farTargets(SkillEntry s)
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
        List<Candidate> sorted_candidates = candidates.OrderByDescending(x => x.dist).ToList();
        
        for (int i = 0; i < sorted_candidates.Count; i++)
        {
            if (s.target_search_num <= i) break;
            result.Add(sorted_candidates[i].target.GetComponent<Actor>());
        }
        return result;

    }



    public List<Actor> OraTargets(SkillEntry s)
    {
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
                if (tmp_dist <= Player.Instance.auraCollider.bounds.extents.x)
                {
                    result.Add(u.GetComponent<Actor>());
                }
            }
        }
        return result;
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
                if (tmp_dist <= s.target_search_num)
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
