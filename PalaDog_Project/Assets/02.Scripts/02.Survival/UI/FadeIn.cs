using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour
{
    Image image;
    Color color;
    float time   = 0f;
    float F_time = 1f;
    private void Awake()
    {
        image = GetComponent<Image>();
        color = image.color;
    }

    public void Fade()
    {
        StartCoroutine(FadeFlow());
    }
    
    IEnumerator FadeFlow()
    {
        Color alpha = color;
        while(color.a > 0f)
        {
            time+=Time.deltaTime / F_time;
            alpha.a = Mathf.Lerp(1, 0, time);
            image.color = alpha;
            yield return null;
        }
        GameManager.Instance.state = GameState.GAME_PLAY;
        yield return null;
    }
}
