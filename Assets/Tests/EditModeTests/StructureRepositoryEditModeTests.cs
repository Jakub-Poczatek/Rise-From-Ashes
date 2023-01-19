using System.Collections;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class StructureRepositoryEditModeTests
{
    StructureRepository structureRepository;
    GameObject roadPrefab, resourceGeneratingStructurePrefab;

    [SetUp]
    public void Init()
    {
        structureRepository = Substitute.For<StructureRepository>();
        StructPool structPool = ScriptableObject.CreateInstance<StructPool>();
        roadPrefab = new();
        resourceGeneratingStructurePrefab = new();

        var road = ScriptableObject.CreateInstance<RoadStruct>();
        road.structureName = "Road";
        road.prefab = roadPrefab;
        var bank = ScriptableObject.CreateInstance<ResourceGenStruct>();
        bank.structureName = "Bank";
        bank.prefab = resourceGeneratingStructurePrefab;

        structPool.RoadStruct = road;
        structPool.resourceGenStructs = new List<ResourceGenStruct>
        {
            bank
        };
        GameObject gameObject = new();
        structureRepository = gameObject.AddComponent<StructureRepository>();
        structureRepository.modelDataPool = structPool;
    }

    #region GetStructureByName

    [Test]
    public void GetStructureByNameRoadStructurePass()
    {
        GameObject gameObject = structureRepository.GetStructureByName("Road", StructureType.RoadStructure).prefab;
        Assert.AreEqual(roadPrefab, gameObject);
    }

    [Test]
    public void GetStructureByNameResourceGenStructurePass()
    {
        GameObject gameObject = structureRepository.GetStructureByName("Bank", StructureType.ResourceGenStructure).prefab;
        Assert.AreEqual(resourceGeneratingStructurePrefab, gameObject);
    }

    [Test]
    public void GetStructureByNameResourceGenStructureNullFail()
    {
        Assert.That(() => structureRepository.GetStructureByName("NullStructure", StructureType.ResourceGenStructure), 
            Throws.Exception);
    }

    #endregion

    #region GetStructNames

    [Test]
    public void ResourceGenStructsQuantityPasses()
    {
        int quantity = structureRepository.GetResourceGenStructNames().Count;
        Assert.AreEqual(1, quantity);
    }

    [Test]
    public void ResourceGenStructsNamePasses()
    {
        string name = structureRepository.GetResourceGenStructNames()[0];
        Assert.AreEqual("Bank", name);
    }

    [Test]
    public void RoadStructNamePasses()
    {
        string name = structureRepository.GetRoadStructName();
        Assert.AreEqual("Road", name);
    }
    #endregion
}
