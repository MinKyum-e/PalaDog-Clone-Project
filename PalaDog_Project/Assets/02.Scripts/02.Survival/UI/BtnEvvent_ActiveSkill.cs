using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BtnEvent_ActiveSkill : MonoBehaviour
{
    Animator auraSkillAnimator;

    public SkillName skillName;
    public MinionUnitIndex minionUnitIndex;
    public UnitCoolTimeUI cooltimeUI;
    public bool hero_alive = false;

    private void Start()
    {
        auraSkillAnimator = Player.Instance.aura.GetComponent<Animator>();
        cooltimeUI = GetComponent<UnitCoolTimeUI>();

    }

    public void PlayAuraSKill()
    {
        auraSkillAnimator.Play(skillName.ToString());
        Player.Instance.aura_skill.ChangeAuraSkill(skillName);
    }
    private void Update()
    {
        if(hero_alive && !GameManager.Instance.CheckHeroExists(minionUnitIndex))
        {
            cooltimeUI.ResetTimer();
            hero_alive = false;
            cooltimeUI.cooltimeImage.fillAmount = 1f;
        }

        else if(!hero_alive && GameManager.Instance.CheckHeroExists(minionUnitIndex))
        {
            hero_alive = true;
            cooltimeUI.cooltimeImage.fillAmount = 0f;
        }
    }

    public void PlayUnitActiveSkill()
    {
        //히어로 유닛과 연결 필요
        //액티브 슬룻 찾기


        Minion minion;
        //연결 안된경우 예외처리
        try
        {
            if (GameManager.Instance.hero_objects.TryGetValue(minionUnitIndex, out minion))
            {
                Actor actor = minion.actor;
                for (int i = 0; i < actor.skills.Length; i++)
                {
                    if (actor.skills[i].entry.index == (int)skillName && actor.skills[i].can_use_skill)
                    {

                        actor.can_action = false;
                        actor.animator.SetTrigger("Skill" + i);
                        cooltimeUI.StartCooldown();
                        return;
                    }
                }
            }
        }
        catch
        {

            print("에픽 유닛 존재하지 않음 , " + minionUnitIndex.ToString());
        }

    }
}
