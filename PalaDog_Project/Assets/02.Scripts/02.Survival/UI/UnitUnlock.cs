using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UnitUnlock : MonoBehaviour, IPointerClickHandler
{
    public bool is_lock;
    [SerializeField]
    private bool already_unlock;
    [SerializeField]
    private int shop_index;
    [SerializeField]
    private int prerequisite;


    public bool can_unlock = false;


    

    private Image image_renderer;
    public Sprite locked_sprite;
    public Sprite unlocked_sprite;
    public Sprite unshow;

    public GameObject cost_object;
    private int cost;

    public int minion_idx;

    private void Awake()
    {
        image_renderer = GetComponent<Image>();

    }
    private void Start()
    {
        foreach (KeyValuePair<int, ShopItemInfo> kvp in Parser.shop_item_info_dict )
        {
            if (kvp.Value.etc_value == minion_idx && kvp.Value.list_type == ShopEnums.ListType.Unlock)
            {
                shop_index = kvp.Key;
                prerequisite = kvp.Value.prelist;
            }

        }

        cost = Parser.minion_status_dict[minion_idx].cost;


        if(PlayerPrefs.HasKey(shop_index.ToString()))
        {
            UnLock(false);
        }

        if (already_unlock)
        {
            UnLock(false);
        }



        if (ShopManager.Instance != null && is_lock)
        {
            foreach (var un in ShopManager.Instance.unlocked_ingame_unit_list)
            {
                if (un == shop_index)
                {
                    UnLock(false);
                    break;
                }
            }
        }
    }
    private void OnEnable()
    {
        if(ShopManager.Instance != null && is_lock )
        {
            foreach (var un in ShopManager.Instance.unlocked_ingame_unit_list)
            {
                if (un == shop_index)
                {
                    UnLock(false);
                    break;
                }
            }
        }
     
    }

    private void Update()
    {
        if( can_unlock)
        {
            if (!is_lock)
            {
                image_renderer.sprite = unlocked_sprite;
            }
            else if (ShopManager.Instance.CheckPrerequisite(ShopEnums.UnLockType.InGameUnit, prerequisite))
            {
                image_renderer.sprite = locked_sprite;
            }
            else
            {
                image_renderer.sprite = unshow;
            }


        }
    }



    public void OnPointerClick(PointerEventData eventData)
    {
        if (ShopManager.Instance.CheckPrerequisite(ShopEnums.UnLockType.InGameUnit, prerequisite) && can_unlock && is_lock)
        {
            UnLock(true);
           
        }
    }

    public void UnLock(bool click)
    {
        is_lock = false;
        image_renderer.sprite = unlocked_sprite;
        
        if(cost_object != null)
        {
            cost_object.SetActive(true);
            cost_object.transform.GetChild(0).GetComponent<TMP_Text>().text = cost.ToString();
        }
        
        ShopManager.Instance.AddToUnLockList(ShopEnums.UnLockType.InGameUnit, shop_index);
        if (!already_unlock && can_unlock && click)
        {
            UIManager.Instance.FadeOut_next_stage1.SetActive(true);
        }
            
    }
}
