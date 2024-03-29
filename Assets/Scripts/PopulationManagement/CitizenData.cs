using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CitizenData
{
    public string name;
    public Occupation occupation;
    private float happiness, loyalty;
    private int food;
    public float health;
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

    public int Food { 
        get => food;
        set
        {
            food = value;
            if(value < 1) food = 1;
            if(value > 10) food = 10;
        }
    }

    public float Health { 
        get => health;
        set
        {
            health = value;
            if (health > 100) health = 100;
            if (health <= 0 && parentCitizen != null) parentCitizen.Die();
        }
    }

    public float Happiness
    {
        get => happiness;
        set
        {
            happiness = value;
            if(happiness < 0) happiness = 0;
            if(happiness > 100) happiness = 100;
        }
    }
    public float Loyalty { get => loyalty; set => loyalty = value; }

    public CitizenData(string name, Occupation occupation, float health, int food, (int, int) workRestRatio, float happiness, float loyalty, Skills skills, Citizen parentCitizen)
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
    Logger,
    Stonemason,
    Miner
}
