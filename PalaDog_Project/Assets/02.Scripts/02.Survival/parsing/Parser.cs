using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parser : MonoBehaviour
{
    public List<Dictionary<string, object>> data_MinionTable = null;
    public List<Dictionary<string, object>> data_EnemyTable = null;
    public List<Dictionary<string, object>> data_WaveTable = null;

    public void Awake()
    {

        data_MinionTable = CSVReader.Read("Minion_Table");
        data_EnemyTable = CSVReader.Read("Enemy_Table");
       data_WaveTable = CSVReader.Read("Wave_Table");
    }
}
