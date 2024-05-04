
using UnityEngine;



public class Actor : MonoBehaviour
{
    public int ID;
    public CommonStatus status;
    
    public CommonStatus cur_status;
    public BuffStruct cur_buff = new BuffStruct();

    public bool isWalk = false;
    public bool can_use_skill = false;
    public bool can_attack = true;

    public Rigidbody2D rigid;
    public SpriteRenderer spriteRenderer; 
    public Animator animator;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

    }
    private void OnEnable()
    {
        cur_status.HP = status.HP;
        cur_status.moveDir = Vector2.right;
    }
    //Ãß»ó
    /* public abstract void Die();
     public abstract void setStatus();*/

}

