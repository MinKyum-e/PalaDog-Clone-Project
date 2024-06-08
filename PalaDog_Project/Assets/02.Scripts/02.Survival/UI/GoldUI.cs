using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GoldUI : MonoBehaviour
{
    public static GoldUI instance;
    public TMP_Text tmp;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        tmp = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        SetGoldUI();
    }

    public void SetGoldUI()
    {

        tmp.text = GameManager.Instance.cur_gold.ToString();
    }

}
