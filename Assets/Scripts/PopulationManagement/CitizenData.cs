using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CitizenData
{
    public string name;
    public Occupation occupation;
    public float health, dailyFood, dailySleep, happiness, loyalty;
    public Skills skills;

    public CitizenData(string name, Occupation occupation, float health, float dailyFood, float dailySleep, float happiness, float loyalty, Skills skills)
    {
        this.name = name;
        this.occupation = occupation;
        this.health = health;
        this.dailyFood = dailyFood;
        this.dailySleep = dailySleep;
        this.happiness = happiness;
        this.loyalty = loyalty;
        this.skills = skills;
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
