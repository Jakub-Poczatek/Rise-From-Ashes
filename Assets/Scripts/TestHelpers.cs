using NSubstitute;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TestHelpers
{
    public static StructureRepository CreateStructureRepositoryContainingRoad()
    {
        StructureRepository structureRepository = Substitute.For<StructureRepository>();
        StructPool structPool = ScriptableObject.CreateInstance<StructPool>();
        RoadStruct roadStruct = ScriptableObject.CreateInstance<RoadStruct>();
        roadStruct.name = "Road";
        roadStruct.prefab = GetGameObjectWithMaterial();
        structPool.roadStruct = roadStruct;
        structureRepository.modelDataPool = structPool;
        return structureRepository;
    }

    public static GameObject GetGameObjectWithMaterial()
    {
        GameObject roadModel = new("Road", typeof(MeshRenderer));
        MeshRenderer meshRenderer = roadModel.GetComponent<MeshRenderer>();
        Material blue = Resources.Load("Blue", typeof(Material)) as Material;
        meshRenderer.material = blue;
        GameObject road = new("Road");
        roadModel.transform.SetParent(road.transform);
        return road;
    }
}