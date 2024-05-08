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
    private int minion_idx;
    [SerializeField]
    private int unlock_gold;
    [SerializeField]
    private int can_unlock; //진화트리 해금조건 확인용


    public TMP_Text unlock_gold_text;
    public Sprite unlocked_sprite;
    private Image image_renderer;

    private void Awake()
    {
        minion_idx = GetComponent<DraggableUI>().minion_idx;
        image_renderer = GetComponent<Image>();
        
    }



    public void OnPointerClick(PointerEventData eventData)
    {
        if(is_lock && GameManager.Instance.GetGold() >= unlock_gold)
        {
            GameManager.Instance.UpdateGold(-unlock_gold);
            is_lock = false;
            image_renderer.sprite = unlocked_sprite;
            unlock_gold_text.enabled = false;
        }
    }
}
