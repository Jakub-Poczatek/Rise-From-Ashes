using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Cost
{
    public float gold = 0;
    public float food = 0;
    public float wood = 0;
    public float stone = 0;
    public float metal = 0;

    public Cost(float gold, float food, float wood, float stone, float metal)
    {
        this.gold = gold;
        this.food = food;
        this.wood = wood;
        this.stone = stone;
        this.metal = metal;
    }

    public static Cost operator *(Cost cost, float multiplier)
    {
        return new Cost
        (
            cost.gold * multiplier,
            cost.food * multiplier,
            cost.wood * multiplier,
            cost.stone * multiplier,
            cost.metal * multiplier
        );
    }

    public override string ToString()
    {
        return 
            "Gold: " + gold + "\n" +
            "Food: " + food + "\n" +
            "Wood: " + wood + "\n" +
            "Stone: " + stone + "\n" +
            "Metal: " + metal;
    }
}
