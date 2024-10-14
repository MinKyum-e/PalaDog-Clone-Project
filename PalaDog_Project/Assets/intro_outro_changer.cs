using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class intro_outro_changer : MonoBehaviour
{
    UIPanelChanger ui;

    public float change_time;
    public float timer;
    public bool refreshing = false;
    
    private void Awake()
    {
        ui = GetComponent<UIPanelChanger>();
        timer = 0;
    }


    private void Update()
    {
        timer+= Time.deltaTime;
        if(timer > change_time)
        {
            timer = 0;
            if(ui.seq < ui.panels.Length-1)
            {
                ui.ChangePanelSeq();
            }
            else if((ui.seq == ui.panels.Length - 1) && refreshing)
            {
                refresh();
            }
        }
    }


    private void refresh()
    {
        ui.panels[0].SetActive(true);
        ui.panels[ui.panels.Length - 1].SetActive(false);
        ui.seq = 0;
        timer = 0;
        gameObject.SetActive(false);
    }
}
