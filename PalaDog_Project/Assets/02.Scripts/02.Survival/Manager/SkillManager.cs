using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//범위, 데미지, 캐스팅시간, 쿨타임 알고있다고 치고
public class SkillManager:MonoBehaviour
{
    private static SkillManager _instance;
    public static SkillManager Instance { get { return _instance; } }
    private PoolManager skillPool;
    private PoolManager enemyPool;
    private PoolManager minionPool;

    private void Awake()
    {
        _instance = this;
        skillPool = GameObject.FindGameObjectWithTag("SkillPool").GetComponent<PoolManager>();
        enemyPool = GameObject.FindGameObjectWithTag("EnemyPool").GetComponent<PoolManager>();
        minionPool = GameObject.FindGameObjectWithTag("MinionPool").GetComponent<PoolManager>();
    }

    //index로 스킬 정보가져오고 추후 actor damage 연산이 필요할 경우를 대비하여 actor도 가져옴 스킬 사용위치는 target위치로 설정
    public IEnumerator FireWall(int index, Actor actor, GameObject target)
    {
        SkillInfo s = Parser.skill_info_dict[index];
        if (target.activeSelf == true)
        {

            GameObject skill_clone = skillPool.Get(index);
        skill_clone.transform.position = target.transform.position;

        //캐스팅 시작
        actor.can_attack = false;
        actor.isWalk = false;
        yield return new WaitForSeconds(s.casting_time);
        actor.can_attack = true;
        actor.isWalk = true;
            //데미지 주기
        BoxCollider2D clone_collider = skill_clone.GetComponent<BoxCollider2D>();
            clone_collider.enabled = true;
        yield return new WaitForSeconds(0.1f);
            clone_collider.enabled = false;
        skill_clone.SetActive(false);
        }
        yield return null;
    }


}
