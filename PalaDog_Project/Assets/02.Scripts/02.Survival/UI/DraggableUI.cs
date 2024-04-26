
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform canvas;// UI가 소속되어 있는 최상단 canvas transform
    private Transform previousParent; // 해당 오브젝트가 직전에 소속되어 잇었던 부모 transform
    private RectTransform rect;// UI 위치 제어를 위한 RectTransform
    private CanvasGroup canvasGroup; //UI의 알파값과 상호작용 제어를 위한 Canvasgroup
    private PoolManager poolManager;
    private CircleCollider2D auraCollider;


    public int minion_idx;

    private void Awake()
    {
        canvas = FindObjectOfType<Canvas>().transform;
        rect = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        poolManager = GameObject.FindGameObjectWithTag("MinionPool").GetComponent<PoolManager>();
        auraCollider = GameObject.FindGameObjectWithTag("Aura").GetComponent<CircleCollider2D>();
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        previousParent = transform.parent;

        transform.SetParent(canvas);
        transform.SetAsLastSibling();//가장 앞에 보이도록 마지막 자식으로 설정

        canvasGroup.alpha = 0.0f;
        canvasGroup.blocksRaycasts = false;
        FadeEffect.Instance.gameObject.SetActive(true);
        Time.timeScale = 0.1f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        float player_y = Camera.main.WorldToScreenPoint(Player.Instance.transform.position).y;
        rect.position = new Vector3(eventData.position.x, player_y+70f, 0);
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(eventData.position);
        float leftBound = auraCollider.bounds.min.x;
        float rightBount = auraCollider.bounds.max.x;
        if (worldPosition.x >= leftBound && worldPosition.x <= rightBount)
        {
            canvasGroup.alpha = 0.6f;
        }
        else
        {
            canvasGroup.alpha = 0f;
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        Vector3 spawnPoint;
        spawnPoint = Camera.main.ScreenToWorldPoint(eventData.position);
        float leftBound = auraCollider.bounds.min.x;
        float rightBount = auraCollider.bounds.max.x;
        //소환전 맥스 코스트 확인
        if (spawnPoint.x >= leftBound && spawnPoint.x <= rightBount && GameManager.Instance.CheckCost(Parser.minion_status_dict[minion_idx].cost) == true)
        {
            Transform playerTransform = Player.Instance.transform;

            //spawnPoint.y = Mathf.Clamp(spawnPoint.y, playerTransform.position.y + yMin, playerTransform.position.y + yMax);
            spawnPoint.y = playerTransform.position.y;
            spawnPoint.z = playerTransform.position.z;

            Minion minion = poolManager.Get(minion_idx).GetComponent<Minion>();
            minion.transform.position = new Vector3(spawnPoint.x, spawnPoint.y + 0.4f, Random.Range(-1, 1));
            minion.tag = "Minion";
            minion.GetComponent<SpriteRenderer>().sortingOrder = 4;
            GameManager.Instance.UpdateCost(minion.cost);
        }


        if (transform.parent == canvas)
        {
            transform.SetParent(previousParent);
            rect.position = previousParent.GetComponentInParent<RectTransform>().position;
            
        }


        canvasGroup.alpha = 1.0f;
        canvasGroup.blocksRaycasts = true;
        FadeEffect.Instance.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }
}
