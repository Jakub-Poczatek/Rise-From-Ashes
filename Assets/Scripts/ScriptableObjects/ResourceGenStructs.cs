using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Resource Generating Structure", menuName = "StructureManagement/Data/ResourceGenStruct")]
public class ResourceGenStructs : StructureBase
{
    public float resourceGenAmount;
    public ResourceType resourceType;
    public bool upgradable;
    public UpgradeType[] availableUpgrades;
}

[System.Serializable]
public struct UpgradeType
{
    public GameObject prefab;
    public Cost cost;
    public int resourceGenAmount;
}

public enum ResourceType
{
    Gold,
    Wood,
    Stone
}
