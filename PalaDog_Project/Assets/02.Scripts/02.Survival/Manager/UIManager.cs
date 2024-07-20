using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance { get { return _instance; } }

    public GameObject GamePlay;
    public GameObject GameOver;
    public GameObject GameChapterClear;
    public GameObject GameClear;
    public GameObject GameResourse;


    private UIPageInfo CurrentPageInfo;
    public int CurrentPage = 0;

    void Start()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetCurrentPage(UIPageInfo pageInfo)
    {
        CurrentPageInfo = pageInfo;
        CurrentPage = (int)CurrentPageInfo;

        GamePlay.SetActive(false);
        GameOver.SetActive(false);
        GameChapterClear.SetActive(false);
        GameClear.SetActive(false);
        GameResourse.SetActive(false);

        if (CurrentPageInfo == UIPageInfo.GamePlay)
        {
            GamePlay.SetActive(true);
            GameResourse.SetActive(true);
        }
        else if (CurrentPageInfo == UIPageInfo.GameOver)
        {
            GameOver.SetActive(true);
        }
        else if (CurrentPageInfo == UIPageInfo.GameChapterClear)
        {
            GameChapterClear.SetActive(true);
        }
        else if(CurrentPageInfo == UIPageInfo.GameClear)
        {
            GameClear.SetActive(true);
        }
        else if(CurrentPageInfo == UIPageInfo.Spawn)
        {
            GameResourse.SetActive(true);
        }

    }
}