using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    public Transform ground;

    public void CreateBuilding(Vector3 gridPosition, GridStructure grid, StructureBase structure, ResourceManager resourceManager)
    {
        GameObject newStructure = Instantiate(structure.prefab, ground.position + gridPosition, Quaternion.identity);

        Vector3 size = newStructure.GetComponentInChildren<MeshRenderer>().bounds.size;
        Vector3 diff = new Vector3(calculateOffset(size.x), 0, calculateOffset(size.x));
        newStructure.transform.position += diff;
        gridPosition += diff;

        if (grid.CheckIfStructureFits(newStructure, gridPosition) && !grid.CheckIfStructureExists(newStructure, gridPosition))
        {
            resourceManager.buyStructure(structure);
            if (structure is ResourceGenStruct)
                resourceManager.adjustResourceGain((ResourceGenStruct) structure);
            grid.PlaceStructureOnTheGrid(newStructure, gridPosition);
        } else
            Destroy(newStructure);
    }

    public void RemoveBuilding(Vector3 gridPosition, GridStructure gridStructure)
    {
        var structure = gridStructure.GetStructureFromTheGrid(gridPosition);
        if (structure != null)
        {
            Destroy(structure);
            gridStructure.removeStructureFromTheGrid(gridPosition);
        }
    }

    private float calculateOffset(float vector)
    {
        if(vector != 1)
            return ((vector % 2f) / 2) + (1 - ((vector % 2f) / 2));
        return (vector % 2) / 2;
    }
}
