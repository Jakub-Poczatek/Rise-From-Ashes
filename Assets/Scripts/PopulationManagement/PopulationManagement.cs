using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulationManagement : MonoBehaviour
{
    public float citizenSpawnRate = 10.0f;
    public GameObject citizenPrefab;
    private List<GameObject> citizens;
    public GameObject townHall;
    private ResourceManager resourceManager;

    public static PopulationManagement Instance { get; private set; }
    public List<GameObject> Citizens { get => citizens; set => citizens = value; }

    private PopulationManagement() {}

    private void Awake()
    {
        citizens = new List<GameObject>();
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        resourceManager = ResourceManager.Instance;
        InvokeRepeating(nameof(SpawnCitizen), 0, citizenSpawnRate);
    }

    private void SpawnCitizen()
    {
        if (resourceManager.CanIHouseCitizen()) {
            GameObject citizen = Instantiate(citizenPrefab, townHall.transform.position, Quaternion.identity);
            resourceManager.CurrentCitizenCapacity++;
            Citizens.Add(citizen);
            foreach (GameObject structure in BuildingManager.Instance.GetAllStructures())
            {
                if (structure.name.Contains("House"))
                {
                    if (structure.GetComponent<Structure>().HasCapacity())
                    {
                        structure.GetComponent<Structure>().AddCitizen(citizen);
                        citizen.GetComponent<Citizen>().HouseBuilding = structure;
                        break;
                    }
                }
            }
        }
    }

    private void OnDisable()
    {
        CancelInvoke();
    }
}
