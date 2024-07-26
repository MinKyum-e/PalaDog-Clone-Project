using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Unity.VisualScripting;

public class UnitCoolTimeUI : MonoBehaviour
{
    DraggableUI draggableUI;


    private Image cooltimeImage;
    private Image draggableImage;

    float cooltime;
    float timer;
    bool isCooldown;

    private void Awake()
    {
        draggableImage = GetComponent<Image>();
        draggableUI = GetComponent<DraggableUI>();
        cooltimeImage = transform.Find("CoolTime").GetComponent<Image>();
    }
    void Start()
    {
        cooltime = Parser.minion_status_dict[draggableUI.minion_idx].cool_time;
    }

    void Update()
    {

        if (timer <= 0f)
        {
            CancelInvoke("TimerStart");
            timer = 0f;
            isCooldown = false;
            cooltimeImage.fillAmount = 0f;
            cooltimeImage.enabled = false;
            draggableImage.raycastTarget = true;
        }

    }
    public void StartCooldown()
    {
        if (!isCooldown)
        {
            draggableImage.raycastTarget = false;
            cooltimeImage.enabled = true;
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
