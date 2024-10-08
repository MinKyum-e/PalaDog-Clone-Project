using JetBrains.Annotations;
using System;
using System.Collections.Generic;
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
    public float value;
    public float duration;
}


[Serializable]
public struct SkillEntry
{
    public int index;
    public int group;
    public SkillAct act;
    public float coolTime;
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
    public Actor target;
}

[Serializable]
public struct BuffStruct
{
    public bool full_immune;
    public bool _melee_attack_ignore;
    public bool _magic_attack_ignore;
    public bool _projectile_attack_ignore;
    public bool stun;
}


[Serializable]
public struct CommonStatus
{
    public int index;
    public string name;
    public float HP;
    public float moveSpeed;
    public float atk;
    public float atkRange;
    public float atkSpeed;
    public int[] skill;
    public Chr_job job;
    public UnitGrade grade;
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
    public float cool_time;
   
}

public struct EnemyStatus
{
    public CommonStatus common;
    public int grade;
    public int gold;
}

