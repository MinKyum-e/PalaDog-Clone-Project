using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDamageTrigger : MonoBehaviour
{
    Actor actor;
    public string target_tag;
    private void Awake()
    {
        actor = GetComponent<Actor>();
    }

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("target : "+ collision.name);
        if(collision.gameObject.activeSelf && target_tag == collision.tag && Buff.CheckAttackIgnore(collision.GetComponent<Actor>().cur_buff, actor.cur_status.job))
            collision.GetComponent<Actions>().Hit(actor.cur_status.atk);
    }
}
