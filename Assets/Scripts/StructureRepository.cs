using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StructureRepository : MonoBehaviour
{
    public StructPool modelDataPool;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<string> GetResourceGenStructNames()
    {
        return modelDataPool.resourceGenStructs.Select(rgStruct => rgStruct.structureName).ToList();
    }

    public string GetRoadStructName()
    {
        return modelDataPool.RoadStruct.structureName;
    }

    public GameObject GetStructurePrefabByName(string structureName, StructureType structureType)
    {
        GameObject structurePrefabToReturn = null;
        switch (structureType)
        {
            case StructureType.ResourceGenStructure:
                structurePrefabToReturn = GetResourceGenStructPrefabByName(structureName);
                break;
            case StructureType.RoadStructure:
                structurePrefabToReturn = GetRoadStructPrefab();
                break;
            default:
                throw new Exception("No structure of this type implemented: " + structureType);
        }

        if (structurePrefabToReturn == null)
        {
            throw new Exception("No prefab of this name implemented: " + structureName);
        }
        return structurePrefabToReturn;
    }

    private GameObject GetRoadStructPrefab()
    {
        return modelDataPool.RoadStruct.prefab;
    }

    private GameObject GetResourceGenStructPrefabByName(string structureName)
    {
        var structure =  modelDataPool.resourceGenStructs.Where(
                s => s.structureName == structureName 
                ).FirstOrDefault();
        if(structure != null)
        {
            return structure.prefab;
        }
        return null;
    }
}

public enum StructureType
{
    ResourceGenStructure,
    RoadStructure
}
