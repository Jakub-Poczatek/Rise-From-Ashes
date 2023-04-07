using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Challenge
{
    public string name;
    public string message;
    public Cost reward;
    public List<Func<bool>> conditionList;

    public Challenge(string name, string message, Cost reward)
    {
        this.name = name;
        this.message = message;
        this.reward = reward;
        conditionList = new List<Func<bool>>();
    }

    public void AddCondition(Func<bool> condition)
    {
        conditionList.Add(condition);
    }

    public bool CheckConditions()
    {
        return conditionList.All(method => method());
    }
}
