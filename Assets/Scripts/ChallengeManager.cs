using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System;
using UnityEngine;
using System.Linq;
using log4net.Core;

public class ChallengeManager : MonoBehaviour
{
    public List<Challenge> challenges;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;

        LoadChallenges();
    }

    public static ChallengeManager Instance { get; private set; }
    private ChallengeManager() { }

    private void LoadChallenges()
    {
        TextAsset text = (TextAsset)Resources.Load("challenges");
        XmlDocument challengesXML = new XmlDocument();
        challengesXML.LoadXml(text.text);
        foreach (XmlNode c in challengesXML.SelectNodes("challenges/challenge"))
        {
            Challenge challenge = new(
                c.Attributes.GetNamedItem("name").Value,
                c.Attributes.GetNamedItem("message").Value, 
                ParseReward(c));
            ParseConditions(c, challenge);
            challenges.Add(challenge);
        }
    }

    private Cost ParseReward(XmlNode challenge)
    {
        XmlNode reward = challenge.SelectNodes("reward").Item(0);
        return new Cost(
            float.Parse(reward.Attributes.GetNamedItem("gold").Value),
            float.Parse(reward.Attributes.GetNamedItem("food").Value),
            float.Parse(reward.Attributes.GetNamedItem("wood").Value),
            float.Parse(reward.Attributes.GetNamedItem("stone").Value),
            float.Parse(reward.Attributes.GetNamedItem("metal").Value)
            );
    }

    private void ParseConditions(XmlNode challengeNode, Challenge challenge)
    {
        foreach (XmlNode c in challengeNode.SelectNodes("conditions").Item(0))
        {
            switch (c.Attributes.GetNamedItem("type").Value)
            {
                case "structureLevel":
                    challenge.AddCondition(() => CheckStructureLevel(
                            c.Attributes.GetNamedItem("structure").Value,
                            int.Parse(c.Attributes.GetNamedItem("level").Value)
                        ));
                    break;
                case "structureCount":
                    challenge.AddCondition(() => CheckStructureCount(
                            c.Attributes.GetNamedItem("structure").Value,
                            int.Parse(c.Attributes.GetNamedItem("count").Value)
                        ));
                    break;
                case "resourceCount":
                    challenge.AddCondition(() => CheckResourceAmount(
                            (ResourceType) Enum.Parse(typeof(ResourceType), c.Attributes.GetNamedItem("resource").Value),
                            int.Parse(c.Attributes.GetNamedItem("count").Value)));
                    break;
                default:
                    break;
            }
        }
    }

    private bool CheckStructureLevel(string structureName, int level)
    {
        foreach (GameObject structure in BuildingManager.Instance.GetAllStructures())
        {
            if (structure.name.Contains(structureName))
            {
                if (structure.GetComponent<Structure>().StructureLevel >= level)
                    return true;
            }
        }
        return false;
    }

    private bool CheckResourceAmount(ResourceType resourceType, int amount)
    {
        return ResourceManager.Instance.CanIAffordItSingle(resourceType, amount);
    }

    private bool CheckStructureCount(string structureName, int count)
    {
        int counter = 0;
        foreach (GameObject structure in BuildingManager.Instance.GetAllStructures())
        {
            if (structure.name.Contains(structureName))
            {
                counter++;
            }
        }
        return counter >= count;
    }
}
