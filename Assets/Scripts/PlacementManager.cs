using NSubstitute.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementManager : MonoBehaviour, IPlacementManager
{
    public Transform ground;
    public GameObject gridOutline;
    public Material transparentMaterial;
    private Dictionary<GameObject, Material[]> originalMaterials = new Dictionary<GameObject, Material[]>();

    public (GameObject, Vector3, GameObject)? CreateGhostStructure(Vector3 gridPosition, StructureBase structure,
        GridStructure gridStructure)
    {

        GameObject newStructure = Instantiate(structure.prefab, ground.position + gridPosition, Quaternion.identity);
        newStructure.GetComponent<WorkableStructure>().StructureData = (ResourceGenStruct) structure;
        Vector3 size = newStructure.GetComponentInChildren<MeshRenderer>().bounds.size;

        // Maybe change size.z to size.x
        Vector3 diff = new Vector3(calculateOffset(size.x), 0, calculateOffset(size.z));
        newStructure.transform.position += diff;
        gridPosition += diff;

        if (gridStructure.CheckIfStructureFits(newStructure, gridPosition) && !gridStructure.CheckIfStructureExists(newStructure, gridPosition))
        {
            Color colourToSet = Color.green;
            ChangeStructureMaterial(newStructure, colourToSet);
            return (newStructure, gridPosition, gridOutline);
        }
        else
        {
            Destroy(newStructure);
            return null;
        }
    }

    public (GameObject, Vector3, GameObject)? CreateGhostRoad(Vector3 gridPosition, GameObject structure,
        GridStructure gridStructure, RotationValue rotationValue = RotationValue.R0)
    {
        GameObject newStructure = InstantiateRoad(gridPosition, structure, rotationValue);

        Vector3 size = newStructure.GetComponentInChildren<MeshRenderer>().bounds.size;

        // Maybe change size.z to size.x
        Vector3 diff = new Vector3(calculateOffset(size.x), 0, calculateOffset(size.z));
        newStructure.transform.position += diff;
        gridPosition += diff;

        if (gridStructure.CheckIfStructureFits(newStructure, gridPosition) && !gridStructure.CheckIfStructureExists(newStructure, gridPosition))
        {
            Color colourToSet = Color.green;
            ChangeStructureMaterial(newStructure, colourToSet);
            return (newStructure, gridPosition, null);
        }
        else
        {
            Destroy(newStructure);
            return null;
        }
    }

    public GameObject InstantiateRoad(Vector3 gridPosition, GameObject structure, RotationValue rotationValue)
    {
        GameObject newStructure = Instantiate(structure, ground.position + gridPosition, Quaternion.identity);
        Vector3 rotation = Vector3.zero;
        switch (rotationValue)
        {
            case RotationValue.R0:
                break;
            case RotationValue.R90:
                rotation = new Vector3(0, 90, 0);
                break;
            case RotationValue.R180:
                rotation = new Vector3(0, 180, 0);
                break;
            case RotationValue.R270:
                rotation = new Vector3(0, 270, 0);
                break;
            default:
                break;
        }
        foreach (Transform child in newStructure.transform)
        {
            child.Rotate(rotation, Space.World);
        }
        return newStructure;
    }

    private void ChangeStructureMaterial(GameObject newStructure, Color colourToSet)
    {
        foreach (Transform child in newStructure.transform)
        {
            if (child.GetComponent<MeshRenderer>() != null)
            {
                MeshRenderer meshRenderer = child.GetComponent<MeshRenderer>();
                if (originalMaterials.ContainsKey(child.gameObject) == false)
                {
                    originalMaterials.Add(child.gameObject, meshRenderer.materials);
                }
                Material[] materialsToSet = new Material[meshRenderer.materials.Length];
                colourToSet.a = 0.5f;
                for (int i = 0; i < materialsToSet.Length; i++)
                {
                    materialsToSet[i] = transparentMaterial;
                    materialsToSet[i].color = colourToSet;
                }
                meshRenderer.materials = materialsToSet;
            }
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

    public void SetStructureForDemolishing(GameObject structureToDemolish)
    {
        Color colourToSet = Color.red;
        ChangeStructureMaterial(structureToDemolish, colourToSet);
    }

    private float calculateOffset(float vector)
    {
        if (vector > 1)
        {
            return ((vector % 2f) / 2f) + (1f - ((vector % 2f) / 2f));
        }
        return (vector % 2f) / 2f;
    }
}
