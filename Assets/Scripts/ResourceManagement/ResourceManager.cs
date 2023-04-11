using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    [SerializeField] private int initialGold = 1000;
    [SerializeField] private int initialFood = 50;
    [SerializeField] private int initialWood = 100;
    [SerializeField] private int initialStone = 100;
    [SerializeField] private int initialMetal = 0;
    [SerializeField] private float resourceCalculationInterval = 1;
    [SerializeField] private int maxCitizenCapacity = 3;
    [SerializeField] private int currentCitizenCapacity = 0;
    
    public UIController uiController;
    private GoldHelper goldHelper;
    private BasicResourceHelper foodHelper;
    private BasicResourceHelper woodHelper;
    private BasicResourceHelper stoneHelper;
    private BasicResourceHelper metalHelper;

    private Cost previousCost;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        goldHelper = new GoldHelper(initialGold);
        foodHelper = new BasicResourceHelper(initialFood);
        woodHelper = new BasicResourceHelper(initialWood);
        stoneHelper = new BasicResourceHelper(initialStone);
        metalHelper = new BasicResourceHelper(initialMetal);

        previousCost = new Cost(
            goldHelper.Resource,
            foodHelper.Resource,
            woodHelper.Resource,
            stoneHelper.Resource,
            metalHelper.Resource
            );

        UpdateMoneyValueUI();
    }

    public static ResourceManager Instance { get; private set; }

    public int MaxCitizenCapacity 
    { 
        get => maxCitizenCapacity;
        set
        {
            maxCitizenCapacity = value;
            if (maxCitizenCapacity < 0) maxCitizenCapacity = 0;
        }
    }

    public int CurrentCitizenCapacity 
    { 
        get => currentCitizenCapacity;
        set
        {
            currentCitizenCapacity = value;
            if (currentCitizenCapacity < 0) currentCitizenCapacity = 0;
        }
    }

    private ResourceManager() { }

    public void PrepareResourceManager()
    {
        InvokeRepeating(nameof(CalculateResources), 0, resourceCalculationInterval);
    }

    public bool CanIHouseCitizen()
    {
        return currentCitizenCapacity < maxCitizenCapacity;
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

    public bool CanIAffordItSingle(ResourceType resourceType, int amount)
    {
        return resourceType switch
        {
            ResourceType.Gold => goldHelper.Resource >= amount,
            ResourceType.Food => foodHelper.Resource >= amount,
            ResourceType.Wood => woodHelper.Resource >= amount,
            ResourceType.Stone => stoneHelper.Resource >= amount,
            ResourceType.Metal => metalHelper.Resource >= amount,
            _ => false,
        };
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
        previousCost = new Cost(
            goldHelper.Resource,
            foodHelper.Resource,
            woodHelper.Resource,
            stoneHelper.Resource,
            metalHelper.Resource
            );

        //IEnumerable<StructureBase> structures = buildingManager.GetAllStructuresData();
        IEnumerable<GameObject> structures = BuildingManager.Instance.GetAllStructures();
        CollectResourceGains(structures);
        goldHelper.Maintain(structures);

        // Consume food
        foreach (GameObject c in PopulationManagement.Instance.Citizens)
        {
            foodHelper.AdjustResource(-c.GetComponent<Citizen>().citizenData.food);
        }

        UpdateMoneyValueUI();
    }

    private void CollectResourceGains(IEnumerable<GameObject> structures)
    {
        foreach (GameObject structure in structures)
        {
            if (structure.CompareTag("ResGenStructure"))
            {
                WorkableStructure workableStructure = structure.GetComponent<WorkableStructure>();
                switch (workableStructure.ResourceType)
                {
                    case ResourceType.Gold:
                        goldHelper.AdjustResource(workableStructure.GenAmount);
                        break;
                    case ResourceType.Food:
                        foodHelper.AdjustResource(workableStructure.GenAmount);
                        break;
                    case ResourceType.Wood:
                        woodHelper.AdjustResource(workableStructure.GenAmount);
                        break;
                    case ResourceType.Stone:
                        stoneHelper.AdjustResource(workableStructure.GenAmount);
                        break;
                    case ResourceType.Metal:
                        metalHelper.AdjustResource(workableStructure.GenAmount);
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
            ), previousCost);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    // Testing Purposes
    public void SetUpForTests()
    {
        goldHelper = new GoldHelper(1000);
        foodHelper = new BasicResourceHelper(1000);
        woodHelper = new BasicResourceHelper(1000);
        stoneHelper = new BasicResourceHelper(1000);
        metalHelper = new BasicResourceHelper(1000);
    }
}
