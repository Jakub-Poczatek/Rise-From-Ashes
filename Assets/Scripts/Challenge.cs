using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Challenge
{
    public string message;
    public Cost reward;
    public List<Func<bool>> conditionList;

    public Challenge(string message, Cost reward)
    {
        this.message = message;
        this.reward = reward;
        conditionList = new List<Func<bool>>();
    }

    public void AddCondition(Func<bool> condition)
    {
        conditionList.Add(condition);
        Debug.Log("Condition added: " + condition.ToString());
    }

    public bool CheckConditions()
    {
        return conditionList.All(method => method());
    }
}
