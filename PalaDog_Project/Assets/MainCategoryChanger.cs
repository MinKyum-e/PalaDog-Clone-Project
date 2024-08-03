using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class MainCategoryChanger : MonoBehaviour
{
    public List<Image> main_btns;
    public float alpha;

    private void Awake()
    {
        int length = transform.childCount;
        for(int i=0;i<length;i++)
        {
            main_btns.Add(transform.GetChild(i).GetComponent<Image>());
        }
    }

    public void ChangeMainBtn(int idx)
    {
        for (int i = 0; i < main_btns.Count;i++)
        {
            Color c = main_btns[i].color;
            c.a = (i == idx) ? alpha : 0.0f;
            main_btns[i].color = c;

        }
    }
    public void HideALL()
    {
        for (int i = 0; i < main_btns.Count; i++)
        {
            main_btns[i].transform.GetChild(1).gameObject.SetActive(false);
        }
    }

}
