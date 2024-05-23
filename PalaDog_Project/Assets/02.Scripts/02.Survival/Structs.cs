using JetBrains.Annotations;
using System;
using UnityEngine;

[Serializable]
public struct SkillInfo
{
    public string name;
    public int range;
    public int damange;
    public int casting_time;
    public int cool_time;
    public int cast_range;
    public bool target_check;
}

[Serializable]
public struct SkillEffectEntry
{
    public int index;
    public SkillEffectType type;
}


[Serializable]
public struct SkillEffect
{
    public int index;
    public int value;
    public float duration;
}


[Serializable]
public struct SkillEntry
{
    public int index;
    public int group;
    public SkillAct act;
    public int coolTime;
    public TargetSearchType target_search_type;
    public UnitType target_type;
    public int target_search_num;
    public BaseStat base_stat;
    public float DMGCoeff;
    public bool need_searching;
    public float searching_range;
    public SkillEffect[] skill_effects;

}

[Serializable]
public struct ActorSkillInfo
{
    public SkillEntry entry;
    public bool can_use_skill;
    public GameObject target;
}





[Serializable]
public struct CommonStatus
{
    public int index;
    public string name;
    public int HP;
    public float moveSpeed;
    public int atk;
    public float atkRange;
    public float atkSpeed;
    public int[] skill;
    public Chr_job job;
    public Vector2 moveDir;
}


public struct PlayerStatus
{

    public CommonStatus common;
    public int auraLV;
}


public struct MinionStatus
{
    public CommonStatus common;
    public int cost;
}

public struct EnemyStatus
{
    public CommonStatus common;
    public int grade;
    public int gold;
}
