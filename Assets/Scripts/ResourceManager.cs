using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public int initialGold = 5000;
    public float resourceCalculationInterval = 1;
    public BuildingManager buildingManager;
    public UIController uiController;
    private GoldHelper goldHelper;

    // Start is called before the first frame update
    void Start()
    {
        goldHelper = new GoldHelper(initialGold);
        UpdateMoneyValueUI();
    }

    public bool SpendGold(int amount)
    {
        if(CanIBuyIt(amount))
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

    private bool CanIBuyIt(int amount)
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
