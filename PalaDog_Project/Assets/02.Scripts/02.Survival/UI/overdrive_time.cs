using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class overdrive : MonoBehaviour
{
    public Image cool;
    void Update()
    {

        if (GameManager.Instance != null)
        {
            cool.fillAmount = GameManager.Instance.overdrive_timer / GameManager.Instance.overdrive_time;
        }

    }
}
