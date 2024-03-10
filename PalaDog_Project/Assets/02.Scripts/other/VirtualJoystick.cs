using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VirtualJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
	private	Image	imageBackground;    // 우리가 실제 터치하는 컨트롤러의 배경 이미지
	private Image imageController;
	private Vector2 touchPosition;


	// x, y 방향 값을 외부에서 열람할 수 있도록 Get 전용 프로퍼티 정의

	private void Awake()
	{
		imageBackground = GetComponent<Image>();
		imageController = transform.GetChild(0).GetComponent<Image>();
	}

	/// <summary>
	/// 터치하는 순간 1회 호출
	/// </summary>
	public void OnPointerDown(PointerEventData eventData)
	{
		//Debug.Log("Touch Began : "+eventData);
	}

	/// <summary>
	/// 터치 상태로 드래그할 때 매 프레임 호출
	/// </summary>
	public void OnDrag(PointerEventData eventData)
	{
		touchPosition = Vector2.zero;

		//조이스틱 위치가 어디에 있든 동일 한 값을 연산하기 위해
		//touchPosition의 위치 값은 이미지의 현재 위치를 기준으로 얼마나 떨어져있는지에 따라 다르게 나옴
		//(터치되는 이미지의 RectTransform 정보, 화면 터치 좌표, 현재 화면에 대한 카메라, 연산된 좌표를 저장)
		//rect의 중심축(pivot)을 기준으로 현재 screenPoint좌표가 중심축으로부터 얼마나 떨어져 있는지를 touchPosition에 저장
        if(RectTransformUtility.ScreenPointToLocalPointInRectangle(
			imageBackground.rectTransform, eventData.position, eventData.pressEventCamera, out touchPosition))
		{

			//touchPosition 값 정규화[0, 1]
			//touchPosition을 이미지 크기로 나눔
			touchPosition.x = (touchPosition.x / imageBackground.rectTransform.sizeDelta.x);

			// [-1 , 1]
			touchPosition = new Vector2(touchPosition.x * 2 - 1, 0);
			//이미지 밖으로 터치가 나가게 되면 -1 ~ 1사이 값으로 정규화
			touchPosition = (touchPosition.magnitude > 1) ? touchPosition.normalized : touchPosition;
			//Debug.Log("Touch & Drag : " + eventData);


			//가상 조이스틱 컨트롤러 이미지 이동
			imageController.rectTransform.anchoredPosition = new Vector2(
				touchPosition.x * imageBackground.rectTransform.sizeDelta.x / 2
				, 0

				);
		
		}
    }

	/// <summary>
	/// 터치를 종료하는 순간 1회 호출
	/// </summary>
	public void OnPointerUp(PointerEventData eventData)
	{
		//중앙으로 옮기기

		imageController.rectTransform.anchoredPosition = Vector2.zero;
		//이동 초기화
		touchPosition = Vector2.zero;
    }

	/// <summary>
	/// get 함수
	/// </summary>
	/// <returns></returns>
	public float Horizontal()
	{
		
		return touchPosition.x;
	}
}

