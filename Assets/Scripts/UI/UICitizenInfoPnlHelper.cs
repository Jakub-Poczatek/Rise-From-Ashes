using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICitizenInfoPnlHelper : MonoBehaviour
{
    public TMP_Text citizenName, citizenOccupation, healthAmount, happinessAmount,
        goldLevel, foodLevel, woodLevel, stoneLevel, metalLevel,
        goldExp, foodExp, woodExp, stoneExp, metalExp;
    public TMP_InputField foodAmount, energyAmount;
    public Slider healthSlider, happinessSlider, goldSlider, foodSlider, woodSlider, stoneSlider, metalSlider;
    public Button assignBtn, cancelBtn, foodDecreaseBtn, foodIncreaseBtn, sleepDecreaseBtn, sleepIncreaseBtn;

    private CitizenData currentData;

    public void DisplayCitizenMenu(CitizenData data)
    {
        Show();
        currentData = data;

        // Set basic info
        citizenName.text = data.name;
        citizenOccupation.text = data.occupation.ToString();

        // Set stat info
        healthSlider.value = data.Health;
        happinessSlider.value = data.Happiness;
        foodAmount.text = data.Food.ToString();
        energyAmount.text = data.WorkRestRatio.Item1 + ":" + currentData.WorkRestRatio.Item2;

        goldLevel.text = "Gold Level: \t\t" + data.skills.Gold.productionLevel.ToString();
        foodLevel.text = "Food Level: \t\t" + data.skills.Food.productionLevel.ToString();
        woodLevel.text = "Wood Level: \t" + data.skills.Wood.productionLevel.ToString();
        stoneLevel.text = "Stone Level: \t" + data.skills.Stone.productionLevel.ToString();
        metalLevel.text = "Metal Level: \t" + data.skills.Metal.productionLevel.ToString();

        // Set skill info
        UpdateSliders(data.skills);
        UpdateSliderValueLabels();
        UpdateLevels(data.skills);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSliders(currentData.skills);
        UpdateSliderValueLabels();
        UpdateLevels(currentData.skills);
    }

    private void UpdateSliders(Skills skills)
    {
        goldSlider.maxValue = skills.Gold.ExpUntilNextLevel;
        goldSlider.value = skills.Gold.ExpUntilNextLevel;
        foodSlider.maxValue = skills.Food.ExpUntilNextLevel;
        foodSlider.value = skills.Food.ProductionExp;
        woodSlider.maxValue = skills.Wood.ExpUntilNextLevel;
        woodSlider.value = skills.Wood.ProductionExp;
        stoneSlider.maxValue = skills.Stone.ExpUntilNextLevel;
        stoneSlider.value = skills.Stone.ProductionExp;
        metalSlider.maxValue = skills.Metal.ExpUntilNextLevel;
        metalSlider.value = skills.Metal.ProductionExp;
    }

    private void UpdateSliderValueLabels()
    {
        healthAmount.text = healthSlider.value.ToString();
        happinessAmount.text = happinessSlider.value.ToString();
        goldExp.text = goldSlider.value.ToString();
        foodExp.text = foodSlider.value.ToString();
        woodExp.text = woodSlider.value.ToString();
        stoneExp.text = stoneSlider.value.ToString();
        metalExp.text = metalSlider.value.ToString();
    }

    private void UpdateLevels(Skills skills)
    {
        goldLevel.text = "Gold Level: \t\t" + skills.Gold.productionLevel.ToString();
        foodLevel.text = "Food Level: \t\t" + skills.Food.productionLevel.ToString();
        woodLevel.text = "Wood Level: \t" + skills.Wood.productionLevel.ToString();
        stoneLevel.text = "Stone Level: \t" + skills.Stone.productionLevel.ToString();
        metalLevel.text = "Metal Level: \t" + skills.Metal.productionLevel.ToString();
    }

    public void UpdateFood(int amount)
    {
        currentData.Food += amount;
        foodAmount.text = currentData.Food.ToString();
        currentData.parentCitizen.UpdateHappiness();
        DisplayCitizenMenu(currentData);
    }

    public void UpdateSleep(int amount)
    {
        currentData.WorkRestRatio = (
            currentData.WorkRestRatio.Item1 + amount,
            currentData.WorkRestRatio.Item2 - amount);
        energyAmount.text = currentData.WorkRestRatio.Item1 + ":" + currentData.WorkRestRatio.Item2;
        currentData.parentCitizen.UpdateHappiness();
        DisplayCitizenMenu(currentData);
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }
}
