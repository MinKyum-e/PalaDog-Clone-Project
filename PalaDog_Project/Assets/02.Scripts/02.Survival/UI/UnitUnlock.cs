using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UnitUnlock : MonoBehaviour, IPointerClickHandler
{
    public bool is_lock;
    [SerializeField]
    private int unlock_gold;
    [SerializeField]
    private int shop_index;
    [SerializeField]
    private int prerequisite;


    public bool can_unlock = false;


    public TMP_Text unlock_gold_text;
    

    private Image image_renderer;
    public Sprite unlocked_sprite;

    public Image resorce_img_renderer;

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
                unlock_gold = kvp.Value.goods_value;
                unlock_gold_text.text = unlock_gold.ToString();
                shop_index = kvp.Key;
                prerequisite = kvp.Value.prelist;
            }

        }

        cost = Parser.minion_status_dict[minion_idx].cost;

        if (unlock_gold == 0)
        {
            UnLock();
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
                    UnLock();
                    break;
                }
            }
        }
     
    }



    public void OnPointerClick(PointerEventData eventData)
    {
        if (ShopManager.Instance.CheckPrerequisite(ShopEnums.UnLockType.InGameUnit, prerequisite) && can_unlock && is_lock)
        {
            UnLock();
           
        }
    }

    public void UnLock()
    {
        is_lock = false;
        image_renderer.sprite = unlocked_sprite;
        resorce_img_renderer.enabled = false;
        unlock_gold_text.enabled = false;
        cost_object.SetActive(true);
        cost_object.transform.GetChild(0).GetComponent<TMP_Text>().text = cost.ToString();
        ShopManager.Instance.AddToUnLockList(ShopEnums.UnLockType.InGameUnit, shop_index);
    }
}
