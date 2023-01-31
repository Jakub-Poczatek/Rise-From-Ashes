using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceProcGen : MonoBehaviour
{
    public GameObject grid;
    public GameObject prefab;
    private GameObject parent;

    // Start is called before the first frame update
    void Start()
    {
        parent = new GameObject("Resources");

        Vector3 gridSize = grid.GetComponentInChildren<MeshRenderer>().bounds.size;
        Vector3 position;
        int resourceAmount = 500;
        for(int i = 0; i < resourceAmount; i++)
        {
            do
            {
                position = new Vector3(
                Random.Range(grid.transform.position.x, grid.transform.position.x + gridSize.x),
                0,
                Random.Range(grid.transform.position.z, grid.transform.position.z + gridSize.z)
                );
            } while (Physics.CheckSphere(position, 1.0f, LayerMask.GetMask("Resources")));
            
            GameObject r = Instantiate(prefab, position, Quaternion.identity);
            r.transform.SetParent(parent.transform);
        }    
    }
}
