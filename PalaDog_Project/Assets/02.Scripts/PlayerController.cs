using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;


    private bool moveRight = true;
    private bool idle = true;

    private Vector3 moveVelocity = Vector2.zero;
    private float moveSpeed = 10;
    private void Awake()
    {
        animator = GetComponent<Animator>();  
        spriteRenderer = GetComponent<SpriteRenderer>();    
    }
    private void Update()
    {
        if (idle)
        {
            animator.SetBool("isWalk", false);
            moveVelocity = Vector2.zero;
        }
        else
        {
            animator.SetBool("isWalk", true);
            if (moveRight)
            {
                spriteRenderer.flipX = true;
                moveVelocity = new Vector3(1, 0, 0);
                transform.position += moveVelocity*moveSpeed *Time.deltaTime;
            }
            else
            {
                spriteRenderer.flipX = false;
                moveVelocity = new Vector3(-1, 0, 0);
                transform.position += moveVelocity * moveSpeed * Time.deltaTime;
            }
        }    
    }

    public void setMoveRight(bool x)
    {
        moveRight = x;
    }

    public void setIdle(bool x)
    {
        idle = x;
    }
}
