using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CitizenData
{
    public string name;
    public Occupation occupation;
    public float health, food, happiness, loyalty;
    private (int, int) workRestRatio;
    public Skills skills;
    public Citizen parentCitizen;

    public (int, int) WorkRestRatio
    {
        get => workRestRatio;
        set
        {
            workRestRatio = value;
            if(workRestRatio.Item1 < 0) workRestRatio.Item1 = 0;
            if(workRestRatio.Item2 < 0) workRestRatio.Item2 = 0;
            if(workRestRatio.Item1 > 100) workRestRatio.Item1 = 100;
            if(workRestRatio.Item2 > 100) workRestRatio.Item2 = 100;
        }
    }

    public CitizenData(string name, Occupation occupation, float health, float food, (int, int) workRestRatio, float happiness, float loyalty, Skills skills, Citizen parentCitizen)
    {
        this.name = name;
        this.occupation = occupation;
        this.health = health;
        this.food = food;
        this.workRestRatio = workRestRatio;
        this.happiness = happiness;
        this.loyalty = loyalty;
        this.skills = skills;
        this.parentCitizen = parentCitizen;
    }
}

public enum Occupation
{
    Citizen,
    Merchant,
    Farmer,
    Miner,
    Logger
}
