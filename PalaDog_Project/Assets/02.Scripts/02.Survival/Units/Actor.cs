
using UnityEngine;



public class Actor : MonoBehaviour
{
    public int ID;
    public CommonStatus status;
    
    public CommonStatus cur_status;
    public BuffStruct cur_buff = new BuffStruct();
    public PoolManager enemy_poolManager;
    public PoolManager minion_poolManager;




    public GameObject atkTarget;
    public GameObject skillTarget;
    public bool isDie = false;
    public bool is_faint = false;
    public bool can_walk = true;
    public bool can_action = true;
    public bool can_use_skill = false;

    public SkillInfo skill_info;
    public Rigidbody2D rigid;
    public SpriteRenderer spriteRenderer; 
    public Animator animator;

    public int final_damage;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        enemy_poolManager = GameObject.FindGameObjectWithTag("EnemyPool").GetComponent<PoolManager>();
        minion_poolManager = GameObject.FindGameObjectWithTag("MinionPool").GetComponent<PoolManager>();

    }
    private void OnEnable()
    {
        cur_status.HP = status.HP;

        /*cur_status.moveDir = Vector2.right;*/
    }
    //Ãß»ó
    /* public abstract void Die();
     public abstract void setStatus();*/

}

