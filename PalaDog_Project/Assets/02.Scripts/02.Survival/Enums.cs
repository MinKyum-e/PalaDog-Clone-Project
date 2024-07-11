
public enum UnitType //안쓰는중
{
    Player,
    Minion,
    Enemy,
    EnemyBase,
    Boss,
}


public enum SkillName
{
    None = 0,
    SelfHealing = 10001,
    ShieldDeployment = 10002,
    WingFlap = 20011,
    LifeDrain = 20012,
    HealingAura = 30011,
    SpeedAura = 30012,
    HammerAttack = 10011,
    LegendHammerAttack = 10012
}

public enum SkillAct
{
    P ,
    A

}
public enum TargetSearchType
{
    Target,
    Range,
    Ora,

}

public enum BaseStat
{
    None,
    Atk,
    HP
}



public enum SkillEffectType
{
    DeBuff,
    Buff,
}

public enum BuffName
{
    ATKBoost = 0,
    ATKSpeedBoost,
    Heal,
    FullImmune,
    Poison,
    Stun,
    KnockBack,
    MoveSpeed,
}
public enum Chr_job
{
    magic,
    melee,
    projectile,
}


