using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class WorkableStructure : Structure
{
    private ResourceGenStruct data;
    private Dictionary<GameObject, float> workersProdRate;
    private ResourceType resourceType;
    private float baseGenAmount;
    private float genAmount = 0;

    public ResourceGenStruct Data { get => data; set => data = value; }
    public float GenAmount { get => genAmount; }
    public ResourceType ResourceType { get => resourceType; }

    // Start is called before the first frame update
    void Start()
    {
        data = (ResourceGenStruct) BaseData;
        SetData();
        workersProdRate = new Dictionary<GameObject, float>();
    }

    protected override void SetData()
    {
        base.SetData();
        baseGenAmount = data.resourceGenAmount;
        resourceType = data.resourceType;
    }

    public override void Upgrade()
    {
        base.Upgrade();
        baseGenAmount = data.resourceGenAmount * (1f  + (0.25f * (StructureLevel-1)));
    }

    public void StartWorking(GameObject worker)
    {
        Citizens[worker] = true;
        workersProdRate[worker] = GetWorkerLevelBonus(worker);
        genAmount += workersProdRate[worker];
    }

    public void StopWorking(GameObject worker)
    {
        Citizens[worker] = false;
        genAmount -= workersProdRate[worker];
    }

    public override void RemoveCitizen(GameObject citizen)
    {
        base.RemoveCitizen(citizen);
        genAmount -= workersProdRate[citizen];
        workersProdRate.Remove(citizen);
    }

    private float GetWorkerLevelBonus(GameObject worker)
    {
        Skills workerSkills = worker.GetComponent<Citizen>().citizenData.skills;
        switch (resourceType)
        {
            case ResourceType.Gold:
                return baseGenAmount + workerSkills.Gold.productionLevel;
            case ResourceType.Food:
                return baseGenAmount + workerSkills.Food.productionLevel;
            case ResourceType.Wood:
                return baseGenAmount + workerSkills.Wood.productionLevel;
            case ResourceType.Stone:
                return baseGenAmount + workerSkills.Stone.productionLevel;
            case ResourceType.Metal:
                return baseGenAmount + workerSkills.Metal.productionLevel;
            default:
                return 0;
        }
    }
}
