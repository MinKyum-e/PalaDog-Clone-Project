using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;
using UnityEngine.UI;



public class UIPanelChanger : MonoBehaviour
{
    public GameObject[] panels;

    public int seq = 0;
    public string cur_image = "knight";

    public Image main_img, sub_img;

    public Sprite knight;
    public Sprite archer;


    public void ChangePanel(int idx)
    {
        if(idx >= panels.Length) { return; }
        for(int i=0;i<panels.Length; i++)
        {
            panels[i].SetActive(false);
        }
        panels[idx].SetActive(true);
        seq = idx;
    }

    public void ChangPlanel_Spawn()
    {
        RectTransform a = panels[0].GetComponent<RectTransform>();
        RectTransform b = panels[1].GetComponent<RectTransform>();
        

        if(b.gameObject.activeSelf  )
        {
            Vector3 pos = panels[0].GetComponent<RectTransform>().position;
            a.position = b.position;
            b.position = pos;
        }
        else
        {
            Vector3 pos = panels[1].GetComponent<RectTransform>().position;
            b.position = a.position;
            a.position = pos;
        }
        
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

    public void changer_sprite_change()
    {

        if (seq == 0)
        {
            main_img.sprite = knight;
            sub_img.sprite = archer;
        }
        else
        {
            main_img.sprite =archer;
            sub_img.sprite = knight;
        }
    }

    public void OnOffPanel(int idx)
    {
        panels[idx].SetActive(!panels[idx].activeSelf);

        //패널 열었을때 타임 멈추기
        if (panels[idx].activeSelf)
            Time.timeScale = 0.0f;
        else
            Time.timeScale = 1.0f;
    }
 public void OnOffPanelNoTimeStop(int idx)
    {
        panels[idx].SetActive(!panels[idx].activeSelf);

    }

    public void AllPanelHide()
    {
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(false);
        }
    }
}
