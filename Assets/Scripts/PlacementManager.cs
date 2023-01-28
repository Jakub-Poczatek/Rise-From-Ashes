using NSubstitute.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    public Transform ground;
    public GameObject gridOutline;
    public Material transparentMaterial;
    private Dictionary<GameObject, Material[]> originalMaterials = new Dictionary<GameObject, Material[]>();

    /*public void CreateBuilding(Vector3 gridPosition, GridStructure grid, StructureBase structure, ResourceManager resourceManager)
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
                resourceManager.adjustResourceGain((ResourceGenStruct)structure);
            grid.PlaceStructureOnTheGrid(newStructure, gridPosition, gridOutline);
        }
        else
            Destroy(newStructure);
    }*/

    public GameObject CreateGhostStructure(Vector3 gridPosition, StructureBase structure)
    {

        GameObject newStructure = Instantiate(structure.prefab, ground.position + gridPosition, Quaternion.identity);
        Color colourToSet = Color.green;
        ChangeStructureMaterial(newStructure, colourToSet);
        return newStructure;
    }

    private void ChangeStructureMaterial(GameObject newStructure, Color colourToSet)
    {
        foreach (Transform child in newStructure.transform)
        {
            MeshRenderer meshRenderer = child.GetComponent<MeshRenderer>();
            if (originalMaterials.ContainsKey(child.gameObject) == false)
            {
                originalMaterials.Add(child.gameObject, meshRenderer.materials);
            }
            Material[] materialsToSet = new Material[meshRenderer.materials.Length];

            for (int i = 0; i < materialsToSet.Length; i++)
            {
                materialsToSet[i] = transparentMaterial;
                materialsToSet[i].color = colourToSet;
            }
            meshRenderer.materials = materialsToSet;
        }
    }

    public void DisplayStructureOnMap(IEnumerable<GameObject> structureCollection)
    {
        foreach (GameObject structure in structureCollection)
        {
            ResetBuildingMaterial(structure);
        }
        originalMaterials.Clear();
    }

    public void ResetBuildingMaterial(GameObject structure)
    {
        foreach (Transform child in structure.transform)
        {
            MeshRenderer meshRenderer = child.GetComponent<MeshRenderer>();
            if (originalMaterials.ContainsKey(child.gameObject))
            {
                meshRenderer.materials = originalMaterials[child.gameObject];
            }
        }
    }

    public void DestroyDisplayedStructures(IEnumerable<GameObject> structureCollection)
    {
        foreach (GameObject structure in structureCollection)
        {
            Destroy(structure);
        }
        originalMaterials.Clear();
    }

    public void SetBuildingForDemolishing(GameObject structureToDemolish)
    {
        Color colourToSet = Color.red;
        ChangeStructureMaterial(structureToDemolish, colourToSet);
    }

    /*public void RemoveBuilding(Vector3 gridPosition, GridStructure gridStructure)
    {
        var structure = gridStructure.GetStructureFromTheGrid(gridPosition);
        if (structure != null)
        {
            Destroy(structure);
            gridStructure.removeStructureFromTheGrid(gridPosition);
        }
    }*/

    private float calculateOffset(float vector)
    {
        if (vector != 1)
            return ((vector % 2f) / 2) + (1 - ((vector % 2f) / 2));
        return (vector % 2) / 2;
    }
}
