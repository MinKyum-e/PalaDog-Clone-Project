using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanelChanger : MonoBehaviour
{
    public GameObject[] panels;

    public int seq = 0;
    public void ChangePanel(int idx)
    {
        for(int i=0;i<panels.Length; i++)
        {
            panels[i].SetActive(false);
        }
        panels[idx].SetActive(true);
    }


    public void ChangePanelSeq()
    {
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(false);
        }
        seq++;
        if(seq >=  panels.Length) 
        {
                seq = 0;
        }
        panels[seq].SetActive(true);
    }
}
