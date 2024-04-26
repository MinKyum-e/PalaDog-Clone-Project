
using UnityEngine;

public class Player: MonoBehaviour
{
    public Actor actor;
    public Actions action;
    private static Player instance;
    int auraLV;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            actor = GetComponent<Actor>();
            action = GetComponent<Actions>();
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
    }

    private void Update()
    {
        if (actor.cur_status.HP <= 0)
        {
            Die();
        }
    }
    private void FixedUpdate()
    {
        action.Move();

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
        actor.isWalk = false;
        actor.cur_status.moveDir = Vector2.zero;
        GameManager.Instance.GameOver();
    }

    public void setStatus()
    {
        try
        {
            actor.status = Parser.minion_status_dict[actor.ID].common;
            actor.cur_status = Parser.minion_status_dict[actor.ID].common;
            auraLV = 1;
        }
        catch { Debug.Log("status Setting Error Player"); }
        
    }


}
