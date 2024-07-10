using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanelChanger : MonoBehaviour
{
    public GameObject[] panels;

    public void ChangePanel(int idx)
    {
        for(int i=0;i<panels.Length; i++)
        {
            panels[i].SetActive(false);
        }
        panels[idx].SetActive(true);
    }
}
