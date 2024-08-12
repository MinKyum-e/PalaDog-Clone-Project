using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBarUI : MonoBehaviour
{
    Canvas canvas;
    Actor actor;
    public Slider hpBar;
    float last_hp;
    float cur_hp;
    bool back_hp_hit = false;

    public GameObject HpLineFolder;
    public GameObject HpLineFolderBackGround;
    void OnEnable()
    {
        canvas = GetComponent<Canvas>();
       actor = transform.parent.GetComponent<Actor>();
        canvas.sortingOrder = actor.GetComponent<SpriteRenderer>().sortingOrder;
        setinit();
    }
    public void setinit()
    {
        last_hp = -1;
        cur_hp = actor.cur_status.HP;
        hpBar.value = 1.0f;
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

       

    }

    public void GetHpBoost()
    {
        float scaleX = (200f/ actor.cur_status.HP) /(actor.status.HP/actor.cur_status.HP);
        HpLineFolder.GetComponent<HorizontalLayoutGroup>().gameObject.SetActive(false);
        HpLineFolderBackGround.GetComponent<HorizontalLayoutGroup>().gameObject.SetActive(false);
        foreach (Transform child in HpLineFolder.transform)
        {
            child.gameObject.transform.localScale = new Vector3(scaleX, 1, 1);
        }


        foreach (Transform child in HpLineFolderBackGround.transform)
        {
            child.gameObject.transform.localScale = new Vector3(scaleX, 1, 1);
        }
        HpLineFolder.GetComponent<HorizontalLayoutGroup>().gameObject.SetActive(true);
        HpLineFolderBackGround.GetComponent<HorizontalLayoutGroup>().gameObject.SetActive(true);

    }

}
