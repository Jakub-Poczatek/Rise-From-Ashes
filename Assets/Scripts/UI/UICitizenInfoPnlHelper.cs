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
        healthSlider.value = data.health;
        happinessSlider.value = data.happiness;
        foodAmount.text = data.dailyFood.ToString();
        energyAmount.text = data.dailySleep.ToString();

        goldLevel.text = "Gold Level: \t\t" + data.skills.goldProductionLevel.ToString();
        foodLevel.text = "Food Level: \t\t" + data.skills.foodProductionLevel.ToString();
        woodLevel.text = "Wood Level: \t" + data.skills.woodProductionLevel.ToString();
        stoneLevel.text = "Stone Level: \t" + data.skills.stoneProductionLevel.ToString();
        metalLevel.text = "Metal Level: \t" + data.skills.metalProductionLevel.ToString();

        // Set skill info
        UpdateSliders(data.skills);
        UpdateSliderValueLabels();
        UpdateLevels(data.skills);

        foodDecreaseBtn.onClick.AddListener(() => UpdateFood(-1));
        foodIncreaseBtn.onClick.AddListener(() => UpdateFood(1));
        sleepDecreaseBtn.onClick.AddListener(() => UpdateSleep(-1));
        sleepIncreaseBtn.onClick.AddListener(() => UpdateSleep(1));
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
        goldSlider.maxValue = skills.GoldExpUntilNextLevel;
        goldSlider.value = skills.GoldProductionExp;
        foodSlider.maxValue = skills.FoodExpUntilNextLevel;
        foodSlider.value = skills.FoodProductionExp;
        woodSlider.maxValue = skills.WoodExpUntilNextLevel;
        woodSlider.value = skills.WoodProductionExp;
        stoneSlider.maxValue = skills.StoneExpUntilNextLevel;
        stoneSlider.value = skills.StoneProductionExp;
        metalSlider.maxValue = skills.MetalExpUntlNextLevel;
        metalSlider.value = skills.MetalProductionExp;
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
        goldLevel.text = "Gold Level: \t\t" + skills.goldProductionLevel.ToString();
        foodLevel.text = "Food Level: \t\t" + skills.foodProductionLevel.ToString();
        woodLevel.text = "Wood Level: \t" + skills.woodProductionLevel.ToString();
        stoneLevel.text = "Stone Level: \t" + skills.stoneProductionLevel.ToString();
        metalLevel.text = "Metal Level: \t" + skills.metalProductionLevel.ToString();
    }

    private void UpdateFood(int amount)
    {
        currentData.dailyFood += amount;
        foodAmount.text = currentData.dailyFood.ToString();
    }

    private void UpdateSleep(int amount)
    {
        currentData.dailySleep += amount;
        energyAmount.text = currentData.dailySleep.ToString();
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
