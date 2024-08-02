
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
    ATKAura = 30021,
    HammerAttack = 10011,
    LegendHammerAttack = 10012,
    AttackSpeedUP = 10021,
    HeroArrow  =10022,
    SpawnMushroom = 20021,
    PoisonFog = 20022,

    Magic = 10031,
    EpicMagic = 10032,
}

public enum SkillAct
{
    P ,
    A

}
public enum TargetSearchType
{
    Target,
    TargetFar,
    Range,
    Ora,

}

public enum BaseStat
{
    None,
    Atk,
    Magic,
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
    Spawn,
}
public enum Chr_job
{
    magic,
    melee,
    projectile,
}

public enum MinionUnitIndex
{
    Player = 10000,
    Warrior = 11110,
    Knight_Elite = 11210,
    Hammer_Elite = 11220,

    Knight_Hero = 11310,
    Hammer_Hero = 11320,

    Archer_Elite = 12210,
    Archer_Hero = 12310,

    Wizard_Elite = 12220,
    Wizard_Hero = 12320,

}


public enum UnitGrade
{
    Player,
    Normal,
    Elite,
    Hero,
    Boss,
}