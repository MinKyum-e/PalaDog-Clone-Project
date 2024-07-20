using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public void Restart()
    {
        GameManager.Instance.RestartGame();
    }



    public void ShowMeTheGold()
    {
        GameManager.Instance.cur_gold += 10000;
    }

    public void GoTitle()
    {
        Destroy(Player.Instance.gameObject);
        Destroy(SoundManager.Instance.gameObject);
        Destroy(Parser.Instance.gameObject);
        Destroy(ShopManager.Instance.gameObject);
        Destroy(GameManager.Instance.gameObject);
        SceneManager.LoadScene("Title");
    }

    /*    public void ShowMeTheFood()
        {
            GameManager.Instance.cur_food += 1000;
        }*/

}
