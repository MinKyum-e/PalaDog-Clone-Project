using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FoodUI : MonoBehaviour
{
    static public TMP_Text tmp;
    // Start is called before the first frame update
    void Awake()
    {
        tmp = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        SetFoodUI();
    }

    static void SetFoodUI()
    {
        
        tmp.text = "Food : " + (int)GameManager.Instance.cur_food   + " / " + GameManager.Instance.MAX_FOOD ;
    }

}
