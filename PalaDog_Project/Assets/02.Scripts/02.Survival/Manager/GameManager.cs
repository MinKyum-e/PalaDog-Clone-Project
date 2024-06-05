using ShopEnums;
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
    private int _cur_cost= 0;
    private int _cur_gold = 0;
    private float _cur_food = 0;
    private float _food_per_time = 1;
    

    public GameState state;

    private Vector3 player_defualt_position;

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
        player_defualt_position = Player.Instance.transform.position ;
        state = GameState.GAME_PLAY;
        Time.timeScale = 1.0f;
        GameObject.Find("EnemyBase").SetActive(true);
        UIManager.Instance.SetCurrentPage(UIPageInfo.GamePlay);
        cur_cost = 0;
        cur_gold = 0;
    }

    private void Update()
    {
        if (cur_food < MAX_FOOD)
            cur_food +=Time.deltaTime * _food_per_time;
        //게임 로직
        switch (state)
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

    void ResetBaseStat()
    {
        MAX_COST = ShopManager.Instance.GetEnforceValue(EnforceType.MAX_Cost, 0);
        MAX_FOOD = ShopManager.Instance.GetEnforceValue(EnforceType.MAX_Food, 0);
        food_per_time = ShopManager.Instance.GetEnforceValue(EnforceType.Gain_Food, 0);
        
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
        cur_cost = 0;
        cur_gold = 0;
        cur_food = 0;
        EnemyBase.Instance().actor.cur_status.HP = EnemyBase.Instance().actor.status.HP;
        UIManager.Instance.SetCurrentPage(UIPageInfo.GamePlay);
        Player.Instance.actor.cur_status.HP = Player.Instance.actor.status.HP;
        Player.Instance.transform.position = player_defualt_position;
        Player.Instance.actor.can_action = true;
        state = GameState.GAME_PLAY;
        SceneManager.LoadScene("Chapter1");

       
    }
    public void GameOver()
    {
        WaveManager.Instance.ClearMonsterObjectOnStage();
        UIManager.Instance.SetCurrentPage(UIPageInfo.GameOver);
        //SceneManager.LoadScene("GameOver");
        Time.timeScale = 0;
        state = GameState.GAME_IDLE;
    }

    public void WaveChange()
    {
        wave++;
    }

    public void StageClear()
    {
        //clear 이벤트 만들기

        WaveManager.Instance.ClearMonsterObjectOnStage();
        cur_cost = 0;
        cur_food = 0;
        stage++;
        wave = 1;
        Player.Instance.transform.position = player_defualt_position;
        Player.Instance.actor.cur_status.HP = Player.Instance.actor.status.HP;
        state = GameState.GAME_PLAY;
        EnemyBase.Instance().gameObject.SetActive(true);
        EnemyBase.Instance().GetComponent<Actor>().isDie = false;

    }
    public void ChangeChapter()
    {
        WaveManager.Instance.ClearMonsterObjectOnStage();
        Time.timeScale = 1;
        state = GameState.GAME_PLAY;
        wave = 1;
        cur_cost = 0;
        cur_food = 0;
        Player.Instance.transform.position = player_defualt_position;
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
        Player.Instance.transform.position = player_defualt_position;
        Time.timeScale = 0;
        
    }
    

    public void GameClear()
    {
        Time.timeScale = 0;
        UIManager.Instance.SetCurrentPage(UIPageInfo.GameClear);
    }


    public float food_per_time
    {
        get { return _food_per_time; }
        set { _food_per_time = value; }
    }
   

    public int cur_cost
    {
        get { return _cur_cost; }
        set { _cur_cost = value; }
    }
    public float cur_food
    {
        get { return _cur_food;}
        set { _cur_food = value; }
    }
    public int cur_gold
    {
        get { return _cur_gold; }
        set { _cur_gold = value; }
    }
    public bool CheckCost(int cost)
    {
        return (cur_cost + cost <= MAX_COST);
    }

    public bool CheckFood()
    {
        return (cur_food <= MAX_FOOD);
    }
}
