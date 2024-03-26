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
        spriteRenderer.flipX = false;
        player.moveDir = Vector2.left;
        player.isWalk = true;
    }
    public void LeftBtnUp() 
    {
        player.moveDir = Vector2.zero ;
        player.isWalk = false;
    }

    public void RightBtnDown()
    {
        spriteRenderer.flipX = true;
        player.moveDir = Vector2.right;
        player.isWalk = true;
    }

    public void RightBtnUp() 
    {

        player.moveDir = Vector2.zero;
        player.isWalk = false;
    }

    public void Restart()
    {
        GameManager.Instance.RestartGame();
    }
    public void ChapterChange()
    {
        GameManager.Instance.ChangeChapter();
    }

}
