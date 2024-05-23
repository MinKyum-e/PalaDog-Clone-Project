using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; //import

public class KnockBack : MonoBehaviour
{
    Actor actor;
    public string target_tag;
    public BoxCollider2D boxCollider;
    public float collider_end_x;
    

    private void Awake()
    {
        actor = GetComponent<Actor>();
        boxCollider = GetComponent<BoxCollider2D>();
        
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("target : " + collision.name);
        if (collision.gameObject.activeSelf && collision.name != "EnemyBase"&& target_tag == collision.tag && Buff.CheckAttackIgnore(collision.GetComponent<Actor>().cur_buff, actor.cur_status.job))
        {
            
            Vector3 target_position = collision.transform.position;
            if(target_position.x < boxCollider.bounds.max.x )
            {
                collision.GetComponent<Actions>().Hit(actor.cur_status.atk);
                collision.transform.DOMove(new Vector3(boxCollider.bounds.max.x, target_position.y, target_position.z), 0.3f);
                
            }
                
        }
    }
}
