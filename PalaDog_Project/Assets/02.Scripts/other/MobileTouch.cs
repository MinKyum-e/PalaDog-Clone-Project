using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MobileTouch : MonoBehaviour
{
    [Header("Debug Test")]
    [SerializeField]
    [Range(0, 50)]private TextMeshProUGUI textTouch;


    private void Update()
    {
        //OnSingleTouch();        
        OnMultiTouch();
        OnAndriodMenu();
        OnVibrate();
    }

    private void OnSingleTouch()
    {
        if (Input.touchCount > 0) 
        {
            //첫번재 손가락 터지 정보 가져옴
            Touch touch = Input.GetTouch(0);

      
            //터치의 상태가 터치 시작일때
            if(touch.phase == TouchPhase.Began) 
            {
                textTouch.text = "Touch Begin";
            }

            //터치의 상태가 터치 종료일때
            else if(touch.phase == TouchPhase.Ended)
            {
                textTouch.text = "Touch End";
            }

        }
    }

    private void OnMultiTouch()
    {
        textTouch.text = "";
        for(int i=0;i<Input.touchCount;i++) 
        {
            Touch touch = Input.GetTouch(i);    //i번째 터치에 대한 정보
            int index = touch.fingerId;         //i번재 터치의 ID 값
            Vector2 position = touch.position;  //i번째 터치의 위치
            TouchPhase phase = touch.phase;     //i번째 터치의 상태

            if(phase == TouchPhase.Began) //터치하는 순간 1회 호출
            { }
            else if(phase == TouchPhase.Moved) //터치 후 드래그 할 때 계속
            {
                
            }
            else if(phase == TouchPhase.Stationary) //터치상태로 가만히 있을 때
            { }
            else if(phase == TouchPhase.Ended) //터치를 종료할 때 1회
            { }
            else if(phase==TouchPhase.Canceled) //시스템에 의해 터치를 종료할 때
            { }

            textTouch.text += "Index : " + index + ", Status : " + phase + ", Position(" + position + ")\n";
        }
    }

    private void OnAndriodMenu()
    {
        if(Application.platform != RuntimePlatform.Android)
        {
            return;
        }

        if(Input.GetKeyDown(KeyCode.Escape)) 
        {
            textTouch.text = "Input Escape";
        }
        if (Input.GetKeyDown(KeyCode.Home))
            {
            textTouch.text = "Input Home";
        }

        if(Input.GetKeyDown(KeyCode.Menu)) {
            textTouch.text = "Input Menu";    
        }
    }

    private void OnVibrate()
    {
        if(Input.touchCount == 3)
        {
            Handheld.Vibrate();
        }
    }




}
