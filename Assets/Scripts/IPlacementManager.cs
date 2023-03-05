/*using System.Collections.Generic;
using UnityEngine;

public interface IPlacementManager
{
    //void CreateBuilding(Vector3 gridPosition, GridStructure grid, StructureBase structure, ResourceManager resourceManager);
    (GameObject, Vector3, GameObject)? CreateGhostStructure(Vector3 gridPosition, StructureBase structure, GridStructure grid);
    (GameObject, Vector3, GameObject)? CreateGhostRoad(Vector3 gridPosition, GameObject structure, GridStructure grid, RotationValue rotationValue = RotationValue.R0);
    public GameObject InstantiateRoad(Vector3 gridPosition, GameObject structure, RotationValue rotationValue);
    void DestroyDisplayedStructures(IEnumerable<GameObject> structureCollection);
    void DisplayStructureOnMap(IEnumerable<GameObject> structureCollection);
    void ResetBuildingMaterial(GameObject structure);
    void SetStructureForDemolishing(GameObject structureToDemolish);
}*/