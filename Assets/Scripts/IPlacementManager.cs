using System.Collections.Generic;
using UnityEngine;

public interface IPlacementManager
{
    void CreateBuilding(Vector3 gridPosition, GridStructure grid, StructureBase structure, ResourceManager resourceManager);
    (GameObject, Vector3, GameObject)? CreateGhostStructure(Vector3 gridPosition, StructureBase structure, GridStructure grid, ResourceManager resourceManager);
    void DestroyDisplayedStructures(IEnumerable<GameObject> structureCollection);
    void DisplayStructureOnMap(IEnumerable<GameObject> structureCollection);
    void ResetBuildingMaterial(GameObject structure);
    void SetStructureForDemolishing(GameObject structureToDemolish);
}