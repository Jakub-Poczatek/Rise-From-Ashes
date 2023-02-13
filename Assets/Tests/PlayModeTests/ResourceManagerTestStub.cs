using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManagerTestStub : MonoBehaviour, IResourceManager
{
    public int InitialGold  { get; }
    public float ResourceCalculationInterval { get; }

    public void CalculateIncome()
    {
    }

    public bool CanIBuyIt(int amount)
    {
        return true;
    }

    public void InceaseGold(int amount)
    {
    }

    public void PrepareResourceManager(BuildingManager buildingManager)
    {
    }

    public bool SpendGold(int amount)
    {
        return true;
    }
}
