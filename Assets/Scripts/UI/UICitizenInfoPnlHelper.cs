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
    public Button assignBtn, cancelBtn;

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

        goldLevel.text.Split(" ")[goldLevel.text.Split(" ").Length - 1] = data.skills.goldProductionLevel.ToString();
        foodLevel.text.Split(" ")[foodLevel.text.Split(" ").Length - 1] = data.skills.foodProductionLevel.ToString();
        woodLevel.text.Split(" ")[woodLevel.text.Split(" ").Length - 1] = data.skills.woodProductionLevel.ToString();
        stoneLevel.text.Split(" ")[stoneLevel.text.Split(" ").Length - 1] = data.skills.stoneProductionLevel.ToString();
        metalLevel.text.Split(" ")[metalLevel.text.Split(" ").Length - 1] = data.skills.metalProductionLevel.ToString();

        // Set skill info
        UpdateSliders(data);
        UpdateSliderValueLabels();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSliders(currentData);
        UpdateSliderValueLabels();
    }

    private void UpdateSliders(CitizenData data)
    {
        goldSlider.maxValue = data.skills.GoldExpUntilNextLevel;
        goldSlider.value = data.skills.GoldProductionExp;
        foodSlider.maxValue = data.skills.FoodExpUntilNextLevel;
        foodSlider.value = data.skills.FoodProductionExp;
        woodSlider.maxValue = data.skills.WoodExpUntilNextLevel;
        woodSlider.value = data.skills.WoodProductionExp;
        stoneSlider.maxValue = data.skills.StoneExpUntilNextLevel;
        stoneSlider.value = data.skills.StoneProductionExp;
        metalSlider.maxValue = data.skills.MetalExpUntlNextLevel;
        metalSlider.value = data.skills.MetalProductionExp;
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

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }
}
