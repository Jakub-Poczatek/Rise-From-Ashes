using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StructureBase : ScriptableObject
{
    public string structureName;
    public GameObject prefab;
    public Cost buildCost;
    public int maintenanceGoldCost;
}

[System.Serializable]
public struct Cost
{
    public Cost(int gold, int wood, int stone)
    {
        this.gold = gold;
        this.wood = wood;
        this.stone = stone;
    }
    public int gold;
    public int wood;
    public int stone;
}
