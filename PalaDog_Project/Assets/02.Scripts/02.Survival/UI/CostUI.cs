using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CostUI: MonoBehaviour
{
    public static CostUI instance; 
    public TMP_Text tmp;
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
        tmp = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        SetCostUI();
    }

    public void SetCostUI()
    {

        
        tmp.text = GameManager.Instance.cur_cost.ToString() + " / " + GameManager.Instance.MAX_COST.ToString();

    }

}
