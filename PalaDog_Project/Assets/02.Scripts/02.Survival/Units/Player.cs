
using UnityEngine;

public class Player: MonoBehaviour
{
    public Actor actor;
    public Actions action;
    private static Player instance;
    public Transform aura;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;

            DontDestroyOnLoad(gameObject);
            actor = GetComponent<Actor>();
            action = GetComponent<Actions>();
            aura = transform.Find("Aura");
        }
        else
        {
            Destroy(gameObject);
        }
        
    }
    private void Start()
    {
        setStatus();
        actor.cur_status.HP = actor.status.HP;
        actor.cur_status.moveDir = Vector2.zero;
    }

    private void Update()
    {
        //키보드 입력
       /* float moveHorizontal = Input.GetAxisRaw("Horizontal");
        actor.spriteRenderer.flipX = moveHorizontal < 0;
       actor.cur_status.moveDir = new Vector2 (moveHorizontal, 0);*/

        if (actor.cur_status.HP <= 0 && actor.can_action)
        {
            actor.can_action = false;
            actor.animator.Play("Die");
        }
    }
    private void FixedUpdate()
    {
        if(actor.can_action)
        {
            action.Move();
        }

    }

    public void SetAuraRange(int value)
    {
        aura.localScale = new Vector3(value, value, value);
    }

    

    public static Player Instance //게임매니저 인스턴스 접근
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }
    public void Die()
    {

        actor.cur_status.moveDir = Vector2.zero;
        actor.can_action = true;
        GameManager.Instance.GameOver();
        
    }

    public void setStatus()
    {
        try
        {
            actor.status = Parser.minion_status_dict[actor.ID].common;
            actor.cur_status = Parser.minion_status_dict[actor.ID].common;
            aura.localScale = new Vector3(5, 5, 5);
        }
        catch { Debug.Log("status Setting Error Player"); }
        
    }

    

}
