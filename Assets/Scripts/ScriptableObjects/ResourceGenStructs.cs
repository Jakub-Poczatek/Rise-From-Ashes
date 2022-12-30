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
    public Cost newCost;
    public int newResourceGenAmount;
}

public enum ResourceType
{
    Gold,
    Wood,
    Stone
}
