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
    private int can_unlock; //진화트리 해금조건 확인용
    [SerializeField]
    private int shop_index;
    [SerializeField]
    private int prerequisite;


    public TMP_Text unlock_gold_text;
    public DraggableUI draggableUI;
    public Sprite unlocked_sprite;
    private Image image_renderer;

    private void Awake()
    {
        draggableUI = GetComponent<DraggableUI>();
        image_renderer = GetComponent<Image>();

    }
    private void Start()
    {
        foreach (KeyValuePair<int, ShopItemInfo> kvp in Parser.shop_item_info_dict )
        {
            if(kvp.Value.etc_value == draggableUI.minion_idx && kvp.Value.list_type == ShopEnums.ListType.Unlock)
            {
                unlock_gold = kvp.Value.goods_value;
                unlock_gold_text.text = unlock_gold.ToString();
                shop_index = kvp.Key;
                prerequisite = kvp.Value.prelist;
            }

            if (kvp.Value.etc_value == draggableUI.minion_idx && kvp.Value.list_type == ShopEnums.ListType.Spawn)
            {
                draggableUI.requisite_food = kvp.Value.goods_value;
            }
        }

        if(unlock_gold == 0)
        {
            UnLock();
        }
    }



    public void OnPointerClick(PointerEventData eventData)
    {
        if (is_lock && GameManager.Instance.cur_gold >= unlock_gold && ShopManager.Instance.CheckPrerequisite(ShopEnums.UnLockType.InGameUnit, prerequisite))
        {
            UnLock();
        }
    }

    public void UnLock()
    {
        GameManager.Instance.cur_gold -= unlock_gold;
        is_lock = false;
        image_renderer.sprite = unlocked_sprite;
        unlock_gold_text.text = draggableUI.requisite_food.ToString();
        ShopManager.Instance.AddToUnLockList(ShopEnums.UnLockType.InGameUnit, shop_index);
    }
}
