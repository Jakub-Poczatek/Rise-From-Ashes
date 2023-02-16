using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour, IResourceManager
{
    [SerializeField] private int initialGold = 1000;
    [SerializeField] private int initialFood = 50;
    [SerializeField] private int initialWood = 100;
    [SerializeField] private int initialStone = 100;
    [SerializeField] private int initialMetal = 0;
    [SerializeField] private float resourceCalculationInterval = 1;
    
    public UIController uiController;
    private BuildingManager buildingManager;
    private GoldHelper goldHelper;
    private BasicResourceHelper foodHelper;
    private BasicResourceHelper woodHelper;
    private BasicResourceHelper stoneHelper;
    private BasicResourceHelper metalHelper;

    // Start is called before the first frame update
    void Start()
    {
        goldHelper = new GoldHelper(initialGold);
        foodHelper = new BasicResourceHelper(initialFood);
        woodHelper = new BasicResourceHelper(initialWood);
        stoneHelper = new BasicResourceHelper(initialStone);
        metalHelper = new BasicResourceHelper(initialMetal);

        UpdateMoneyValueUI();
    }

    public void PrepareResourceManager(BuildingManager buildingManager)
    {
        this.buildingManager = buildingManager;
        InvokeRepeating(nameof(CalculateResources), 0, resourceCalculationInterval);
    }

    public bool Purchase(Cost cost)
    {
        if (CanIAffordIt(cost))
        {
            SpendResources(cost);
            return true;
        }
        return false;
    }

    public bool CanIAffordIt(Cost cost)
    {
        return (goldHelper.Resource >= cost.gold
        && foodHelper.Resource >= cost.food
        && woodHelper.Resource >= cost.wood
        && stoneHelper.Resource >= cost.stone
        && metalHelper.Resource >= cost.metal);
    }

    private void SpendResources(Cost cost)
    {
        goldHelper.AdjustResource(-cost.gold);
        foodHelper.AdjustResource(-cost.food);
        woodHelper.AdjustResource(-cost.wood);
        stoneHelper.AdjustResource(-cost.stone);
        metalHelper.AdjustResource(-cost.metal);
        UpdateMoneyValueUI();
    }

    public void EarnResources(Cost cost)
    {
        goldHelper.AdjustResource(cost.gold);
        foodHelper.AdjustResource(cost.food);
        woodHelper.AdjustResource(cost.wood);
        stoneHelper.AdjustResource(cost.stone);
        metalHelper.AdjustResource(cost.metal);
        UpdateMoneyValueUI();
    }

    public void CalculateResources()
    {
        IEnumerable<StructureBase> structures = buildingManager.GetAllStructures();
        CollectResourceGains(structures);
        goldHelper.Maintain(structures);
        UpdateMoneyValueUI();
    }

    private void CollectResourceGains(IEnumerable<StructureBase> structures)
    {
        foreach (StructureBase structure in structures)
        {
            if (structure.GetType() == typeof(ResourceGenStruct))
            {
                ResourceGenStruct tempStruct = (ResourceGenStruct)structure;
                switch (tempStruct.resourceType)
                {
                    case ResourceType.Gold:
                        goldHelper.CollectResource(tempStruct.resourceGenAmount);
                        break;
                    case ResourceType.Food:
                        foodHelper.CollectResource(tempStruct.resourceGenAmount);
                        break;
                    case ResourceType.Wood:
                        woodHelper.CollectResource(tempStruct.resourceGenAmount);
                        break;
                    case ResourceType.Stone:
                        stoneHelper.CollectResource(tempStruct.resourceGenAmount);
                        break;
                    case ResourceType.Metal:
                        metalHelper.CollectResource(tempStruct.resourceGenAmount);
                        break;
                    default:
                        break;
                }
            }
        }
    }

    private void UpdateMoneyValueUI()
    {
        uiController.UpdateResourceValues(new Cost(
            goldHelper.Resource,
            foodHelper.Resource,
            woodHelper.Resource,
            stoneHelper.Resource,
            metalHelper.Resource
            ));
    }

    private void OnDisable()
    {
        CancelInvoke();
    }
}
