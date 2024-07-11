
using UnityEngine;
using UnityEngine.U2D.Animation;
using UnityEngine.EventSystems;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
using System.Collections.Generic;

public class Player: MonoBehaviour
{
    public Actor actor;
    public Actions action;
    public static Player instance;
    public Transform aura;
    public AuraSkill aura_skill;
    public CircleCollider2D auraCollider;
    SpriteResolver aura_spriteResolver;
    

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;

            DontDestroyOnLoad(gameObject);
            actor = GetComponent<Actor>();
            action = GetComponent<Actions>();
            aura = transform.Find("Aura");
            aura_spriteResolver = aura.GetComponent<SpriteResolver>();
            aura_spriteResolver.SetCategoryAndLabel("Aura", "lvl1");
            aura_skill = aura.GetComponent<AuraSkill>();
            auraCollider = aura.GetComponent<CircleCollider2D>();   
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
        if (actor.cur_status.HP <= 0 && actor.can_action)
        {
            actor.can_action = false;
            actor.animator.Play("Die");
        }

    }
    private void FixedUpdate()
    {
        Touch tempPos;
        if (Input.touchCount > 0)
        {
             for(int i=0;i<Input.touchCount;i++)
            {
                tempPos = Input.GetTouch(i);
                print(tempPos);
            }
        }

        if(actor.can_action)
        {
            action.Move();
        }
    }

    public void SetAuraRange(int value, int lvl)
    {
        aura.localScale = new Vector3(value, value, value);
        aura_spriteResolver.SetCategoryAndLabel("Aura", "lvl" + lvl);
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
