using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; //import

public class WallCollider : MonoBehaviour
{
    public float recoilDistance = 2f;
    public float recoilDuration = 0.2f;
    public Vector3 recoilDirection;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" || collision.tag == "Minion")
        {
            Actor actor = collision.GetComponent<Actor>();
            actor.transform.DOMove(actor.transform.position + recoilDirection * recoilDistance, recoilDuration);

        }
        else if (collision.tag == "Player")
        {
            Actor actor = collision.GetComponent<Actor>();
            actor.transform.DOMove(actor.transform.position + recoilDirection * recoilDistance, recoilDuration);

            actor.cur_status.moveDir = Vector2.zero;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" ||collision.tag == "Minion")
        {
            Actor actor = collision.GetComponent<Actor>();
            actor.transform.DOMove(actor.transform.position + recoilDirection * recoilDistance, recoilDuration);
            
        }
        else if (collision.tag == "Player")
        {
            Actor actor = collision.GetComponent<Actor>();
            actor.transform.DOMove(actor.transform.position + recoilDirection * recoilDistance, recoilDuration);

            actor.cur_status.moveDir = Vector2.zero;
        }
    }



}
