using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;
    public Player player;
    public PoolManager pool;
    public Parser parser;

    private void Awake()
    {
        if(null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);//씬 전환되도 삭제 안됨
        }
        else
        {//씬 이동됐는데 게임 매니저가 존재할 경우 자신을 삭제
            Destroy(this.gameObject );
        }
    }

    public static GameManager Instance //게임매니저 인스턴스 접근
    {
        get
        {
            if(null == instance)
            {
                return null;
            }
            return instance;
        }
    }

    public void InitGame()
    {

    }
    public void PauseGame()
    {

    }
    public void ContinueGame()
    {

    }
    public void RestartGame()
    {

    }
    public void StopGame()
    {
      
    }
}
