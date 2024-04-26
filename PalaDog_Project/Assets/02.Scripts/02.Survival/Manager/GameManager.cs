using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public enum GameState
{
    GAME_IDLE,
    GAME_OVER, 
    GAME_PLAY, 
    GAME_PAUSE,
    
    GAME_STAGE_CLEAR,
    GAME_CHAPTER_CLEAR,
    GAME_CLEAR,
    
}

public class GameManager : MonoBehaviour
{
    public int STAGE_PER_CHAPTER;
    public int MAX_CHAPTER;
    public int MAX_COST;
    public int MAX_FOOD;

    private static GameManager instance = null;

    public int chapter = 1;
    public int wave = 1;
    public int stage = 1;
    private int cur_cost= 0;
    private int cur_gold = 0;
    private float cur_food = 0;
    

    public GameState state;
    

    private void Awake()
    {
        if(null == instance) //게임 완전처음시작할때만
        {
            instance = this;
            wave = 1;
            stage = 1;
            DontDestroyOnLoad(gameObject);
        }
        else
        {//씬 이동됐는데 게임 매니저가 존재할 경우 자신을 삭제
            Destroy(this.gameObject );
        }
    }
    private void Start()
    {
        Player.Instance.transform.position = Vector3.zero;
        state = GameState.GAME_PLAY;
        Time.timeScale = 1.0f;
        GameObject.Find("EnemyBase").SetActive(true);
        UIManager.Instance.SetCurrentPage(UIPageInfo.GamePlay);
        SetCost(0);
        SetGold(0);
    }

    private void Update()
    {
        //게임 로직
        switch(state)
        {
            case GameState.GAME_PLAY:

                break;
            case GameState.GAME_OVER:
                GameOver();

                break;
            case GameState.GAME_PAUSE:
                PauseGame();
                break;
            case GameState.GAME_STAGE_CLEAR:
                StageClear();
                break;
            case GameState.GAME_CHAPTER_CLEAR:
                ChapterClear();
                break;
            case GameState.GAME_CLEAR:
                GameClear();
                break;
            case GameState.GAME_IDLE:
                break;

            default: 
                break;
        }
        
    }

    public static GameManager Instance //게임매니저 인스턴스 접근
    {
        get
        {
            if(null == instance)
            {
                return null;
            }
            return instance;
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }
    public void ContinueGame()
    {

    }
    public void RestartGame()
    {
        Time.timeScale = 1;
        wave = 1;
        stage = 1;
        chapter = 1;
        SetCost(0);
        SetGold(0);
        SetFood (0);
        EnemyBase.Instance().actor.cur_status.HP = EnemyBase.Instance().actor.status.HP;
        UIManager.Instance.SetCurrentPage(UIPageInfo.GamePlay);
        Player.Instance.actor.cur_status.HP = Player.Instance.actor.status.HP;
        Player.Instance.transform.position = Vector3.zero;
        state = GameState.GAME_PLAY;
        SceneManager.LoadScene("Chapter1");

       
    }
    public void GameOver()
    {
        UIManager.Instance.SetCurrentPage(UIPageInfo.GameOver);
        Time.timeScale = 0;
        state = GameState.GAME_IDLE;
    }

    public void WaveChange()
    {
        wave++;
    }

    public void StageClear()
    {
        
        
        WaveManager.Instance.ClearMonsterObjectOnStage();
        SetCost(0);
        SetFood(0);
        stage++;
        wave = 1;
        Player.Instance.transform.position = Vector3.zero;
        Player.Instance.actor.cur_status.HP = Player.Instance.actor.status.HP;
        state = GameState.GAME_PLAY;
        EnemyBase.Instance().gameObject.SetActive(true);

    }
    public void ChangeChapter()
    {
        Time.timeScale = 1;
        state = GameState.GAME_PLAY;
        wave = 1;
        SetCost(0);
        SetFood(0);
        Player.Instance.transform.position = Vector3.zero;
        Player.Instance.actor.cur_status.HP = Player.Instance.actor.status.HP;
        state = GameState.GAME_PLAY;
        SceneManager.LoadScene("Chapter" + chapter);

    }
    public void ChapterClear()
    {
        stage++;
        wave = 1;
        //씬 체인지 구현
        chapter = stage / STAGE_PER_CHAPTER + 1;
        UIManager.Instance.SetCurrentPage(UIPageInfo.GameChapterClear);
        state = GameState.GAME_IDLE;
        Player.Instance.transform.position = Vector3.zero;
        Time.timeScale = 0;
        
    }
    public void GameClear()
    {
        Time.timeScale = 0;
        UIManager.Instance.SetCurrentPage(UIPageInfo.GameClear);
    }
    public bool CheckCost(int cost)
    {
        return (cur_cost + cost <= MAX_COST);
    }

    public void SetCost(int cost)
    {
        cur_cost = cost;
        CostUI.instance.SetCostUI();
    }
    public int GetCost() {
        return cur_cost; }

    public void UpdateCost(int cost)
    {
        cur_cost += cost;
        CostUI.instance.SetCostUI();
    }
    public bool CheckFood()
    {
        return (cur_food <= MAX_FOOD);
    }
    public void SetFood(float food)
    {
        cur_food = food;
    }
    public void UpdateFood(float food)
    {
        cur_food += food;
    }
    public float GetFood()
    {
        return cur_food;
    }

    public void UpdateGold(int gold)
    {
        cur_gold += gold;
        GoldUI.instance.SetGoldUI();
    }
    public void SetGold(int gold)
    {
        cur_gold = gold;
        GoldUI.instance.SetGoldUI();
    }
    public int GetGold()
    {
        return cur_gold;
    }
}
