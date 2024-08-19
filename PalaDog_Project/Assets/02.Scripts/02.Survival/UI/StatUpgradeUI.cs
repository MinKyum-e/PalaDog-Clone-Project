using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using ShopEnums;
using System.Diagnostics;
using System;
using Unity.VisualScripting;
using UnityEngine.UI;
public class StatUpgradeUI : MonoBehaviour
{
    public EnforceType type;
    TMP_Text t;
    TMP_Text gold_t;
    public Image gold_img;
    [SerializeField]
    int cur_lvl;
    [SerializeField]
    int max_lvl;
    [SerializeField]
    int next_price;
    // Start is called before the first frame update
    bool save_file;
    void Start()
    {
        t = transform.GetChild(0).GetComponent<TMP_Text>();
        gold_t = transform.GetChild(1).GetComponent<TMP_Text>();

        if (PlayerPrefs.HasKey(type.ToString())){
            save_file = true;

            cur_lvl = PlayerPrefs.GetInt(type.ToString());
        }
        else
            cur_lvl = 0;
        
        /*if(!ShopManager.Instance.ingame_enforce_cur_lvl.TryGetValue(type, out cur_lvl))
        {
            cur_lvl = 0;
        }*/


    }


    private void Update()
    {
        if (cur_lvl == 0)
        {
            Upgrade();
        }
        if (save_file)
        {
            save_file = false;
            max_lvl = ShopManager.Instance.GetEnforceMaxLvL(type);
            if (cur_lvl < max_lvl)
            {

                next_price = ShopManager.Instance.GetEnforcePrice(type, cur_lvl);
                switch (type)
                {
                    case EnforceType.MAX_Cost:
                        t.text = $"최대 코스트 {cur_lvl}";

                        break;
                    case EnforceType.Unit_LvL:
                        t.text = $"유닛 레벨 {cur_lvl}";
                        break;
                    case EnforceType.Aura:
                        t.text = $"마법진 크기 {cur_lvl}";
                        break;

                }
                gold_t.text = $"{next_price}";
                gold_img.gameObject.SetActive(true);
            }
            else
            {
                t.text = $"{type} ({cur_lvl}/{max_lvl})\n  MAX";
                switch (type)
                {
                    case EnforceType.MAX_Cost:
                        t.text = $"최대 코스트 M";
                        break;
                    case EnforceType.Unit_LvL:
                        t.text = $"유닛 레벨 M";
                        break;
                    case EnforceType.Aura:
                        t.text = $"마법진 크기 M";
                        break;
                }
                gold_t.text = "";
                gold_img.gameObject.SetActive(false);
            }
        }
        
    }

    public void Upgrade()
    {
        if (cur_lvl == 0)
        {
            max_lvl = ShopManager.Instance.GetEnforceMaxLvL(type);

            
        }
        if (cur_lvl >= max_lvl)
            return;


        if(GameManager.Instance.cur_gold >= next_price)
        {
            GameManager.Instance.cur_gold -=next_price;
            ShopManager.Instance.EnforceIngameBase(type, cur_lvl++);
            ShopManager.Instance.ingame_enforce_cur_lvl[type] = cur_lvl;
            if (cur_lvl < max_lvl)
            {

                next_price = ShopManager.Instance.GetEnforcePrice(type, cur_lvl);
                switch (type)
                {
                    case EnforceType.MAX_Cost:
                        t.text = $"최대 코스트 {cur_lvl}";

                        break;
                    case EnforceType.Unit_LvL:
                        t.text = $"유닛 레벨 {cur_lvl}";
                        break;
                    case EnforceType.Aura:
                        t.text = $"마법진 크기 {cur_lvl}";
                        break;

                }
                gold_t.text = $"{next_price}";
                gold_img.gameObject.SetActive(true);
            }
            else
            {
                t.text = $"{type} ({cur_lvl}/{max_lvl})\n  MAX";
                switch (type)
                {
                    case EnforceType.MAX_Cost:
                        t.text = $"최대 코스트 M";
                        break;
                    case EnforceType.Unit_LvL:
                        t.text = $"유닛 레벨 M";
                        break;
                    case EnforceType.Aura:
                        t.text = $"마법진 크기 M";
                        break;
                }
                gold_t.text = "";
                gold_img.gameObject.SetActive(false);
            }

        }


    }
}
