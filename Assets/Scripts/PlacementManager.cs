using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    public GameObject buildingPrefab;
    public Transform ground;

    public void CreateBuilding(Vector3 gridPosition, GridStructure grid, ResourceManager resourceManager)
    {
        Structure structure = buildingPrefab.GetComponent<Structure>();
        if(structure.goldCost > resourceManager.GoldAmount) { return; }

        GameObject newStructure = Instantiate(buildingPrefab, ground.position + gridPosition, Quaternion.identity);
        if (grid.CheckIfStructureFits(newStructure, gridPosition) && !grid.CheckIfStructureExists(newStructure, gridPosition))
        {
            resourceManager.GoldAmount -= structure.goldCost;
            resourceManager.GoldAmountChange += structure.goldGenerationAmount;
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
}
