using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace ShopEnums
{
    public enum ListType
    {
        Enforce,
        Unlock,
        Spawn,
    }

    public enum GoodsType
    {
        Gold,
        Food,
    }

    public enum UnLockType
    {
        InGameUnit,
        Evolution
    }
}

public struct ShopItemInfo
{
    public int idx;
    public string name; //
    public int group;
    public ShopEnums.ListType list_type;
    public int prelist;
    public ShopEnums.GoodsType goods_type;
    public int goods_value;
    public int etc_value;
}

public class ShopManager : MonoBehaviour
{
    private static ShopManager instance = null;

    private List<int> unlocked_evoluation_unit_list;//진화 해금 unit_list
    private List<int> unlocked_ingame_unit_list;//인게임에서 골드로 사는 unit 해금 list

    public static ShopManager Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }
    private void Awake()
    {
        if (null == instance) //게임 완전처음시작할때만
        {
            instance = this;
            unlocked_ingame_unit_list = new List<int>();
            unlocked_evoluation_unit_list = new List<int>(); //게임종료시에 저장되게 구현했다면 이거 바꾸기
            DontDestroyOnLoad(gameObject);
        }
        else
        {//씬 이동됐는데 게임 매니저가 존재할 경우 자신을 삭제
            Destroy(gameObject);
        }

    }

    /// <summary>
    /// unlock한 item의 shopindex를 shopmanager unlocked list에 추가
    /// 추가 성공시 true
    /// </summary>
    /// <param name="unlock_type">unlock 한 item의 type</param>
    /// <param name="index">shop index</param>
    /// <returns></returns>
    public bool AddToUnLockList(ShopEnums.UnLockType unlock_type, int index) 
    {
        switch(unlock_type)
        {
            case ShopEnums.UnLockType.InGameUnit:
                unlocked_ingame_unit_list.Add(index);
                break;
            case ShopEnums.UnLockType.Evolution:
                unlocked_evoluation_unit_list.Add(index);
                break;
            default:
                return false;
        }
        return true;
    }
    /// <summary>
    /// unlock type에 맞는 list에서 prerequisite 만족하는지 확인. 
    /// true : prerequisite 만족
    /// </summary>
    /// <param name=""></param>
    /// <param name="prerequisite"></param>
    /// <returns></returns>
    public bool CheckPrerequisite(ShopEnums.UnLockType unlock_type, int prerequisite)
    {
        switch (unlock_type)
        {
            case ShopEnums.UnLockType.InGameUnit:
                return unlocked_ingame_unit_list.Contains(prerequisite);
            case ShopEnums.UnLockType.Evolution:
                return unlocked_evoluation_unit_list.Contains(prerequisite);
            default:
                return false;
        }
    }

    public void ClearInGameUnlockedList()
    {
        unlocked_ingame_unit_list.Clear();
    }
}
