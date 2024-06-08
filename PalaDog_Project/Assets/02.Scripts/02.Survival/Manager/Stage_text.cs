using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Stage_text : MonoBehaviour
{
    public TMP_Text tmp;
    void Awake()
    {
        tmp = GetComponent<TMP_Text>();
    }

    public void Update()
    {
        tmp.text = GameManager.Instance.stage.ToString() + "-" + GameManager.Instance.wave.ToString();
    }
}
