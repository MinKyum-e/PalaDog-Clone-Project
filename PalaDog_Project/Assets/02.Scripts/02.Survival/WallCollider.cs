using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; //import

public class WallCollider : MonoBehaviour
{
    public float recoilDistance = 1.5f;
    public float recoilDuration = 0.2f;
    public Vector2 recoilDirection;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" || collision.tag == "Player" || collision.tag == "Minion")
        { 
            Actor actor = collision.GetComponent<Actor>();
            actor.rigid.DOMove(actor.rigid.position + recoilDirection * recoilDistance, recoilDuration);
            
            actor.cur_status.moveDir = Vector2.zero;
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" || collision.tag == "Player" || collision.tag == "Minion")
        {
            Actor actor = collision.GetComponent<Actor>();
            actor.rigid.DOMove(actor.rigid.position + recoilDirection * recoilDistance, recoilDuration);
            actor.cur_status.moveDir = Vector2.zero;
        }
    }

}
