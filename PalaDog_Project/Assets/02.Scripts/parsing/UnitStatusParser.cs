using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parser : MonoBehaviour
{
    public List<Dictionary<string, object>> data_Unitstatus = null;
    private void Awake()
    {

        data_Unitstatus = CSVReader.Read("unit_status");

    }
}
