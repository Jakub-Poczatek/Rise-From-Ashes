using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System;
using UnityEngine;

public class ChallengeManager : MonoBehaviour
{
    public List<Challenge> challenges;

    // Start is called before the first frame update
    void Start()
    {
        LoadChallenges();
        Invoke(nameof(CheckChallenges), 5);
    }

    private void LoadChallenges()
    {
        TextAsset text = (TextAsset)Resources.Load("challenges");
        XmlDocument challengesXML = new XmlDocument();
        challengesXML.LoadXml(text.text);
        foreach (XmlNode c in challengesXML.SelectNodes("challenges/challenge"))
        {
            Challenge challenge = new(
                c.Attributes.GetNamedItem("message").Value, ParseReward(c));
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
                default:
                    break;
            }
        }
    }

    private void CheckChallenges()
    {
        foreach (Challenge c in challenges)
        {
            print(c.CheckConditions());
        }
    }

    private bool CheckStructureLevel(string structureName, int level)
    {
        return true;
    }

    private bool CheckResourceAmount(ResourceType resourceType, int amount)
    {
        return true;
    }
}
