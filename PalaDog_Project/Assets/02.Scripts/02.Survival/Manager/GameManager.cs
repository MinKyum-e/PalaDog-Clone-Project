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
    DRAGING,
    
}

public class GameManager : MonoBehaviour
{
    public int STAGE_PER_CHAPTER;
    public int MAX_CHAPTER;
    public int MAX_COST;

    private Skill skill;

    public int Unit_LvL = 0;
    //히어로 오브젝트 관리 draggable 소환시 추가, die 시 제거,
    public Dictionary<MinionUnitIndex, Minion> hero_objects;

    private static GameManager instance = null;
    public float overdrive_time = 300f;
    public bool can_get_gold = true;
    public float overdrive_timer = 0f;
    public int chapter = 1;
    public int wave = 1;
    public int stage = 1;
    private int _cur_cost= 0;
    private int _cur_gold = 0;
/*    private float _cur_food = 0;*/
    private float _food_per_time = 1;
    private bool bgm_play = false;
    public ParticleSystem spawnCloud;
    public ParticleSystem poisonFog;

    public float main_camera_size = 7;


    public bool spawn_ui_update ;



    public GameState state;

    private Vector3 player_defualt_position;

    bool save_data_exists ;

    private void Awake()
    {
        if(null == instance) //게임 완전처음시작할때만
        {
            instance = this;
            hero_objects = new Dictionary<MinionUnitIndex, Minion>();
            skill = GetComponent<Skill>();
            DontDestroyOnLoad(gameObject);
            save_data_exists = PlayerPrefs.HasKey("savedata");
            if(!save_data_exists)
            {
                PlayerPrefs.SetInt("savedata", 1);
                PlayerPrefs.SetInt("Chapter", 1);
                PlayerPrefs.SetInt("Stage", 1);
                PlayerPrefs.SetInt("Gold", 0);
                PlayerPrefs.SetInt(EnforceType.MAX_Cost.ToString(),1);
                PlayerPrefs.SetInt(EnforceType.Unit_LvL.ToString(), 1);
                PlayerPrefs.SetInt(EnforceType.Aura.ToString(), 1);
                PlayerPrefs.Save();
            }
            else
            {
                chapter = PlayerPrefs.GetInt("Chapter");
                stage = PlayerPrefs.GetInt("Stage");
                cur_gold = PlayerPrefs.GetInt("Gold");

            }
                
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
        
        cur_cost = 0;
        overdrive_timer = overdrive_time; 

    }

    public void PlayParticleEffect(Vector3 pos)
    {
        GameManager.Instance.spawnCloud.gameObject.SetActive(true);
        GameManager.Instance.spawnCloud.transform.position = pos;
        GameManager.Instance.spawnCloud.Play();
        Invoke("EndEffect", 1.0f);
    }

    public void EndEffect()
    {
        GameManager.Instance.spawnCloud.Stop();
    }
    

    private void Update()
    {
/*        if(save_data_exists)
        {
            //데이터 불러오기
            save_data_exists = false;
            
            *//*ShopManager.Instance.GetEnforceValue(EnforceType.MAX_Cost, PlayerPrefs.GetInt("MAXCOST"));
            ShopManager.Instance.GetEnforceValue(EnforceType.Unit_LvL, PlayerPrefs.GetInt("UNITLVL"));
            ShopManager.Instance.GetEnforceValue(EnforceType.Aura, PlayerPrefs.GetInt("AURA"));*//*
        }*/
        if(!bgm_play)
        {
            SoundManager.Instance.PlayBGM(SoundManager.BGM_CLIP.ingame);
            UIManager.Instance.SetCurrentPage(UIPageInfo.GamePlay);
            bgm_play = true;
        }
        
        if (overdrive_timer <= 0)
        {
            can_get_gold = false;
        }
        else
        {
            overdrive_timer -= Time.deltaTime;
        }
/*        if (cur_food < MAX_FOOD)
            cur_food +=Time.deltaTime * _food_per_time;*/
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
/*            case GameState.GAME_STAGE_CLEAR:
            case GameState.GAME_CHAPTER_CLEAR:
                StageClear();
                break;*/
            case GameState.GAME_CLEAR:
                GameClear();
                break;
            case GameState.GAME_IDLE:
                break;

            default: 
                break;
        }
        
    }

