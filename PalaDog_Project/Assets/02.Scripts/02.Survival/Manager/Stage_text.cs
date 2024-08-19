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
        if(GameManager.Instance.overdrive_timer <= 0)
        {
            tmp.color = Color.red;
        }
        tmp.text = GameManager.Instance.chapter.ToString() + "-" + ((GameManager.Instance.stage -1)%GameManager.Instance.STAGE_PER_CHAPTER + 1).ToString();
    }
}
