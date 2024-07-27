using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBarUI : MonoBehaviour
{
    Actor actor;
    public Slider hpBar;
    public Slider hpBarBack;
    float last_hp;
    float cur_hp;
    bool back_hp_hit = false;

    public GameObject HpLineFolder;
    public GameObject HpLineFolderBack;
    public GameObject HpLineFolderBackGround;
    void OnEnable()
    {
       actor = transform.parent.GetComponent<Actor>();
        setinit();
    }
    public void setinit()
    {
        last_hp = -1;
        cur_hp = actor.cur_status.HP;
        hpBar.value = 1.0f;
        hpBarBack.value = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(last_hp != actor.status.HP)
        {
            last_hp = actor.status.HP;
            GetHpBoost();
        }


        float ratio = ((float)actor.cur_status.HP / (float)actor.status.HP);
        if (ratio >= 0)
        {
            hpBar.value = Mathf.Lerp(hpBar.value, ratio, Time.deltaTime * 5f);
        }
        else
        {
            hpBar.value = Mathf.Lerp(hpBar.value, 0, Time.deltaTime * 5f);
        }

        if(back_hp_hit)
        {

             hpBarBack.value = Mathf.Lerp(hpBarBack.value, hpBar.value, Time.deltaTime * 10f);


            if(hpBar.value >= hpBarBack.value - 0.01f)
            {
                back_hp_hit = false;
                hpBarBack.value = hpBar.value;
            }
        }

       

    }

    public void GetHpBoost()
    {
        float scaleX = (200f/ actor.cur_status.HP) /(actor.status.HP/actor.cur_status.HP);
        HpLineFolder.GetComponent<HorizontalLayoutGroup>().gameObject.SetActive(false);
        HpLineFolderBack.GetComponent<HorizontalLayoutGroup>().gameObject.SetActive(false);
        HpLineFolderBackGround.GetComponent<HorizontalLayoutGroup>().gameObject.SetActive(false);
        foreach (Transform child in HpLineFolder.transform)
        {
            child.gameObject.transform.localScale = new Vector3(scaleX, 1, 1);
        }
        foreach (Transform child in HpLineFolderBack.transform)
        {
            child.gameObject.transform.localScale = new Vector3(scaleX, 1, 1);
        }

        foreach (Transform child in HpLineFolderBackGround.transform)
        {
            child.gameObject.transform.localScale = new Vector3(scaleX, 1, 1);
        }
        HpLineFolder.GetComponent<HorizontalLayoutGroup>().gameObject.SetActive(true);
        HpLineFolderBackGround.GetComponent<HorizontalLayoutGroup>().gameObject.SetActive(true);
        HpLineFolderBack.GetComponent<HorizontalLayoutGroup>().gameObject.SetActive(true);

    }
    public void InvokeHPbar()
    {
        Invoke("BackHitHp", 0.2f);
    }


    public void BackHitHp()
    {
        back_hp_hit=true;
    }

}
