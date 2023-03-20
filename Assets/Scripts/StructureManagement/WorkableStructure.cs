using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkableStructure : Structure
{
    private ResourceGenStruct data;
    private Dictionary<GameObject, bool> workers;
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
        genAmount += baseGenAmount;
    }

    public void StopWorking(GameObject worker)
    {
        workers[worker] = false;
        genAmount -= baseGenAmount;
    }

    public bool HasCapacity()
    {
        return workers.Count < maxWorkerCapacity;
    }
}
