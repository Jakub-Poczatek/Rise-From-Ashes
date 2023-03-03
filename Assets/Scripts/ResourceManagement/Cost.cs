using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Cost
{
    public int gold = 0;
    public int food = 0;
    public int wood = 0;
    public int stone = 0;
    public int metal = 0;

    public Cost(int gold, int food, int wood, int stone, int metal)
    {
        this.gold = gold;
        this.food = food;
        this.wood = wood;
        this.stone = stone;
        this.metal = metal;
    }
}
