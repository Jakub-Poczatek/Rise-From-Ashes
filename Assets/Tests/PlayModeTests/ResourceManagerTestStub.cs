using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManagerTestStub : MonoBehaviour, IResourceManager
{
    public int InitialGold  { get; }
    public float ResourceCalculationInterval { get; }

    public void CalculateResources()
    {
    }

    public bool CanIBuyIt(int amount)
    {
        return true;
    }

    public void EarnResources(int amount)
    {
    }

    public void PrepareResourceManager(BuildingManager buildingManager)
    {
    }

    public bool Purchase(int amount)
    {
        return true;
    }
}
