using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public int unitID = 99;
    public string Name = "player";
    public int HP = 100;
    public int curHP =100;

    private Animator animator;
    private bool idle = true;
    private Vector2 moveDir = Vector2.zero;
    private Rigidbody2D rigid;
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private float moveSpeed = 10;


    private void Awake()
    {
        animator = GetComponent<Animator>();  
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer  = GetComponent<SpriteRenderer>();   
    }

    private void Update()
    {
        if (curHP <= 0)
        {
            Die();
        }
    }

    private void FixedUpdate()
    {
        if (idle)
        {
            animator.SetBool("isWalk", false);
        }
        else
        {
            animator.SetBool("isWalk", true);


        }
        Vector3 nextPos = rigid.position + moveDir * moveSpeed * Time.fixedDeltaTime;
        rigid.MovePosition(nextPos);

    }

    public void setMoveDir(Vector2 d)
    {
        moveDir = d;
    }

    public void setIdle(bool x)
    {
        idle = x;
    }

    public void Hit(int Damage)
    {

        spriteRenderer.color = Color.red;
        curHP -= Damage;

    }

    void Die()
    {
        print("Game Over");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
