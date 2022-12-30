using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StructureBase : ScriptableObject
{
    public string structureName;
    public GameObject prefab;
    public Cost buildCost;
}

[System.Serializable]
public struct Cost
{
    public int gold;
    public int wood;
    public int stone;
}