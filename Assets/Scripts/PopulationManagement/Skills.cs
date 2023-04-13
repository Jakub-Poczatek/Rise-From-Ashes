using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Skills
{
    private Skill gold = new();
    private Skill food = new();
    private Skill wood = new();
    private Skill stone = new();
    private Skill metal = new();

    public Skill Gold { get => gold; set => gold = value; }
    public Skill Food { get => food; set => food = value; }
    public Skill Wood { get => wood; set => wood = value; }
    public Skill Stone { get => stone; set => stone = value; }
    public Skill Metal { get => metal; set => metal = value; }
}

public class Skill
{
    public int productionLevel = 1;
    private float productionExp = 0;
    private float expUntilNextLevel = 10;

    public float ProductionExp
    {
        get => productionExp;
        set
        {
            productionExp = value;
            if (productionExp > expUntilNextLevel)
            {
                productionLevel++;
                productionExp = 0;
                expUntilNextLevel *= 1.15f;
            }
        }
    }

    public float ExpUntilNextLevel { get => expUntilNextLevel; }

    public float GetExpDiff()
    {
        return expUntilNextLevel - productionExp;
    }
}
