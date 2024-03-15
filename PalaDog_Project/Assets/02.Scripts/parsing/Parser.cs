using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parser : MonoBehaviour
{
    public List<Dictionary<string, object>> data_UnitTable = null;
    public List<Dictionary<string, object>> data_EnemyTable = null;
    public List<Dictionary<string, object>> data_WaveTable = null;

    public void Awake()
    {

        data_UnitTable = CSVReader.Read("Unit_Table");
        data_EnemyTable = CSVReader.Read("Enemy_Table");
       data_WaveTable = CSVReader.Read("Wave_Table");
    }
}
