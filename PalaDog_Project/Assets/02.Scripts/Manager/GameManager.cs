using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Player player;
    public PoolManager pool;
    public Parser parser;
    private void Awake()
    {
        Instance = this;
        
    }
}
