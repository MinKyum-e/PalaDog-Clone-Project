using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parser : MonoBehaviour
{
    private static Parser instance = null;
    public static List<Dictionary<string, object>> data_MinionTable = null;
    public static List<Dictionary<string, object>> data_EnemyTable = null;
    public static List<Dictionary<string, object>> data_WaveTable = null;

    public void Awake()
    {
        if(instance == null)
        {
            instance = this;
            data_MinionTable = CSVReader.Read("Minion_Table");
            data_EnemyTable = CSVReader.Read("Enemy_Table");
            data_WaveTable = CSVReader.Read("Wave_Table");
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
