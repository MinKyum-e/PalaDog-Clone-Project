
using UnityEngine;

public class Actor : MonoBehaviour
{
    public int ID;
    public CommonStatus status;
    public CommonStatus cur_status;
    public bool isWalk = false;

    public Rigidbody2D rigid;
    public SpriteRenderer spriteRenderer; 
    public Animator animator;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }
    //Ãß»ó
    /* public abstract void Die();
     public abstract void setStatus();*/

}

