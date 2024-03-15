using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance { get { return _instance; } }

    public GameObject GamePlay;
    public GameObject GameOver;


    private UIPageInfo CurrentPageInfo;
    public int CurrentPage = 0;

    void Start()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    public void SetCurrentPage(UIPageInfo pageInfo)
    {
        CurrentPageInfo = pageInfo;
        CurrentPage = (int)CurrentPageInfo;

        GamePlay.SetActive(false);
        GameOver.SetActive(false);

        if (CurrentPageInfo == UIPageInfo.GamePlay)
        {
            GamePlay.SetActive(true);
        }
        else if (CurrentPageInfo == UIPageInfo.GameOver)
        {
            GameOver.SetActive(true);
        }
    }
}