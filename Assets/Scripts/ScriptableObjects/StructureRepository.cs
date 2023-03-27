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
        return modelDataPool.roadStruct.structureName;
    }

    public string GetResidentialStructName()
    {
        return modelDataPool.residentialStruct.structureName;
    }

    public StructureBase GetStructureByName(string structureName, StructureType structureType)
    {
        StructureBase foundStructure;
        switch (structureType)
        {
            case StructureType.ResourceGenStructure:
                foundStructure = GetResourceGenStructByName(structureName);
                break;
            case StructureType.RoadStructure:
                foundStructure = GetRoadStruct();
                break;
            case StructureType.ResidentialStructure:
                foundStructure = GetResidentialStruct();
                break;
            default:
                throw new Exception("Invalid Type: " + structureType);
        }

        if (foundStructure == null)
        {
            throw new Exception("Invalid prefab name: " + structureName);
        }
        return foundStructure;
    }

    private StructureBase GetResidentialStruct()
    {
        return modelDataPool.residentialStruct;
    }

    private RoadStruct GetRoadStruct()
    {
        return modelDataPool.roadStruct;
    }

    private ResourceGenStruct GetResourceGenStructByName(string structureName)
    {
        var structure =  modelDataPool.resourceGenStructs.Where(
                s => s.structureName == structureName 
                ).FirstOrDefault();
        if(structure != null)
        {
            return structure;
        }
        return null;
    }
}

public enum StructureType
{
    ResourceGenStructure,
    RoadStructure,
    ResidentialStructure,
    None
}
