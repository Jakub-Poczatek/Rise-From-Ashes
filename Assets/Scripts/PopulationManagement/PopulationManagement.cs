using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulationManagement : MonoBehaviour
{
    public float citizenSpawnRate = 10.0f;
    private float baseCitizenSpawnRate = 0f;
    private float citizenSpawnRateTimer = 0f;
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
        baseCitizenSpawnRate = citizenSpawnRate;
        resourceManager = ResourceManager.Instance;
    }

    private void Update()
    {
        citizenSpawnRateTimer += Time.deltaTime;
        if(citizens.Count > 0)
            citizenSpawnRate = (baseCitizenSpawnRate * citizens.Count / (ResourceManager.Instance.AvgHappiness*2)) * 1000;
        else citizenSpawnRate = 0;
        if(citizenSpawnRateTimer > citizenSpawnRate)
        {
            SpawnCitizen();
        }
        print(GetTimeUntilNewCitizen());
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
            citizenSpawnRateTimer = 0;
        }
    }

    public float GetTimeUntilNewCitizen()
    {
        return Mathf.Clamp(citizenSpawnRate - citizenSpawnRateTimer, 0, float.MaxValue);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }
}
