using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class UnitSpawnUI : MonoBehaviour
{

    public GameObject[] unit_panels;

    public int current_page = -1;

    public void OpenUnitPanel(int _panel_info)
    {
        //close panel
        if((int)_panel_info == current_page)
        {
            unit_panels[current_page].SetActive(false);
            current_page = -1;
            return;
        }

        //all close
        for (int i = 0; i < unit_panels.Length; i++)
        {
                unit_panels[i].gameObject.SetActive(false);
        }


        current_page = _panel_info;

        //open panel
        for(int i=0;i<unit_panels.Length;i++)
        {
            if (i == current_page)
                unit_panels[i].gameObject.SetActive(true);
        }


    }
}
