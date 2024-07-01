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
        if (isCooldown)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                timer = 0f;
                isCooldown = false;
                cooltimeImage.fillAmount = 0f;
                cooltimeImage.enabled = false;
                draggableImage.raycastTarget = true;
            }
            else
            {
                cooltimeImage.fillAmount = timer / cooltime;
            }
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
    }
}
