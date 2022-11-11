using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    public GameObject buildingPrefab;
    public Transform ground;

    public void CreateBuilding(Vector3 gridPosition, GridStructure grid)
    {
        GameObject newStructure = Instantiate(buildingPrefab, ground.position + gridPosition, Quaternion.identity);
        if (grid.CheckIfStructureFits(newStructure, gridPosition) && !grid.CheckIfStructureExists(newStructure, gridPosition))
            grid.PlaceStructureOnTheGrid(newStructure, gridPosition);
        else
            Destroy(newStructure);
    }
}
