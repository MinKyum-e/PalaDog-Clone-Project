using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    private static EffectManager m_Instance;
    public ParticleSystem spawnCloud; 
    public static EffectManager Instance
    {
        get
        {
            if (m_Instance == null) return null;
            else
            return m_Instance;
        }
    }

    private void Awake()
    {
        if (null == m_Instance) //게임 완전처음시작할때만
        {
            m_Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {//씬 이동됐는데 게임 매니저가 존재할 경우 자신을 삭제
            Destroy(this.gameObject);
        }
    }
}