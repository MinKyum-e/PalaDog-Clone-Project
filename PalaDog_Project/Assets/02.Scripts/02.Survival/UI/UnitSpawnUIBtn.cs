using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class UnitSpawnUI : MonoBehaviour
{

    public GameObject[] unit_panels;
    public GameObject player_skill;
    public GameObject[] skill_panels;

    public bool player_panel = false;
    public bool current_page = false;


    public void OpenPlayerPanel()
    {
        player_skill.SetActive(false);
        for (int i = 0; i < unit_panels.Length; i++)
        {
            skill_panels[i].SetActive(false);
            unit_panels[i].SetActive(false);
        }

        player_panel = !player_panel;
        if (player_panel)
        {
            if (current_page)
            {
                skill_panels[0].gameObject.SetActive(true);
                unit_panels[0].gameObject.SetActive(true);
            }
            else
            {
                skill_panels[1].gameObject.SetActive(true);
                unit_panels[1].gameObject.SetActive(true);
            }
        }
        else
            player_skill.gameObject.SetActive(true);
    }

    public void OpenUnitPanel()
    {
        for(int i=0;i<unit_panels.Length;i++)
        {
            skill_panels[i].SetActive(false);
            unit_panels[i].SetActive(false);
        }


        current_page = !current_page;
        if (current_page)
        {
            skill_panels[0].gameObject.SetActive(true);
            unit_panels[0].gameObject.SetActive(true);
        }
            
        else
        {
            unit_panels[1].gameObject.SetActive(true);
            skill_panels[1].gameObject.SetActive(true);
        }
            


    }
}
