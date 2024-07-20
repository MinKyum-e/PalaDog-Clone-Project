using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using ShopEnums;
using System.Diagnostics;
using System;
using Unity.VisualScripting;
public class StatUpgradeUI : MonoBehaviour
{
    public EnforceType type;
    TMP_Text t;
    [SerializeField]
    int cur_lvl;
    [SerializeField]
    int max_lvl;
    [SerializeField]
    int next_price;
    // Start is called before the first frame update
    void Start()
    {
        t = transform.GetChild(0).GetComponent<TMP_Text>();
        cur_lvl = 0;
        
    }


    private void Update()
    {
        if(cur_lvl == 0)
        {
            Upgrade();
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
            if (cur_lvl < max_lvl)
            {
                next_price = ShopManager.Instance.GetEnforcePrice(type, cur_lvl);
                t.text = $"{type} ({cur_lvl}/{max_lvl})\n  price : {next_price}";
            }
            else
            {
                t.text = $"{type} ({cur_lvl}/{max_lvl})\n  MAX";
            }
        }
        

    }
}
