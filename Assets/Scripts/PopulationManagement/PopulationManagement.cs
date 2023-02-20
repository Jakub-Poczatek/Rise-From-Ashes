using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulationManagement : MonoBehaviour
{
    public float citizenSpawnRate = 10.0f;
    public GameObject citizenPrefab;
    public int citizenLimit = 2;
    private List<GameObject> citizens = new List<GameObject>();
    public GameObject townHall;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("spawnCitizen", 0, citizenSpawnRate);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void spawnCitizen()
    {
        if (citizens.Count < citizenLimit) {
            GameObject citizen = Instantiate(citizenPrefab, townHall.transform.position + new Vector3(5, 0, 5), Quaternion.identity);
            citizens.Add(citizen);
        }
    }

    private void OnDisable()
    {
        CancelInvoke();
    }
}
