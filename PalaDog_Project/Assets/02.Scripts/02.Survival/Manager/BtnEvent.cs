using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnEvent : MonoBehaviour
{
    // Start is called before the first frame update
    private Player player;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        player = Player.Instance;
        spriteRenderer = player.GetComponent<SpriteRenderer>();
    }

    public void LeftBtnDown()
    {
        player.actor.animator.SetBool("isWalk", true);
        spriteRenderer.flipX = true;
        player.actor.cur_status.moveDir = Vector2.left;
        player.actor.can_walk = true;
    }
    public void LeftBtnUp() 
    {
        player.actor.animator.SetBool("isWalk", false);
        player.actor.cur_status.moveDir = Vector2.zero ;
        player.actor.can_walk = false;
    }

    public void RightBtnDown()
    {
        player.actor.animator.SetBool("isWalk", true);
        spriteRenderer.flipX = false;
        player.actor.cur_status.moveDir = Vector2.right;
        player.actor.can_walk = true;
    }

    public void RightBtnUp() 
    {
        player.actor.animator.SetBool("isWalk", false);
        player.actor.cur_status.moveDir = Vector2.zero;
        player.actor.can_walk = false;
    }

    public void Restart()
    {
        GameManager.Instance.RestartGame();
    }
    public void ChapterChange()
    {
        GameManager.Instance.ChangeChapter();
    }

    public void ShowMeTheGold()
    {
        GameManager.Instance.cur_gold += 10000;
    }

    public void ShowMeTheFood()
    {
        GameManager.Instance.cur_food += 1000;
    }

}
