using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour, IResourceManager
{
    [SerializeField] private int initialGold = 5000;
    [SerializeField] private float resourceCalculationInterval = 1;
    private BuildingManager buildingManager;
    public UIController uiController;
    private GoldHelper goldHelper;

    public int InitialGold { get => initialGold; }
    public float ResourceCalculationInterval { get => resourceCalculationInterval; }

    // Start is called before the first frame update
    void Start()
    {
        goldHelper = new GoldHelper(initialGold);
        UpdateMoneyValueUI();
    }

    public void PrepareResourceManager(BuildingManager buildingManager)
    {
        this.buildingManager = buildingManager;
        InvokeRepeating(nameof(CalculateIncome), 0, ResourceCalculationInterval);
    }

    public bool SpendGold(int amount)
    {
        if (CanIBuyIt(amount))
        {
            try
            {
                goldHelper.DecreaseGold(amount);
                UpdateMoneyValueUI();
                return true;
            }
            catch (GoldException)
            {
                ReloadGame();
            }
        }
        return false;
    }

    public void InceaseGold(int amount)
    {
        goldHelper.IncreaseGold(amount);
        UpdateMoneyValueUI();
    }

    private void ReloadGame()
    {
        Debug.Log("End the game");
    }

    public bool CanIBuyIt(int amount)
    {
        if (goldHelper.Gold >= amount) return true;
        else return false;
    }

    public void CalculateIncome()
    {
        try
        {
            goldHelper.CalculateGold(buildingManager.GetAllStructures());
            UpdateMoneyValueUI();
        }
        catch (GoldException)
        {

            ReloadGame();
        }
    }

    private void UpdateMoneyValueUI()
    {
        uiController.SetGoldValue(goldHelper.Gold);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }
}
