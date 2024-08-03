using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AuraBtnDisable : MonoBehaviour
{

    public Button healing;
    public Button atk;

    public void UpdateBtnInteractable(SkillName skill)
    {
        healing.interactable = (skill != SkillName.HealingAura);
        atk.interactable = (skill != SkillName.ATKAura);

    }
}
