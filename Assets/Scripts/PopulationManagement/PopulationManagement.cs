using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulationManagement : MonoBehaviour
{
    public float citizenSpawnRate = 10.0f;
    public GameObject citizenPrefab;
    private List<GameObject> citizens = new List<GameObject>();
    public GameObject townHall;
    private ResourceManager resourceManager;

    // Start is called before the first frame update
    void Start()
    {
        resourceManager = ResourceManager.Instance;
        InvokeRepeating("spawnCitizen", 0, citizenSpawnRate);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void spawnCitizen()
    {
        if (resourceManager.CanIHouseCitizen()) {
            GameObject citizen = Instantiate(citizenPrefab, townHall.transform.position + new Vector3(5, 0, 5), Quaternion.identity);
            resourceManager.CurrentCitizenCapacity++;
            citizens.Add(citizen);
        }
    }

    private void OnDisable()
    {
        CancelInvoke();
    }
}
