using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnEvent_ActiveSkill : MonoBehaviour
{
    Animator auraSkillAnimator;
    private void Start()
    {
        auraSkillAnimator = Player.Instance.aura.GetComponent<Animator>();    
    }

    public void PlayAuraSKill(int idx)
    {
        auraSkillAnimator.Play(((SkillName)idx).ToString());
        Player.Instance.aura_skill.ChangeAuraSkill((SkillName)idx);
    }

    public void PlayUnitActiveSkill(int idx)
    {
        //히어로 유닛과 연결 필요
    }
}
