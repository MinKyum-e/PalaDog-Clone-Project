using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class DraggableUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    private UnitUnlock locker;
    public Transform canvas;// UI가 소속되어 있는 최상단 canvas transform
    private Transform previousParent; // 해당 오브젝트가 직전에 소속되어 잇었던 부모 transform
    private RectTransform rect;// UI 위치 제어를 위한 RectTransform
    private CanvasGroup canvasGroup; //UI의 알파값과 상호작용 제어를 위한 Canvasgroup
    private PoolManager poolManager;
    private CircleCollider2D auraCollider;
    private UnitCoolTimeUI cooltimeUI;
    /* private Transform food_text_transform;*/
    private Vector2 base_size;
    


    public int minion_idx;
    /*public int requisite_food;*/


    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        poolManager = GameObject.FindGameObjectWithTag("MinionPool").GetComponent<PoolManager>();
        auraCollider = GameObject.FindGameObjectWithTag("Aura").GetComponent<CircleCollider2D>();
        locker = GetComponent<UnitUnlock>();
/*        food_text_transform = transform.GetChild(0);*/
        cooltimeUI = GetComponent<UnitCoolTimeUI>();
        base_size = rect.sizeDelta;

    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        if(locker.is_lock == false )
        {
            base_size = rect.sizeDelta;
            previousParent = transform.parent;
            transform.SetParent(canvas);
            transform.SetAsLastSibling();//가장 앞에 보이도록 마지막 자식으로 설정
/*            food_text_transform.gameObject.SetActive(false);*/

            canvasGroup.alpha = 0.0f;
            canvasGroup.blocksRaycasts = false;
            FadeEffect.Instance.gameObject.SetActive(true);
            Time.timeScale = 0.1f;
        }
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(locker.is_lock == false)
        {
            UIManager.Instance.SetCurrentPage(UIPageInfo.Spawn);
            float player_y = Camera.main.WorldToScreenPoint(Player.Instance.transform.position).y;
            
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(eventData.position);
            float leftBound = auraCollider.bounds.min.x;
            float rightBount = auraCollider.bounds.max.x;
            if (worldPosition.x >= leftBound && worldPosition.x <= rightBount)
            {
                rect.position = new Vector3(eventData.position.x, player_y, 0);
                rect.sizeDelta = new Vector2( base_size.x * 2, base_size.x * 2);
                canvasGroup.alpha = 1.0f;
            }
            else
            {
                rect.position = new Vector3(eventData.position.x, eventData.position.y, 0);
                rect.sizeDelta = base_size;
                canvasGroup.alpha = 0.6f;
            }
        }
       
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if(locker.is_lock == false )
        {
            rect.sizeDelta = base_size;
            UIManager.Instance.SetCurrentPage(UIPageInfo.GamePlay);
            Vector3 spawnPoint;
            spawnPoint = Camera.main.ScreenToWorldPoint(eventData.position);
            float leftBound = auraCollider.bounds.min.x;
            float rightBount = auraCollider.bounds.max.x;
            //소환전 맥스 코스트 확인
            if (spawnPoint.x >= leftBound && spawnPoint.x <= rightBount && GameManager.Instance.CheckCost(Parser.minion_status_dict[minion_idx].cost) == true && !GameManager.Instance.CheckHeroExists((MinionUnitIndex)minion_idx))
            {
                Transform playerTransform = Player.Instance.transform;

                //spawnPoint.y = Mathf.Clamp(spawnPoint.y, playerTransform.position.y + yMin, playerTransform.position.y + yMax);
                spawnPoint.y = playerTransform.position.y;
                Minion minion = poolManager.Get(minion_idx, spawnPoint).GetComponent<Minion>();
                minion.tag = "Minion";
                //히어로 유닛인경우 game manager에 추가하기
                if(minion.actor.status.grade == UnitGrade.Hero)
                {
                    GameManager.Instance.AddHeroUnit(minion);

                    //액티브 UI 잠금해제

                }
                else
                {
                    cooltimeUI.StartCooldown();
                }
   /*             GameManager.Instance.cur_food -=requisite_food;*/
                GameManager.Instance.cur_cost +=minion.cost;

            }


            if (transform.parent == canvas)
            {
                transform.SetParent(previousParent);
                rect.position = previousParent.GetComponentInParent<RectTransform>().position;
/*                food_text_transform.gameObject.SetActive(true);*/

            }


            canvasGroup.alpha = 1.0f;
            canvasGroup.blocksRaycasts = true;
            FadeEffect.Instance.gameObject.SetActive(false);
            Time.timeScale = 1f;
            
        }
        
    }
}
