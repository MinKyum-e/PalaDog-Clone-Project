
public class UnitInfo
{
    public int index;
    public string name;
    public int HP;
    public float moveSpeed;
}

public class PlayerInfo : UnitInfo
{
    public int auraLV;
    public int[] skill;
}

public class MonsterInfo : UnitInfo
{
    public int group;
    public int atk;
    public float atkRange;
    public float atkSpeed;
}

public class MinionInfo : MonsterInfo
{
    public int skill;
    public int cost;
}

public class EnemyInfo : MonsterInfo
{
    public int gold;
    public int grade;
    public int[] skill;
}
