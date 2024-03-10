using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitID
{
    Unit1,
    Unit2,
    Unit3,

    Enemy1 = 10,
    Enemy2,
    Enemy3,
}

public struct Unitstatus
{

    public string Name;
    public int HP;
    public int Damage;
    public float Range;
    public float MoveSpeed;
}

