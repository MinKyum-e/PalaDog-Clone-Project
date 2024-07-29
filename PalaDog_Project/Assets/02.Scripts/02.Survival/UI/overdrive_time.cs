using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class overdrive : MonoBehaviour
{
    public TMP_Text tmp;
    private void Awake()
    {
        tmp = GetComponent<TMP_Text>();
    }
    void Update()
    {
        if((int)GameManager.Instance.overdrive_timer<=0)
        {
            tmp.text = "overdrive!!!!!!!";
        }
        else
        tmp.text = "overdrive timer : " + ((int)GameManager.Instance.overdrive_timer).ToString();
    }
}
