using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss: Enemy
{

    protected override void OnEnable()
    {
        base.OnEnable();
        BossHPUI.Instance.SetTarget(gameObject);
    }
    public override void Die()
    {
        
        if(GameManager.Instance.chapter == GameManager.Instance.MAX_CHAPTER)
        {
            GameManager.Instance.StageClear();
        }
        else
        {
            GameManager.Instance.StageClear();
        }
        actor.spriteRenderer.color = Color.white;

        gameObject.SetActive(false);
        gameObject.transform.position = new Vector3(100, 0, 0);
    }

}
