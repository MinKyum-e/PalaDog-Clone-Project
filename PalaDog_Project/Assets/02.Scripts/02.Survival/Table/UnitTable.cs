
public class UnitTable
{
    int index;
    string name;
    int HP;
    int curHP;
    float moveSpeed;
}

public class HeroTable : UnitTable
{
    int auraLV;
    int[] skill;
}

public class MonsterTable : UnitTable
{
    int group;
    int atk;
    float atkRange;
    float atkSpeed;
}

public class MinionTable : MonsterTable
{
    int skill;
    int cost;
}

public class EnemyTable : MonsterTable
{
    int grade;

}
