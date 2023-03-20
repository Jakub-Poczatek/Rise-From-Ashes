using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Skills
{
    public int goldProductionLevel = 0;
    private float goldProductionExp = 0;
    private float goldExpUntilNextLevel = 0;
    public int foodProductionLevel = 0;
    private float foodProductionExp = 0;
    private float foodExpUntilNextLevel = 0;
    public int woodProductionLevel = 0;
    private float woodProductionExp = 0;
    private float woodExpUntilNextLevel = 0;
    public int stoneProductionLevel = 0;
    private float stoneProductionExp = 0;
    private float stoneExpUntilNextLevel = 0;
    public int metalProductionLevel = 0;
    private float metalProductionExp = 0;
    private float metalExpUntlNextLevel = 0;

    public float GoldProductionExp 
    { 
        get => goldProductionExp;
        set
        {
            goldProductionExp = value;
            if (goldProductionExp > goldExpUntilNextLevel)
            {
                goldProductionLevel++;
                goldProductionExp = 0;
                goldExpUntilNextLevel = goldExpUntilNextLevel * 1.15f;
            }
        }
    }

    public float FoodProductionExp 
    { 
        get => foodProductionExp; 
        set
        {
            foodProductionExp = value;
            if (foodProductionExp > foodExpUntilNextLevel)
            {
                foodProductionLevel++;
                foodProductionExp = 0;
                foodExpUntilNextLevel = foodExpUntilNextLevel * 1.15f;
            }
        } 
    }

    public float WoodProductionExp 
    { 
        get => woodProductionExp;
        set
        {
            woodProductionExp = value;
            if (woodProductionExp > woodExpUntilNextLevel)
            {
                woodProductionLevel++;
                woodProductionExp = 0;
                woodExpUntilNextLevel = woodExpUntilNextLevel * 1.15f;
            }
        }
    }

    public float StoneProductionExp 
    { 
        get => stoneProductionExp;
        set
        {
            stoneProductionExp = value;
            if (stoneProductionExp > stoneExpUntilNextLevel)
            {
                stoneProductionLevel++;
                stoneProductionExp = 0;
                stoneExpUntilNextLevel = stoneExpUntilNextLevel * 1.15f;
            }
        }
    }

    public float MetalProductionExp 
    { 
        get => metalProductionExp;
        set
        {
            metalProductionExp = value;
            if (metalProductionExp > metalExpUntlNextLevel)
            {
                metalProductionLevel++;
                metalProductionExp = 0;
                metalExpUntlNextLevel = metalExpUntlNextLevel * 1.15f;
            }
        }
    }

    public float GoldExpUntilNextLevel { get => goldExpUntilNextLevel; }
    public float FoodExpUntilNextLevel { get => foodExpUntilNextLevel; }
    public float WoodExpUntilNextLevel { get => woodExpUntilNextLevel; }
    public float StoneExpUntilNextLevel { get => stoneExpUntilNextLevel; }
    public float MetalExpUntlNextLevel { get => metalExpUntlNextLevel; }

    public Skills(int goldLevel, int foodLevel, int woodLevel, int stoneLevel, int metalLevel)
    {
        this.goldProductionLevel = goldLevel;
        this.foodProductionLevel = foodLevel;
        this.woodProductionLevel = woodLevel;
        this.stoneProductionLevel = stoneLevel;
        this.metalProductionLevel = metalLevel;
    }
}
