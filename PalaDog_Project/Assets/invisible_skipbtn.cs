using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class invisible_skipbtn : MonoBehaviour
{
    public UIPanelChanger changer;

    private void Update()
    {
        if(changer.seq == changer.panels.Length-1)
        {
            gameObject.SetActive(false);
        }
    }
}
