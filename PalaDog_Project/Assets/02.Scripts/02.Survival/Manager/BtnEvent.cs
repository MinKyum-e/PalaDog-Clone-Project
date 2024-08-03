using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BtnEvent : MonoBehaviour
{
    public void GameClear()
    {
        GameManager.Instance.GameClear();
    }
    public void StageClear()
    {
        GameManager.Instance.StageChange();
    }

    public void SpawnStageBoss(int chapter)
    {
        GameManager.Instance.chapter = chapter;
        GameManager.Instance.stage = 5 * chapter;
        GameManager.Instance.wave = 4;
    }

    public void ChapterChange(int chapter)
    {
        WaveManager.Instance.ClearMonsterObjectOnStage();
        GameManager.Instance.chapter = chapter;
        GameManager.Instance.stage = ((chapter-1)*5) + 1;
        GameManager.Instance.wave = 1;
        SceneManager.LoadScene("Chapter" + chapter); 
    }


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

    public void ClearManagers()
    {
        Destroy(Player.Instance.actor.minion_poolManager.gameObject);
        Destroy(Player.Instance.actor.enemy_poolManager.gameObject);
        Destroy(Player.Instance.gameObject);

        Destroy(SoundManager.Instance.gameObject);
        Destroy(ShopManager.Instance.gameObject);

        Destroy(ArrowPool.Instance.gameObject);
        Destroy(GameManager.Instance.gameObject);
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
