using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    public GraphicRaycaster uiRaycaster;
    public EventSystem eventSystem;
    public Actor actor;
    bool walking = false;
    bool right = true;

    void Update()
    {
#if UNITY_EDITOR
        HandleMouseInput();
#else
        HandleTouchInput();
#endif
    }

    private void HandleMouseInput()
    {
        // 마우스 입력 확인 (에디터에서 디버깅용)
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Input.mousePosition;


            if (!IsPointerOverUIObject(mousePosition))
            {
                if (mousePosition.x < Screen.width / 2)
                {
                    LeftBtnDown();
                }
                else
                {
                    RightBtnDown();
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            Vector2 mousePosition = Input.mousePosition;

            if (!IsPointerOverUIObject(mousePosition))
            {
                if (mousePosition.x < Screen.width / 2)
                {
                    LeftBtnUp();
                }
                else
                {
                    // 화면의 오른쪽 클릭 해제
                    RightBtnUp();
                }
            }
        }
    }

    private void HandleTouchInput()
    {
        // 터치 입력 확인 (모바일 장치에서 실행될 때)
        if (Input.touchCount > 0)
        {
            bool walking = false;
            for (int i = 0; i < Input.touchCount; i++)
            {
                {
                    Touch touch = Input.GetTouch(i);

                    if (touch.phase == TouchPhase.Began)
                    {
                        Vector2 touchPosition = touch.position;

                        Debug.Log("Touch detected at: " + touchPosition); // 터치 감지 확인

                        if (!IsPointerOverUIObject(touchPosition))
                        {
                            if (touchPosition.x < Screen.width / 2)
                            {
                                // 화면의 왼쪽 터치
                                LeftBtnDown();
                            }
                            else
                            {
                                // 화면의 오른쪽 터치
                                RightBtnDown();
                            }
                        }
                    }
                }

            }
        }
        else
        {
            if(right && walking)
            {
                RightBtnUp();
            }
            else
            {
                LeftBtnUp();
                
            }
          
        }
    }

    private bool IsPointerOverUIObject(Vector2 position)
    {
        PointerEventData pointerEventData = new PointerEventData(eventSystem)
        {
            position = position
        };

        List<RaycastResult> raycastResults = new List<RaycastResult>();
        uiRaycaster.Raycast(pointerEventData, raycastResults);
        return raycastResults.Count > 0;
    }

    public void LeftBtnDown()
    {
        SoundManager.Instance.PlayPLAYERSFX(SoundManager.PLAYER_SFX_CLIP.WALK);
        actor.animator.SetBool("isWalk", true);
        actor.spriteRenderer.flipX = true;
        actor.cur_status.moveDir = Vector2.left;
        actor.can_walk = true;
        right = false;
        walking = true;
    }

    public void LeftBtnUp()
    {
        SoundManager.Instance.StopPLAYERSFX();
        actor.animator.SetBool("isWalk", false);
        actor.cur_status.moveDir = Vector2.zero;
        actor.can_walk = false;
        walking = false;
    }

    public void RightBtnDown()
    {
        SoundManager.Instance.PlayPLAYERSFX(SoundManager.PLAYER_SFX_CLIP.WALK);
        actor.animator.SetBool("isWalk", true);
        actor.spriteRenderer.flipX = false;
        actor.cur_status.moveDir = Vector2.right;
        actor.can_walk = true;
        right = true;
        walking = true;
    }

    public void RightBtnUp()
    {
        SoundManager.Instance.StopPLAYERSFX();
        actor.animator.SetBool("isWalk", false);
        actor.cur_status.moveDir = Vector2.zero;
        actor.can_walk = false;
        walking = false;
    }
}
