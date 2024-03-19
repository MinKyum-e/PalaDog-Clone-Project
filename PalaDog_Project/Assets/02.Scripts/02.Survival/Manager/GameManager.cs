using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public enum GameState
{
    GAME_OVER, 
    GAME_PLAY, 
    GAME_PAUSE,
    GAME_STAGE_CLEAR
}

public class GameManager : MonoBehaviour
{
    public int MAX_STAGE ;

    private static GameManager instance = null;
    public Player_fix player;
    public EnemyBase enemyBase;
    public PoolManager enemy_pool;
    public PoolManager minion_pool;
    public Parser parser;
    public UIManager uiManager;
    public Stage_text st;
    public WaveManager waveManager;
    
    public int wave = 1;
    public int stage = 1;
    

    public GameState state;
    

    private void Awake()
    {
        if(null == instance)
        {
            instance = this;

        }
        else
        {//씬 이동됐는데 게임 매니저가 존재할 경우 자신을 삭제
            Destroy(this.gameObject );
        }
    }
    private void Start()
    {
        InitGame();
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

    public void InitGame()
    {
        Time.timeScale = 1.0f;
        player.transform.position = Vector3.zero;
        state = GameState.GAME_PLAY;
        uiManager.SetCurrentPage(UIPageInfo.GamePlay);
        wave = 1;
        stage = 1;
        st.stageWaveUpdate(stage, wave);

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
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void GameOver()
    {
        uiManager.SetCurrentPage(UIPageInfo.GameOver);
        Time.timeScale = 0;
        
    }

    public void WaveChange()
    {
        wave++;
        st.stageWaveUpdate(stage, wave);
    }

    public void StageClear()
    {
        if( stage != MAX_STAGE)
        {
            
            stage++;
            wave = 1;
            waveManager.ClearMonsterObjectOnStage();
            player.transform.position = Vector3.zero;
            state = GameState.GAME_PLAY;
            st.stageWaveUpdate(stage, wave);
            enemyBase.gameObject.SetActive(true);

        }
        else
        {
            uiManager.SetCurrentPage(UIPageInfo.GameStageClear);
            Time.timeScale = 0;
        }
        
        
    }
}
