using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkableStructure : MonoBehaviour
{
    private Dictionary<GameObject, bool> workers;
    private ResourceGenStruct structureData;
    private string structureName;
    private float maintenanceCost;
    private float baseGenAmount;
    private float genAmount = 0;
    private ResourceType resourceType;
    private int maxWorkerCapacity;

    public ResourceGenStruct StructureData { get => structureData; set => structureData = value; }
    public float GenAmount { get => genAmount; }
    public ResourceType ResourceType { get => resourceType; }
    public float MaintenanceCost { get => maintenanceCost; }

    // Start is called before the first frame update
    void Start()
    {
        SetData();
        workers = new Dictionary<GameObject, bool>();
    }

    private void SetData()
    {
        structureName = structureData.structureName;
        maintenanceCost = structureData.maintenanceGoldCost;
        baseGenAmount = structureData.resourceGenAmount;
        resourceType = structureData.resourceType;
        maxWorkerCapacity = structureData.maxWorkerCapacity;
    }

    public void AddWorker(GameObject worker)
    {
        workers[worker] = false;
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
}
