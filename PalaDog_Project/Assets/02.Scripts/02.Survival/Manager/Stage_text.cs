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

    public void stageWaveUpdate(int stage, int wave)
    {
        tmp.text = "stage : " + stage + "-" + wave;
    }
}