    public bool CheckHeroExists(MinionUnitIndex idx)
    {
        if(hero_objects.TryGetValue(idx, out var hero))
        {
            if (hero != null)
                return true;
        }
        return false;

    }
    public void AddHeroUnit(Minion minion)
    {
        print("adddd");
        hero_objects[(MinionUnitIndex)minion.actor.ID] = minion;
    }
    public void DeleteHeroUnit(MinionUnitIndex idx)
    {
        spawn_ui_update = true;
        hero_objects[idx] = null;
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
        food_per_time = ShopManager.Instance.GetEnforceValue(EnforceType.Gain_Food, 0);
        
    }
    public void PauseGame()
    {
        Time.timeScale = 0;
        state = GameState.GAME_IDLE;
        UIManager.Instance.SetCurrentPage(UIPageInfo.GamePause);

    }
    public void ContinueGame()
    {
        Time.timeScale = 1;
        state = GameState.GAME_PLAY;
        UIManager.Instance.SetCurrentPage(UIPageInfo.GamePlay);
    }

    

    
/*    public void RestartGame()
    {

        *//*Time.timeScale = 1;
        WaveManager.Instance.ClearMonsterObjectOnStage();
        wave = 1;
*//*        stage = 1;
        chapter = 1;*//*
        cur_cost = 0;
*//*        cur_gold = 0;
        Unit_LvL = 0;*//*
        overdrive_timer = overdrive_time;
        can_get_gold = true;
        //데이터 초기화

*//*        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();*/
        /*        cur_food = 0;*//*
        EnemyBase.Instance().actor.cur_status.HP = EnemyBase.Instance().actor.status.HP;
        UIManager.Instance.SetCurrentPage(UIPageInfo.GamePlay);

        //ShopManager.Instance.ClearInGameShop();
       // Player.Instance.actor.status = Parser.minion_status_dict[(int)MinionUnitIndex.Player].common;
        Player.Instance.actor.cur_status = Player.Instance.actor.status;
        Player.Instance.transform.Find("Buff").GetComponent<BuffSystem>().buff_init();
        Player.Instance.transform.position = player_defualt_position;
        Player.Instance.actor.can_action = true;
        state = GameState.GAME_PLAY;
        spawnCloud.gameObject.SetActive(false);
        poisonFog.gameObject.SetActive(false);*//*
        SceneManager.LoadScene("Chapter" + chapter);
       
    }*/
    public void GameOverTitle()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
    public void GameOver()
    {
        WaveManager.Instance.ClearMonsterObjectOnStage();
        UIManager.Instance.SetCurrentPage(UIPageInfo.GameOver);
        //데이터 삭제
        //SceneManager.LoadScene("GameOver");
        state = GameState.GAME_IDLE;
        Time.timeScale = 0;
    }

    public void WaveChange(int wave)
    {

        this.wave = wave;
        List<GameObject> target_list = new List<GameObject>();
        foreach(var pool in Player.Instance.actor.minion_poolManager.pools)
        {
            foreach(var obj in pool)
            {
                if (obj.activeSelf)
                    target_list.Add(obj);
            }
            
        }

        skill.NoneTypeBuff(BuffName.KnockBack, 20f, 1, target_list);


    }

    public bool StageClear()
    {
        if(state != GameState.DRAGING)
        {
            state = GameState.GAME_STAGE_CLEAR;

            UIManager.Instance.SetCurrentPage(UIPageInfo.GameStageClear);
            WaveManager.Instance.ClearMonsterObjectOnStage();
            Time.timeScale = 0;
            return true;
        }
        return false;
        
    }


    public void StageChange()
    {
        Time.timeScale = 1;
        state = GameState.GAME_PLAY;
        
        if (stage % STAGE_PER_CHAPTER != 0)
        {
            stage++;
            wave = 1;
            SceneManager.LoadScene("Chapter" + chapter);
        }
        else
        {
            chapter = stage / STAGE_PER_CHAPTER + 1;
            stage++;
            wave = 1;
            SceneManager.LoadScene("Chapter" + chapter);
        }

        //데이터저장
        PlayerPrefs.SetInt("Chapter", chapter);
        PlayerPrefs.SetInt("Stage", stage);
        PlayerPrefs.SetInt("Gold", cur_gold);
        PlayerPrefs.SetInt(EnforceType.MAX_Cost.ToString(), ShopManager.Instance.ingame_enforce_cur_lvl[EnforceType.MAX_Cost] );
        PlayerPrefs.SetInt(EnforceType.Unit_LvL.ToString(), ShopManager.Instance.ingame_enforce_cur_lvl[EnforceType.Unit_LvL] );
        PlayerPrefs.SetInt(EnforceType.Aura.ToString(), ShopManager.Instance.ingame_enforce_cur_lvl[EnforceType.Aura]);
        PlayerPrefs.Save();

        cur_cost = 0;
        Player.Instance.transform.position = player_defualt_position;
        Player.Instance.actor.cur_status.HP = Player.Instance.actor.status.HP;
        Player.Instance.transform.Find("Buff").GetComponent<BuffSystem>().buff_init();
        EnemyBase.Instance().gameObject.SetActive(true);
        EnemyBase.Instance().GetComponent<Actor>().isDie = false;
        overdrive_timer = overdrive_time;
        can_get_gold = true;
    }



    public void GameClear()
    {
        WaveManager.Instance.ClearMonsterObjectOnStage();
        UIManager.Instance.FadeOut.SetActive(true);
        //데이터 삭제
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
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
/*    public float cur_food
    {
        get { return _cur_food;}
        set { _cur_food = value; }
    }*/
    public int cur_gold
    {
        get { return _cur_gold; }
        set { _cur_gold = value; }
    }
    public bool CheckCost(int cost)
    {
        return (cur_cost + cost <= MAX_COST);
    }

/*    public bool CheckFood()
    {
        return (cur_food <= MAX_FOOD);
    }*/
}
