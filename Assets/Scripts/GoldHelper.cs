using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldHelper
{
    private int gold;

    public GoldHelper(int initialGold)
    {
        this.gold = initialGold;
    }

    public int Gold { 
        get => gold; 
        private set 
        {
            if (value < 0)
            {
                gold = 0;
                throw new GoldException("Not enough gold");
            }
            else
                gold = value; 
        }
    }

    public void DecreaseGold(int amount)
    {
        Gold -= amount;
    }

    public void IncreaseGold(int amount)
    {
        Gold += amount;
    }

    public void CalculateGold(IEnumerable<StructureBase> structures)
    {
        CollectGold(structures);
        ReduceMaintenance(structures);
    }

    private void ReduceMaintenance(IEnumerable<StructureBase> structures)
    {
        foreach (StructureBase structure in structures)
        {
            Gold -= structure.maintenanceGoldCost;
        }
    }

    private void CollectGold(IEnumerable<StructureBase> structures)
    {
        foreach (ResourceGenStruct structure in structures)
        {
            Gold += Mathf.FloorToInt(structure.resourceGenAmount);
        }
    }
}
