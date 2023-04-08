using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StructureBase : ScriptableObject
{
    public string structureName;
    public GameObject prefab;
    public Cost buildCost;
    public int maintenanceGoldCost;
    public int maxCitizenCapacity;
}
