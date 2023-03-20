using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkableStructure : Structure
{
    private ResourceGenStruct data;
    private Dictionary<GameObject, bool> workers;
    private Dictionary<GameObject, float> workersProdRate;
    private ResourceType resourceType;
    private float baseGenAmount;
    private float genAmount = 0;
    private int maxWorkerCapacity;

    public ResourceGenStruct Data { get => data; set => data = value; }
    public float GenAmount { get => genAmount; }
    public ResourceType ResourceType { get => resourceType; }

    // Start is called before the first frame update
    void Start()
    {
        data = (ResourceGenStruct) BaseData;
        SetData();
        workers = new Dictionary<GameObject, bool>();
        workersProdRate = new Dictionary<GameObject, float>();
    }

    protected override void SetData()
    {
        base.SetData();
        baseGenAmount = data.resourceGenAmount;
        resourceType = data.resourceType;
        maxWorkerCapacity = data.maxWorkerCapacity;
    }

    public void AddWorker(GameObject worker)
    {
        workers.Add(worker, false);
    }

    public void RemoveWorker(GameObject worker)
    {
        workers.Remove(worker);
    }

    public void StartWorking(GameObject worker)
    {
        workers[worker] = true;
        workersProdRate[worker] = GetWorkerLevelBonus(worker);
        genAmount += workersProdRate[worker];
    }

    public void StopWorking(GameObject worker)
    {
        workers[worker] = false;
        genAmount -= workersProdRate[worker];
    }

    private float GetWorkerLevelBonus(GameObject worker)
    {
        Skills workerSkills = worker.GetComponent<Citizen>().citizenData.skills;
        switch (resourceType)
        {
            case ResourceType.Gold:
                return baseGenAmount + workerSkills.goldProductionLevel;
            case ResourceType.Food:
                return baseGenAmount + workerSkills.foodProductionLevel;
            case ResourceType.Wood:
                return baseGenAmount + workerSkills.woodProductionLevel;
            case ResourceType.Stone:
                return baseGenAmount + workerSkills.stoneProductionLevel;
            case ResourceType.Metal:
                return baseGenAmount + workerSkills.metalProductionLevel;
            default:
                return 0;
        }
    }

    public bool HasCapacity()
    {
        return workers.Count < maxWorkerCapacity;
    }
}
