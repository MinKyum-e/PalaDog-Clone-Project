using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class OpeningView : MonoBehaviour
{

    public List<GameObject> views;
    public float duration = 2.0f;
    int cur_view = 0;
    
    public void nextView()
    {
        cur_view++;

        if(cur_view == views.Count)
        {

            SceneManager.LoadScene("Title");
        }
        else
        {
            DOTween.To(() => Camera.main.transform.position, x => Camera.main.transform.position = x, new Vector3(views[cur_view].transform.position.x, views[cur_view].transform.position.y, -10), 0.5f)
    .SetEase(Ease.OutSine);
        }

    }
}
