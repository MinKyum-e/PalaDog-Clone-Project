using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class intro_outro_changer : MonoBehaviour
{
    UIPanelChanger ui;

    public float change_time;
    public float timer;
    
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
        }
    }
}
