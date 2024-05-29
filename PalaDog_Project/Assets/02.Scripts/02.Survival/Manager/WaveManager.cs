
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

/// <summary>
/// WaveTable 
///public int Wave_Group;                 //동일 웨이브 내 몬스터 그룹화를 위한 그룹 인덱스
///public int Wave_StageNum;              //웨이브가 충현하는 스테이지 번호
///public int Wave_WaveNum;               //스테이지 내의 웨이브 순서 번호
///public WaveType Wave_WaveType;
///public float Wave_SpawnTime;           //웨이브의 재출현 주기
///public int Wave_MonsterIndex;          //웨이브에 포함할 몬스터의 고유 인덱스
///public int Wave_MonsterNum;            //웨이브에 포함할 몬스터 종류 별 개체 수*/
/// 
/// </summary>

public class WaveMonster
{
    public int monster_index;
    public int monster_num;
    public float monster_spawnTime;
    public float last_spawnTime;

    public WaveMonster(int wave_MonsterIndex, int wave_MonsterNum, float wave_SpawnTime)
    {
        this.monster_index = wave_MonsterIndex;
        this.monster_num = wave_MonsterNum;
        this.monster_spawnTime = wave_SpawnTime;
        this.last_spawnTime = -99999999;
    }
};

public class WaveManager : MonoBehaviour
{
    private static WaveManager instance; 
    public int cur_stageNum;
    public int cur_waveNum;
    public List<WaveMonster> monster_list;
    
    public List<Coroutine> coroutine_list ;
    private PoolManager monsterPool;
    public WaveType wave_type = WaveType.Normal;
    int sort_num;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this; 
        }
        cur_stageNum = 0;
        cur_waveNum = 0;
        monster_list = new List<WaveMonster>();
        coroutine_list = new List<Coroutine>();
        monsterPool = GameObject.FindGameObjectWithTag("EnemyPool").GetComponent<PoolManager>();

    }
    public static WaveManager Instance
    {
        get{
            if (instance == null)
                return null;
            return instance;
        }
    }



    private void Update()
    {
        if(GameManager.Instance.stage != cur_stageNum || GameManager.Instance.wave != cur_waveNum) //monsterlist 세팅
        {
            //리스트 초기화
            monster_list.Clear();
            cur_stageNum = GameManager.Instance.stage;
            cur_waveNum = GameManager.Instance.wave;
            foreach(Coroutine c in coroutine_list) { StopCoroutine(c); }

            //monsterlist 세팅
            foreach(KeyValuePair<int, WaveInfo>  waveTable in Parser.wave_info_dict)
            {
                if(waveTable.Value.Wave_StageNum == cur_stageNum && waveTable.Value.Wave_WaveNum == cur_waveNum)
                {
                    monster_list.Add(new WaveMonster(waveTable.Value.Wave_MonsterIndex, waveTable.Value.Wave_MonsterNum, waveTable.Value.Wave_SpawnTime));
                    wave_type = waveTable.Value.Wave_WaveType;

                }
            }
        }
        else 
        {
            //spawn time, last spawn time 비교 후 소환
            foreach(WaveMonster monster in monster_list)
            {
                if((Time.time - monster.last_spawnTime) > monster.monster_spawnTime )
                {
                    coroutine_list.Add(StartCoroutine(SpawnMonster(monster.monster_index, monster.monster_num)));
                    monster.last_spawnTime = Time.time;
                }
            }
        }
    }

    IEnumerator SpawnMonster(int idx, int num)
    {
        for(int i=0;i<num;i++)
        {
            GameObject clone = monsterPool.Get(idx, transform.position);
            Color c = clone.GetComponent<SpriteRenderer>().color;
            //clone.GetComponent<SpriteRenderer>().color = new Color(c.r, c.g,  c.b, 1);
            clone.tag = "Enemy";
            yield return new WaitForSeconds(0.5f);
        }
        yield return null;
    }


    private void ClearMonsterList()
    {
        monster_list.Clear();
    }


    public void ClearMonsterObjectOnStage()
    {
        GameObject[] alive_enemys = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] alive_minions = GameObject.FindGameObjectsWithTag("Minion");
        foreach(GameObject e in alive_enemys)
        {
            if(e.activeSelf)
             e.GetComponent<Enemy>().Die();
        }
        foreach (GameObject m in alive_minions)
        {
            if (m.activeSelf)
                m.GetComponent<Minion>().Die();
        }
    }
}
