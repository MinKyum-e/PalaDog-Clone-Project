using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BtnEvent : MonoBehaviour
{


    public void GamePlay()
    {
        GameManager.Instance.state = GameState.GAME_PLAY;
        UIManager.Instance.SetCurrentPage(UIPageInfo.GamePlay);
    }
    public void ChooseUnit()
    {
        UIManager.Instance.SetCurrentPage(UIPageInfo.GameStageClear);
    }

    public void Restart()
    {
        GameManager.Instance.RestartGame();
    }

    public void Pause()
    {
        GameManager.Instance.PauseGame();
    }

    public void ContinueGame()
    {
        GameManager.Instance.ContinueGame();
    }

    public void ShowMeTheGold()
    {
        GameManager.Instance.cur_gold += 10000;
    }

    public void GoTitle()
    {
        Destroy(Player.Instance.actor.minion_poolManager.gameObject);
        Destroy(Player.Instance.actor.enemy_poolManager.gameObject);
        Destroy(Player.Instance.gameObject);

        Destroy(SoundManager.Instance.gameObject);
        Destroy(ShopManager.Instance.gameObject);
        Destroy(GameManager.Instance.gameObject);
        Destroy(ArrowPool.Instance.gameObject);
        SceneManager.LoadScene("Title");
    }

    public void IntroGoTitle()
    {
        SceneManager.LoadScene("Title");
    }

    public void LoadScnene_name(string name)
    {
        SceneManager.LoadScene(name);
    }

    /*    public void ShowMeTheFood()
        {
            GameManager.Instance.cur_food += 1000;
        }*/

}
