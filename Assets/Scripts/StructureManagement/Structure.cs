using System.Collections.Generic;
using UnityEngine;

public class Structure : MonoBehaviour
{
    private StructureBase baseData;
    private Dictionary<GameObject, bool> citizens;
    private Cost upgradeCost;
    private string structureName;
    private int structureLevel;
    private float maintenanceCost;
    private int maxCitizenCapacity;

    public float MaintenanceCost { get => maintenanceCost; }
    public StructureBase BaseData { get => baseData; set => baseData = value; }
    public string StructureName { get => structureName; set => structureName = value; }
    public int StructureLevel { get => structureLevel; set => structureLevel = value; }
    public Dictionary<GameObject, bool> Citizens { get => citizens; set => citizens = value; }
    public int MaxCitizenCapacity { 
        get => maxCitizenCapacity;
        set {
            maxCitizenCapacity = value;
            if (structureName.Contains("House"))
                ResourceManager.Instance.MaxCitizenCapacity++;
            HouseCitizenMaybe();
        }
    }
    public Cost UpgradeCost { get => upgradeCost; set => upgradeCost = value; }

    // Start is called before the first frame update
    void Start()
    {
        SetData();
        HouseCitizenMaybe();
    }

    private void HouseCitizenMaybe()
    {
        if (structureName.Contains("House"))
        {
            foreach (GameObject citizen in PopulationManagement.Instance.Citizens)
            {
                if (citizen.GetComponent<Citizen>().HouseBuilding == null)
                {
                    citizen.GetComponent<Citizen>().HouseBuilding = this.gameObject;
                    AddCitizen(citizen);
                    return;
                }
            }
        }
    }

    protected virtual void SetData()
    {
        structureName = baseData.name;
        maintenanceCost = baseData.maintenanceGoldCost;
        maxCitizenCapacity = baseData.maxCitizenCapacity;
        structureLevel = 1;
        citizens = new Dictionary<GameObject, bool>();
        upgradeCost = BaseData.buildCost * 1.5f;
    }

    public virtual void Upgrade()
    {
        if (structureLevel < 5 && ResourceManager.Instance.Purchase(upgradeCost))
        {
            upgradeCost *= 1.5f;
            structureLevel += 1;
            MaxCitizenCapacity++;
        }
    }

    public void AddCitizen(GameObject citizen)
    {
        citizens.Add(citizen, false);
    }

    public void RemoveCitizen(GameObject citizen)
    {
        citizens.Remove(citizen);
    }
    public bool HasCapacity()
    {
        return citizens.Count < maxCitizenCapacity;
    }
}
