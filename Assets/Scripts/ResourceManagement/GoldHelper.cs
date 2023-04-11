using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GoldHelper : BasicResourceHelper
{

    public GoldHelper(float initialAmount) : base(initialAmount) { }

    public void Maintain(IEnumerable<GameObject> structures)
    {
        foreach (GameObject structure in structures)
        {
            if(structure.CompareTag("Structure"))
            {
                resource -= Mathf.FloorToInt(structure.GetComponent<Structure>().MaintenanceCost);
            }
        }
    }

    /*public void Maintain(IEnumerable<StructureBase> structures)
    {
        foreach (StructureBase structure in structures)
        {
            resource -= structure.maintenanceGoldCost;
        }
    }*/
}



/*public class GoldHelper
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
                throw new ResourceException("Not enough gold");
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
        foreach (StructureBase structure in structures)
        {
            if (structure.GetType() == typeof(ResourceGenStruct))
            {
                Gold += Mathf.FloorToInt(((ResourceGenStruct) structure).resourceGenAmount);
            }
        }
    }
}*/
