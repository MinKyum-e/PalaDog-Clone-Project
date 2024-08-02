using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG;
using DG.Tweening;
using UnityEngine.UI;
public class round_text : MonoBehaviour
{
    public Text txt;
    public float action_time,  end_time;
    public GameObject wait_img;

    public void change_end_string()
    {
        string s = "Stage " + GameManager.Instance.stage.ToString();
        txt.DOText(s, action_time).OnComplete(() =>
        {
            Color c = txt.color;
            DOTween.To(() => txt.color, x => txt.color = x, new Color(c.r, c.g, c.b, 0), end_time).OnComplete(() =>
            {
                txt.color = c;
                txt.text = "";
                gameObject.SetActive(false);
                
            });
        });

    }
}
