
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

public class Monster
{
    public int monster_index;
    public int monster_num;
    public float monster_spawnTime;
    public float last_spawnTime;

    public Monster(int wave_MonsterIndex, int wave_MonsterNum, float wave_SpawnTime)
    {
        this.monster_index = wave_MonsterIndex;
        this.monster_num = wave_MonsterNum;
        this.monster_spawnTime = wave_SpawnTime;
        this.last_spawnTime = 0;
    }
};

public class WaveManager : MonoBehaviour
{
    public int stageNum = 1;
    public int waveNum = 1;
    public int cur_stageNum = 1;
    public int cur_waveNum = 1;
    public List<Monster> monster_list;
    public int group_cnt = 0;



    public WaveTable[] waveTables = null;
    

    private void Start()
    {

        List<Dictionary<string, object>> wave_data = GameManager.Instance.parser.data_WaveTable;
        waveTables = new WaveTable[wave_data.Count];
        SetWaveTables(wave_data, waveTables);
        monster_list = new List<Monster>();
    }

    private void Update()
    {


        if(stageNum != cur_stageNum || waveNum != cur_waveNum) //monsterlist 세팅
        {
            //초기화
            monster_list = new List<Monster>();
            group_cnt = 0;
            cur_stageNum = stageNum;
            cur_waveNum = waveNum;
            monster_list.Clear();

            //monsterlist 세팅
            foreach(WaveTable waveTable in waveTables)
            {
                if(waveTable.Wave_StageNum == cur_stageNum && waveTable.Wave_WaveNum == cur_waveNum)
                {
                    monster_list.Add(new Monster(waveTable.Wave_MonsterIndex, waveTable.Wave_MonsterNum, waveTable.Wave_SpawnTime));
                }
            }
        }
        else 
        {
            //spawn time, last spawn time 비교 후 소환
            foreach(Monster monster in monster_list)
            {
                if((Time.time - monster.last_spawnTime) > monster.monster_spawnTime)
                {
                    StartCoroutine(SpawnMonster(monster.monster_index, monster.monster_num));
                    monster.last_spawnTime = Time.time;
                }
            }
        }
    }

    IEnumerator SpawnMonster(int idx, int num)
    {
        for(int i=0;i<=num;i++)
        {
            GameObject clone = GameManager.Instance.pool.Get(idx);
            Color c = clone.GetComponent<SpriteRenderer>().color;
            //clone.GetComponent<SpriteRenderer>().color = new Color(c.r, c.g,  c.b, 1);
            clone.transform.position = new Vector3(transform.position.x, transform.position.y, Random.Range(-1, 1));
            clone.tag = "Enemy";
            yield return new WaitForSeconds(0.5f);
        }
        yield return null;
    }

    private void SetWaveTables(List<Dictionary<string, object>> wave_data,  WaveTable[] waveTables)
    {
        foreach (Dictionary<string, object> wave in wave_data)
        {
            int idx = (int)wave["Wave_Index"];
            waveTables[idx].Wave_Index = (int)wave["Wave_Index"];
            waveTables[idx].Wave_Name = wave["Wave_Name"].ToString();
            waveTables[idx].Wave_DevName = wave["Wave_DevName"].ToString();
            waveTables[idx].Wave_Group = (int)wave["Wave_Group"];
            waveTables[idx].Wave_StageNum = (int)wave["Wave_StageNum"];
            waveTables[idx].Wave_WaveNum = (int)wave["Wave_WaveNum"];

            if (wave["Wave_WaveType"].ToString() == "Normal")
                waveTables[idx].Wave_WaveType = WaveType.Normal;
            else
                waveTables[idx].Wave_WaveType = WaveType.Boss;

            waveTables[idx].Wave_SpawnTime = float.Parse(wave["Wave_SpawnTime"].ToString());
            waveTables[idx].Wave_MonsterIndex = (int)wave["Wave_MonsterIndex"];
            waveTables[idx].Wave_MonsterNum = (int)wave["Wave_MonsterNum"];
        }
    }

    private void PrintWaveTable(WaveTable[] waveTables, int idx)
    {
        print(waveTables[idx].Wave_MonsterNum);
    }
}
