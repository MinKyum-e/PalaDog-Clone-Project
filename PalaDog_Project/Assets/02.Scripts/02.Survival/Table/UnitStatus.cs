using UnityEngine;

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
