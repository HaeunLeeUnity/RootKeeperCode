using System.Collections.Generic;
public class LevelData
{
    public List<DifficultyStamp> DifficultyStamp = new List<DifficultyStamp>();
    public List<Patten> Pattens = new List<Patten>();

    public MonsterBalance[] MonsterBalances = new MonsterBalance[8];
    public int[] ItemPrices = { 80, 200, 800, 1000, 1200, 9999, 250, 250, 10, 10, 1500 };
}

public class DifficultyStamp
{
    public float TimeStamp = 0;
    public short Difficulty = 0;
}

public class Patten
{
    public PatternType Type = 0;
    public float Delay = 0;
    public int[] Monsters = { 0, 0, 0, 0, 0, 0, 0, 0 };
}

// struct 를 이용하면 좋지만 JsonConverter 에서 직렬화를 지원하지 않기 때문에 클래스 형식으로 만들어 깊은 복사를 함.
public class MonsterBalance
{
    public int MaxHp;
    public float Speed;
    public int Money;
}
