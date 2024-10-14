using UnityEngine;

public class TouchAnywhere : MonoBehaviour
{
    public GameObject obj;
    void Update()
    {
        // 모바일 터치 입력 처리
        if (Input.touchCount > 0)
        {
            // 첫 번째 터치에 대한 정보 가져오기
            Touch touch = Input.GetTouch(0);

            // 터치 시작 시 발동 (터치하는 순간)
            if (touch.phase == TouchPhase.Began)
            {
                OnTouch();
            }
        }

        // PC에서의 마우스 클릭 입력 처리
        if (Input.GetMouseButtonDown(0))
        {
            OnTouch();
        }
    }

    // 터치 혹은 클릭 시 발동할 이벤트
    void OnTouch()
    {
        obj.SetActive(true);
    }
}