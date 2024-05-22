using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//범위, 데미지, 캐스팅시간, 쿨타임 알고있다고 치고
public class SkillManager:MonoBehaviour
{
    private static SkillManager _instance;
    public static SkillManager Instance { get { return _instance; } }
    private PoolManager skillPool;

    private void Awake()
    {
        _instance = this;
        skillPool = GameObject.FindGameObjectWithTag("SkillPool").GetComponent<PoolManager>();
    }

    public bool UseSkill(SkillName index, Actor actor, GameObject target)//스킬 인덱스, 시전자, 타겟
    {
        print(target.name);
        switch (index)
        {
            case SkillName.FireWall:
                StartCoroutine(FireWall((int)index, actor, target));
                break;
            case SkillName.KnockBack:
                StartCoroutine(RangeSKill((int)index, actor, target));
                break;
            default:

                return false;
        }
        return true;
    }

    public void RangeTarget(int index, Actor actor, GameObject target)
    {
        SkillInfo s = Parser.skill_info_dict[index];

    }

    public IEnumerator RangeSKill(int index, Actor actor, GameObject target)
    {
        SkillInfo s = Parser.skill_info_dict[index];
        if (target.activeSelf == true)
        {
            GameObject skill_clone = skillPool.Get(index);
            skill_clone.GetComponent<Actor>().cur_status.atk = s.damange;
            skill_clone.transform.localScale = new Vector3(s.range, 5, 1);
            skill_clone.transform.position = new Vector3(actor.transform.position.x + 0.5f, 0, 0);
            BoxCollider2D clone_collider = skill_clone.GetComponent<BoxCollider2D>();
            clone_collider.enabled = true;
            yield return new WaitForSeconds(0.1f);
            clone_collider.enabled = false;
            skill_clone.transform.position = new Vector3(0, 100, 0);
            skill_clone.SetActive(false);
        }
    }


    //index로 스킬 정보가져오고 추후 actor damage 연산이 필요할 경우를 대비하여 actor도 가져옴 스킬 사용위치는 target위치로 설정
    public IEnumerator FireWall(int index, Actor actor, GameObject target)
    {
        SkillInfo s = Parser.skill_info_dict[index];
        if (target.activeSelf == true)
        {

            GameObject skill_clone = skillPool.Get(index);
            skill_clone.transform.position = new Vector3(target.transform.position.x,0, 0);
            skill_clone.transform.localScale = new Vector3(s.range, 1, 1);



            //캐스팅 시작
            actor.isWalk = false;
            actor.can_search = false;
            actor.can_attack = false;
            yield return new WaitForSeconds(s.casting_time);
            actor.can_search = true;
            actor.can_attack = true;
            // 딜계산
            skill_clone.GetComponent<Actor>().cur_status.atk = s.damange;

            //데미지 타이밍
            BoxCollider2D clone_collider = skill_clone.GetComponent<BoxCollider2D>();
            clone_collider.enabled = true;
            skill_clone.GetComponent<SpriteRenderer>().color = Color.gray;
            yield return new WaitForSeconds(0.1f);
            skill_clone.GetComponent<SpriteRenderer>().color = Color.white;
            clone_collider.enabled = false;
            //종료

            skill_clone.transform.position = new Vector3(0, 100, 0);
            skill_clone.SetActive(false);
        }
        yield return null;
    }


}
