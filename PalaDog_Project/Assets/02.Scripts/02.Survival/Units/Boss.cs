using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss: MonoBehaviour
{
    Actor actor;
    Actions action;
    PoolManager poolManager;
    GameObject player;
    GameObject atkTarget;
    public int grade;
    public int gold;
    private void Awake()
    {
        actor = GetComponent<Actor>();
        poolManager = GameObject.FindGameObjectWithTag("MinionPool").GetComponent<PoolManager>();
        player = GameObject.FindGameObjectWithTag("Player");
        action = GetComponent<Actions>();
    }
    private void OnEnable()
    {
        setStatus();
        actor.cur_status.HP = actor.status.HP;
        atkTarget = null;
        StartCoroutine(NormalAttack());

    }
    private void Update()
    {
        if (actor.cur_status.HP <= 0)
        {
            if(GameManager.Instance.chapter ==  GameManager.Instance.MAX_CHAPTER)
            {
                GameManager.Instance.state = GameState.GAME_CLEAR;
            }
            else
            {
                GameManager.Instance.state = GameState.GAME_CHAPTER_CLEAR;
            }
            
            Die();
        }
    }

    void FixedUpdate()
    {
        if (actor.can_action)
        {
            action.SetMoveDir("Player");
            action.Move();
        }

    }

    public void setStatus()
    {
        try
        {
            actor.status = Parser.enemy_status_dict[actor.ID].common;
            actor.cur_status = Parser.enemy_status_dict[actor.ID].common;
            gold = Parser.enemy_status_dict[actor.ID].gold;
        }
        catch { Debug.Log("status Setting Error"); }
    }
    public void Die()
    {
        actor.can_action = false;
        atkTarget = null;
        gameObject.SetActive(false);
        gameObject.transform.position = new Vector3(100, 0, 0);
        GameManager.Instance.UpdateGold(gold);
    }

    public IEnumerator NormalAttack()
    {
        while (true)
        {
            //타켓지정
            atkTarget = setAttackTarget();

            //attack
            if (atkTarget != null)
            {
                actor.can_action = false;
                atkTarget.GetComponent<SpriteRenderer>().color = Color.red;//추후 애니메이션 적용
                atkTarget.GetComponent<Actions>().Hit(actor.cur_status.atk);
                yield return new WaitForSeconds(actor.cur_status.atkSpeed);
                atkTarget.GetComponent<SpriteRenderer>().color = Color.white;
            }
            else
            {
                actor.can_action = true;
            }
            yield return null;
        }
    }
    public GameObject setAttackTarget()
    {

        GameObject target = null;
        //주인공 공격할 수 있으면 공격
        float dist;
        try
        {
            dist = Utils.DistanceToTarget(player.transform.position, transform.position);
            if (dist <= actor.cur_status.atkRange)
            {
                return player;
            }

        }
        catch
        {
            print("SetAttackTarget: maintarget missing set diff 99999");
            dist = 99999999;
        }

        if (atkTarget != null && Utils.DistanceToTarget(atkTarget.transform.position, transform.position) <= actor.cur_status.atkRange)
        {
            return atkTarget;
        }

        foreach (List<GameObject> units in poolManager.pools)
        {
            foreach (GameObject u in units)
            {
                if (!u.activeSelf) { continue; }
                float tmp_dist = Utils.DistanceToTarget(u.transform.position, transform.position);
                if (tmp_dist < dist && tmp_dist <= actor.cur_status.atkRange)
                {
                    dist = tmp_dist;
                    target = u;
                }
            }
        }
        if (target != null)
        {
            atkTarget = target;
            return atkTarget;
        }
        else { return null; }
    }

}
