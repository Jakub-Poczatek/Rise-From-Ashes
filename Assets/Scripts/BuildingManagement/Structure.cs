using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structure : MonoBehaviour
{
    private StructureBase baseData;
    protected string structureName;
    private float maintenanceCost;

    public float MaintenanceCost { get => maintenanceCost; }
    public StructureBase BaseData { get => baseData; set => baseData = value; }

    // Start is called before the first frame update
    void Start()
    {
        SetData();
    }

    protected virtual void SetData()
    {
        structureName = baseData.name;
        maintenanceCost = baseData.maintenanceGoldCost;
    }
}
