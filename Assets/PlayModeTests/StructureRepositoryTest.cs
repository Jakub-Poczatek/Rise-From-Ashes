using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class StructureRepositoryTest
{
    StructureRepository structureRepository;

    [SetUp]
    public void Init()
    {
        StructPool structPool = new StructPool();
        var road = new RoadStruct();
        road.structureName = "Road";
        var bank = new ResourceGenStruct();
        bank.structureName = "Bank";
        structPool.RoadStruct = road;
        structPool.resourceGenStructs = new List<ResourceGenStruct>();
        structPool.resourceGenStructs.Add(bank);
        GameObject gameObject = new GameObject();
        structureRepository = gameObject.AddComponent<StructureRepository>();
        structureRepository.modelDataPool = structPool;
    }

    [UnityTest]
    public IEnumerator ResourceGenStructsQuantityPasses()
    {
        int quantity = structureRepository.GetResourceGenStructNames().Count;
        yield return new WaitForEndOfFrame();
        Assert.AreEqual(1, quantity);
    }

    [UnityTest]
    public IEnumerator ResourceGenStructsNamePasses()
    {
        string name = structureRepository.GetResourceGenStructNames()[0];
        yield return new WaitForEndOfFrame();
        Assert.AreEqual("Bank", name);
    }

    [UnityTest]
    public IEnumerator RoadStructNamePasses()
    {
        string name = structureRepository.GetRoadStructName();
        yield return new WaitForEndOfFrame();
        Assert.AreEqual("Road", name);
    }
}
