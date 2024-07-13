using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static DG.Tweening.DOTweenAnimation;

public class Parser : MonoBehaviour
{
    private static Parser instance = null;
    public static List<Dictionary<string, object>> data_MinionTable = null;
    public static List<Dictionary<string, object>> data_EnemyTable = null;
    public static List<Dictionary<string, object>> data_WaveTable = null;
    public static List<Dictionary<string, object>> data_ShopTable = null;
    public static List<Dictionary<string, object>> data_SkillTable = null;
    public static List<Dictionary<string, object>> data_SkillEffectTable = null;

    public static Dictionary<int, EnemyStatus> enemy_status_dict = null;
    public static Dictionary<int, WaveInfo> wave_info_dict = null;
    public static Dictionary<int, MinionStatus> minion_status_dict = null;
    public static Dictionary<int, ShopItemInfo> shop_item_info_dict = null;
    public static Dictionary<int, SkillEntry> skill_table_dict = null;
    public static Dictionary<int, SkillEffectEntry> skill_effect_table_dict = null;

    public void Awake()
    {
        if(instance == null)
        {
            instance = this;
            data_MinionTable = CSVReader.Read("DT_ChrTable", 20);
            data_EnemyTable = CSVReader.Read("DT_MonsterTable", 19);
            data_WaveTable = CSVReader.Read("DT_WaveTable", 10);
            data_ShopTable = CSVReader.Read("DT_ShopTable", 29);
            data_SkillTable = CSVReader.Read("DT_SkillTable", 43);

            data_SkillEffectTable = CSVReader.Read("DT_SkillEffectTable", 3);


            enemy_status_dict = new Dictionary<int, EnemyStatus>();
            wave_info_dict = new Dictionary<int, WaveInfo>();
            minion_status_dict = new Dictionary<int, MinionStatus>();
            shop_item_info_dict= new Dictionary<int, ShopItemInfo>();
            skill_table_dict = new Dictionary<int, SkillEntry>();
            skill_effect_table_dict = new Dictionary<int, SkillEffectEntry>();

            //몬스터 유닛 정보들어있는 class 만들기
            foreach (var d in data_EnemyTable)
            {
                int idx = (int)d["Monster_Index"];
                EnemyStatus e = new EnemyStatus();
                e.common.skill = new int[3];
                e.common.name = d["Monster_GameName"].ToString();
                //enemy_info_dict[idx].grade = (int)d["Monster_Grade"];
                e.gold = (int)d["Monster_Gold"];
                e.common.HP = (int)d["Monster_HP"];
                e.common.atk = float.Parse(d["Monster_Atk"].ToString());
                e.common.atkSpeed = float.Parse(d["Monster_AtkSpeed"].ToString());
                e.common.atkRange = float.Parse(d["Monster_AtkRange"].ToString());
                e.common.moveSpeed = float.Parse(d["Monster_MoveSpeed"].ToString());
                e.common.skill[0] = (int)d["Monster_Skill1"];
                e.common.skill[1] = (int)d["Monster_Skill2"];
                e.common.skill[2] = (int)d["Monster_Skill3"];
                e.common.moveDir = Vector2.left;
                string grade = d["Monster_Grade"].ToString();
                string[] target_grade = Enum.GetNames(typeof(UnitGrade));
                for (int i = 0; i < target_grade.Length; i++)
                {
                    if (target_grade[i].Equals(grade))
                    {
                       e.common.grade = (UnitGrade)i;
                        break;
                    }
                }
                enemy_status_dict[idx] = e;
            }

            foreach (var d in data_MinionTable)
            {
                
                int idx = (int)d["Chr_Index"];
                MinionStatus s = new MinionStatus();
                s.common.skill = new int[3];
                s.common.skill[0] = (int)d["Chr_Skill"];
                s.common.skill[1] = 0;
                s.common.skill[2] = 0;
                s.common.name = d["Chr_GameName"].ToString();
                //enemy_info_dict[idx].grade = (int)d["Monster_Grade"];
                s.cost = (int)d["Chr_Cost"];
                s.common.HP = float.Parse(d["Chr_HP"].ToString());
                s.common.atk = float.Parse(d["Chr_Atk"].ToString());
                s.common.atkSpeed = float.Parse(d["Chr_AtkSpeed"].ToString());
                s.common.atkRange = float.Parse(d["Chr_AtkRange"].ToString());
                //job확인
                if (d["Chr_Job"].ToString() == "melee")
                    s.common.job = Chr_job.melee;
                else if (d["Chr_Job"].ToString() == "projectile")
                    s.common.job = Chr_job.projectile;
                else if (d["Chr_Job"].ToString() == "magic")
                    s.common.job = Chr_job.magic;

                s.common.moveSpeed = float.Parse(d["Chr_MoveSpeed"].ToString());
                s.common.moveDir = Vector2.right;
                s.cool_time = float.Parse(d["Chr_CoolTime"].ToString());
                

                string grade = d["Chr_Grade"].ToString();
                string[] target_grade = Enum.GetNames(typeof(UnitGrade));
                for (int i = 0; i < target_grade.Length; i++)
                {
                    if (target_grade[i].Equals(grade))
                    {
                        s.common.grade = (UnitGrade)i;
                        break;
                    }
                }

                minion_status_dict[idx] = s;
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


            //찐스킬정보
            foreach (var d in data_SkillTable)
            {
                int idx = (int)d["Skill_Index"];
                SkillEntry e = new SkillEntry();
                e.index = idx;
                e.group = (int)d["Skill_Group"];

                string act = d["Skill_Act"].ToString();
                string[] act_strings = Enum.GetNames(typeof(SkillAct));
                for(int i=0;i<act_strings.Length;i++)
                {
                    if (act_strings[i].Equals(act))
                    {
                        e.act = (SkillAct)i;
                        break;
                    }
                }
                e.coolTime = float.Parse(d["Skill_CoolTime"].ToString());
                string target_search_type = d["Skill_Type"].ToString();
                string[] target_search_type_strings = Enum.GetNames(typeof(TargetSearchType));
                for (int i = 0; i < target_search_type_strings.Length; i++)
                {
                    if (target_search_type_strings[i].Equals(target_search_type))
                    {
                        e.target_search_type = (TargetSearchType)i;
                        break;
                    }
                }

                string target_type = d["Skill_TargetType"].ToString();
                string[] target_type_strings = Enum.GetNames(typeof(UnitType));
                for (int i = 0; i < target_type_strings.Length; i++)
                {
                    if (target_type_strings[i].Equals(target_type))
                    {
                        e.target_type = (UnitType)i;
                        break;
                    }
                }
                e.target_search_num = (int)d["Skill_TypeValue"];
                string base_stat = d["Skill_BaseStat"].ToString();
                string[] base_stat_strings = Enum.GetNames(typeof(BaseStat));
                for (int i = 0; i < base_stat_strings.Length; i++)
                {
                    if (base_stat_strings[i].Equals(base_stat))
                    {
                        e.base_stat = (BaseStat)i;
                        break;
                    }
                }

                e.DMGCoeff = float.Parse(d["Skill_DMGCoeff"].ToString());
                e.need_searching = d["Skill_TargetSearching"].Equals("TRUE");

                e.skill_effects = new SkillEffect[2];

                for(int i=1;i<=2;i++)
                {

                    e.skill_effects[i-1].index = (int)d["Skill_Effect" + i+"Index"];
                    e.skill_effects[i-1].value = float.Parse(d["Skill_Effect" + i + "Value"].ToString());
                    e.skill_effects[i-1].duration = float.Parse(d["Skill_Effect" + i + "Duration"].ToString());
                }

                e.searching_range = float.Parse(d["Skill_TargetSearchingRange"].ToString());
                skill_table_dict[idx] = e;
            }

            //스킬 이펙트 정보
            foreach (var d in data_SkillEffectTable)
            {
                int idx = (int)d["SkillEffect_Index"];
                SkillEffectEntry e = new SkillEffectEntry();
                e.index = idx;


                string type = d["SkillEffect_Type"].ToString();
                string[] type_strings = Enum.GetNames(typeof(SkillEffectType));
                for (int i = 0; i < type_strings.Length; i++)
                {
                    if (type_strings[i].Equals(type))
                    {
                        e.type = (SkillEffectType)i;
                        break;
                    }
                }
                skill_effect_table_dict[idx] = e;
            }

            //상점 아이템 정보
            foreach (var d in data_ShopTable)
            {
                int idx = (int)d["Shop_Index"];

                ShopItemInfo e = new ShopItemInfo();
                e.idx = idx;
                e.name = d["Shop_Name"].ToString();
                e.group = (int)d["Shop_Group"];

                switch (d["Shop_ListType"].ToString())
                {
                    case "Enforce":
                        e.list_type = ShopEnums.ListType.Enforce;
                        break;
                    case "Unlock":
                        e.list_type = ShopEnums.ListType.Unlock;
                        break;
                    case "Spawn":
                        e.list_type = ShopEnums.ListType.Spawn;
                        break;
                }
                e.prelist = (int)d["Shop_PreList"];
                switch (d["Shop_GoodsType"].ToString())
                {
                    case "Gold":
                        e.goods_type = ShopEnums.GoodsType.Gold;
                        break;
                    case "Food":
                        e.goods_type = ShopEnums.GoodsType.Food;
                        break;
                }

                e.goods_value = (int)d["Shop_GoodsValue"];
                e.etc_value = (int)d["Shop_etcValue"];
                shop_item_info_dict[idx] = e;   
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
