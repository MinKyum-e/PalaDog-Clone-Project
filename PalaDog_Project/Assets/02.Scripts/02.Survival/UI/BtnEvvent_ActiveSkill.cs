using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BtnEvent_ActiveSkill : MonoBehaviour
{
    Animator auraSkillAnimator;

    public SkillName skillName;
    public MinionUnitIndex minionUnitIndex;
    private void Start()
    {
        auraSkillAnimator = Player.Instance.aura.GetComponent<Animator>();    
    }

    public void PlayAuraSKill()
    {
        auraSkillAnimator.Play(skillName.ToString());
        Player.Instance.aura_skill.ChangeAuraSkill(skillName);
    }

    public void PlayUnitActiveSkill()
    {
        //히어로 유닛과 연결 필요
        //액티브 슬룻 찾기


        Minion minion;
        //연결 안된경우 예외처리
        if (GameManager.Instance.hero_objects.TryGetValue(minionUnitIndex, out minion ))
        {
            Actor actor = minion.actor;
            for (int i = 0; i < actor.skills.Length; i++)
            {
                if (actor.skills[i].entry.index == (int)skillName && actor.skills[i].can_use_skill)
                {
                    actor.can_action = false;
                    actor.animator.SetTrigger("Skill" + i);
                    return;
                }
            }
        }
        print("에픽 유닛 존재하지 않음 , " + minionUnitIndex.ToString());

    }
}
