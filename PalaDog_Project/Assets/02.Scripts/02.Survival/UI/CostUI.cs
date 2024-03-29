using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CostUI : MonoBehaviour
{
    static public TMP_Text tmp;
    // Start is called before the first frame update
    void Awake()
    {
        tmp = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        SetCostUI();
    }

    static void SetCostUI()
    {

        tmp.text = "Cost : " + GameManager.Instance.cur_cost + " / " + GameManager.Instance.MAX_COST;
    }

}
