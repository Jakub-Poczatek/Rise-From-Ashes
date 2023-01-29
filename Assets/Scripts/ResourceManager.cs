using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    private float timer;
    private float oneSecond = 1;
    private float goldAmount = 100;
    private float woodAmount = 100;
    private float stoneAmount = 100;
    private float goldGain = 0;
    private float woodGain = 0;
    private float stoneGain = 0;
    private float tempGoldGain = 0;
    private float tempWoodGain = 0;
    private float tempStoneGain = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if(timer > oneSecond)
        {
            timer -= oneSecond;

            goldAmount += goldGain;
            woodAmount += woodGain;
            stoneAmount += stoneGain;
        }
    }

    public bool isAffordable(StructureBase structure)
    {
        return  goldAmount > structure.buildCost.gold &&
                stoneAmount > structure.buildCost.stone &&
                woodAmount > structure.buildCost.wood;
    }

    public void buyStructure(StructureBase structure)
    {
        goldAmount -= structure.buildCost.gold;
        stoneAmount -= structure.buildCost.stone;
        woodAmount -= structure.buildCost.wood;
    }

    public void adjustResourceGain(ResourceGenStruct structure)
    {
        switch (structure.resourceType)
        {
            case ResourceType.Gold:
                goldGain += structure.resourceGenAmount;
                break;
            case ResourceType.Wood:
                woodGain += structure.resourceGenAmount;
                break;
            case ResourceType.Stone:
                stoneGain += structure.resourceGenAmount;
                break;
            default:
                throw new System.Exception("Invalid resource gain type: " + structure.resourceType);
        }
    }

    public void adjustTempResourceGain(ResourceGenStruct structure)
    {
        switch (structure.resourceType)
        {
            case ResourceType.Gold:
                tempGoldGain += structure.resourceGenAmount;
                break;
            case ResourceType.Wood:
                tempWoodGain += structure.resourceGenAmount;
                break;
            case ResourceType.Stone:
                tempStoneGain += structure.resourceGenAmount;
                break;
            default:
                throw new System.Exception("Invalid resource gain type: " + structure.resourceType);
        }
    }

    public void SyncResourceGains()
    {
        goldGain += tempGoldGain;
        woodGain += tempWoodGain;
        stoneGain += tempStoneGain;
        ClearTempResourceGain();
    }

    public void ClearTempResourceGain()
    {
        tempGoldGain = 0;
        tempWoodGain = 0;
        tempStoneGain = 0;
    }

    public float GoldAmount { get => goldAmount;}
    public float WoodAmount { get => woodAmount;}
    public float StoneAmount { get => stoneAmount;}
}
