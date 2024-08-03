using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class MainCategoryChanger : MonoBehaviour
{
    public Image[] main_btns;
    public float alpha;

    public void ChangeMainBtn(int idx)
    {
        for (int i = 0; i < main_btns.Length;i++)
        {
            Color c = main_btns[i].color;
            c.a = (i == idx) ? alpha : 0.0f;
            main_btns[i].color = c;
            
            
        }
    }

}
