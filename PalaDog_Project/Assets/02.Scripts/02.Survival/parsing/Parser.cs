using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parser : MonoBehaviour
{
    private static Parser instance = null;
    public static List<Dictionary<string, object>> data_MinionTable = null;
    public static List<Dictionary<string, object>> data_EnemyTable = null;
    public static List<Dictionary<string, object>> data_WaveTable = null;

    public static Dictionary<int, EnemyInfo> enemy_info_dict = null;
    public static Dictionary<int, WaveInfo> wave_info_dict = null;
    public static Dictionary<int, MinionInfo> minion_info_dict = null;

    public void Awake()
    {
        if(instance == null)
        {
            instance = this;
            data_MinionTable = CSVReader.Read("DT_ChrTable", 20);
            data_EnemyTable = CSVReader.Read("DT_MonsterTable", 19);
            data_WaveTable = CSVReader.Read("DT_WaveTable", 10);
            
            enemy_info_dict = new Dictionary<int, EnemyInfo>();
            wave_info_dict = new Dictionary<int, WaveInfo>();
            minion_info_dict = new Dictionary<int, MinionInfo>();

            //몬스터 유닛 정보들어있는 class 만들기
            foreach (var d in data_EnemyTable)
            {
                int idx = (int)d["Monster_Index"];
                enemy_info_dict[idx] = new EnemyInfo();
                enemy_info_dict[idx].skill = new int[3];
                enemy_info_dict[idx].name = d["Monster_GameName"].ToString();
                //enemy_info_dict[idx].grade = (int)d["Monster_Grade"];
                enemy_info_dict[idx].gold = (int)d["Monster_Gold"];
                enemy_info_dict[idx].HP = (int)d["Monster_HP"];
                enemy_info_dict[idx].atk = (int)d["Monster_Atk"];
                enemy_info_dict[idx].atkSpeed = float.Parse(d["Monster_AtkSpeed"].ToString());
                enemy_info_dict[idx].atkRange = float.Parse(d["Monster_AtkRange"].ToString());
                enemy_info_dict[idx].moveSpeed = float.Parse(d["Monster_MoveSpeed"].ToString());
                enemy_info_dict[idx].skill[0] = (int)d["Monster_Skill1"];
                enemy_info_dict[idx].skill[1] = (int)d["Monster_Skill2"];
                enemy_info_dict[idx].skill[2] = (int)d["Monster_Skill3"];
                
            }

            foreach (var d in data_MinionTable)
            {
                
                int idx = (int)d["Chr_Index"];
                
                minion_info_dict[idx] = new MinionInfo();
                minion_info_dict[idx].skill = (int)d["Chr_Skill"];
                minion_info_dict[idx].name = d["Chr_GameName"].ToString();
                //enemy_info_dict[idx].grade = (int)d["Monster_Grade"];
                minion_info_dict[idx].cost = (int)d["Chr_Cost"];
                minion_info_dict[idx].HP = (int)d["Chr_HP"];
                minion_info_dict[idx].atk = (int)d["Chr_Atk"];
                minion_info_dict[idx].atkSpeed = float.Parse(d["Chr_AtkSpeed"].ToString());
                minion_info_dict[idx].atkRange = float.Parse(d["Chr_AtkRange"].ToString());
                minion_info_dict[idx].moveSpeed = float.Parse(d["Chr_MoveSpeed"].ToString());

            }
            //웨이브 정보
            foreach (var wave in data_WaveTable)
            {
                int idx = (int)wave["Wave_Index"];
                wave_info_dict[idx] = new WaveInfo();
                wave_info_dict[idx].Wave_Index = (int)wave["Wave_Index"];
                wave_info_dict[idx].Wave_Name = wave["Wave_Name"].ToString();
                wave_info_dict[idx].Wave_DevName = wave["Wave_DevName"].ToString();
                wave_info_dict[idx].Wave_Group = (int)wave["Wave_Group"];
                wave_info_dict[idx].Wave_StageNum = (int)wave["Wave_StageNum"];
                wave_info_dict[idx].Wave_WaveNum = (int)wave["Wave_WaveNum"];

                if (wave["Wave_WaveType"].ToString() == "Normal")
                    wave_info_dict[idx].Wave_WaveType = WaveType.Normal;
                else
                    wave_info_dict[idx].Wave_WaveType = WaveType.Boss;

                wave_info_dict[idx].Wave_SpawnTime = float.Parse(wave["Wave_SpawnTime"].ToString());
                wave_info_dict[idx].Wave_MonsterIndex = (int)wave["Wave_MonsterIndex"];
                wave_info_dict[idx].Wave_MonsterNum = (int)wave["Wave_MonsterNum"];
            }


            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

    public static Parser Instance //게임매니저 인스턴스 접근
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }
}
