
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform canvas;// UI가 소속되어 있는 최상단 canvas transform
    private Transform previousParent; // 해당 오브젝트가 직전에 소속되어 잇었던 부모 transform
    private RectTransform rect;// UI 위치 제어를 위한 RectTransform
    private CanvasGroup canvasGroup; //UI의 알파값과 상호작용 제어를 위한 Canvasgroup
    private float yMax;
    private float yMedian;
    private float yMin;

    public int unit_idx;

    private void Awake()
    {
        canvas = FindObjectOfType<Canvas>().transform;
        rect = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        yMax = 0.5f; yMedian = -1.0f; yMin = -1.5f;
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        previousParent = transform.parent;

        transform.SetParent(canvas);
        transform.SetAsLastSibling();//가장 앞에 보이도록 마지막 자식으로 설정

        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rect.position = eventData.position;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        Vector3 spawnPoint;
        spawnPoint = Camera.main.ScreenToWorldPoint( eventData.position );
        Transform playerTransform = GameManager.Instance.player.transform;
       
        spawnPoint.y = Mathf.Clamp(spawnPoint.y, playerTransform.position.y + yMin, playerTransform.position.y + yMax);
        spawnPoint.z = GameManager.Instance.player.transform.position.z;


        GameObject unit = GameManager.Instance.pool.Get(unit_idx);
        unit.transform.position = spawnPoint;
        if (spawnPoint.y > playerTransform.position.y +  yMedian)
            unit.GetComponent<SpriteRenderer>().sortingOrder = 0;
        else
            unit.GetComponent<SpriteRenderer>().sortingOrder=2;

        Debug.Log($" {unit.transform.position}");
        
        if (transform.parent == canvas)
        {
            transform.SetParent(previousParent);
            rect.position = previousParent.GetComponentInParent<RectTransform>().position;
            
        }

        canvasGroup.alpha = 1.0f;
        canvasGroup.blocksRaycasts = true;
    }
}
