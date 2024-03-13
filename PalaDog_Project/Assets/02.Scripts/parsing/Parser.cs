using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parser : MonoBehaviour
{
    public List<Dictionary<string, object>> data_Unitstatus = null;
    public List<Dictionary<string, object>> data_WaveTable = null;
    private void Awake()
    {

       data_Unitstatus = CSVReader.Read("unit_status");
       data_WaveTable = CSVReader.Read("Wave_Table");
    }
}
