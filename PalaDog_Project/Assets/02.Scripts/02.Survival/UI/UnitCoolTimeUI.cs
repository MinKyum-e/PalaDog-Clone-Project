using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Unity.VisualScripting;

public class UnitCoolTimeUI : MonoBehaviour
{
    DraggableUI draggableUI;
    BtnEvent_ActiveSkill skillUI;



    public Image cooltimeImage;
    private Image mainImage;


    public bool is_hero = false;
    public bool unit_alive = false;

    public bool resets = false;

    float cooltime;
    float timer;
    bool isCooldown;

    private void Awake()
    {

        mainImage = GetComponent<Image>();
        if (gameObject.layer ==6)//spawnUI
        {
            draggableUI = GetComponent<DraggableUI>();
        }
        else if (gameObject.layer == 7)//skillSUI
        {
            skillUI = GetComponent<BtnEvent_ActiveSkill>();
        }

        cooltimeImage = transform.Find("CoolTime").GetComponent<Image>();

    }


    void Start()
    {
        if(gameObject.layer == 6)
        {
            cooltime = Parser.minion_status_dict[draggableUI.minion_idx].cool_time;
            if(Parser.minion_status_dict[draggableUI.minion_idx].common.grade == UnitGrade.Hero)
            {
                is_hero = true;
            }

        }
        else if(gameObject.layer == 7)
        {
            cooltime = Parser.skill_table_dict[(int)skillUI.skillName].coolTime;
        }
        

    }

    void Update()
    {
        
        if (is_hero)
        {

            if (unit_alive)
            {
                if (!GameManager.Instance.CheckHeroExists((MinionUnitIndex)draggableUI.minion_idx))
                {
                    unit_alive = false;
                    StartCooldown();
                }
            }
            else
            {
                if(GameManager.Instance.CheckHeroExists((MinionUnitIndex)draggableUI.minion_idx))
                {
                    cooltimeImage.fillAmount = 1f;
                    unit_alive = true;
                    mainImage.raycastTarget = false;
                }
            }
        }


        if (timer < 0f)
        {
            ResetTimer();
        }

    }



    public void ResetTimer()
    {
        CancelInvoke("TimerStart");
        timer = 0f;
        isCooldown = false; unit_alive = false;
        cooltimeImage.fillAmount = 0f;
        mainImage.raycastTarget = true;
    }

    public void StartCooldown()
    {
        if (!isCooldown)
        {
            mainImage.raycastTarget = false;
            isCooldown = true;
            timer = cooltime;
            cooltimeImage.fillAmount = 1f;
        }

        InvokeRepeating("TimerStart", 0.01f, 0.01f);

    }

    public void TimerStart()
    {
        timer -= 0.01f;
        cooltimeImage.fillAmount = timer / cooltime;
    }
}
