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
        player = GameManager.Instance.player;
        spriteRenderer = player.GetComponent<SpriteRenderer>();
    }

    public void LeftBtnDown()
    {
        spriteRenderer.flipX = false;
        player.setMoveDir(new Vector2(-1, 0));
        player.setIdle(false);
    }
    public void LeftBtnUp() 
    {
        player.setMoveDir(new Vector2(0, 0));
        player.setIdle(true);
    }

    public void RightBtnDown()
    {
        spriteRenderer.flipX = true;
        player.setMoveDir(new Vector2(1, 0));
        player.setIdle(false);
    }

    public void RightBtnUp() 
    {

        player.setMoveDir(new Vector2(0, 0));
        player.setIdle(true);
    }

}
