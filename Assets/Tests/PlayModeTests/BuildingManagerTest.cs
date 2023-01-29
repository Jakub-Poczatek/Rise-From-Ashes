using System;
using System.Collections;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

[TestFixture]
public class BuildingManagerTest
{
    BuildingManager buildingManager;
    Material transparentMaterial;

    [SetUp]
    public void Init()
    {
        PlacementManager placementManager = Substitute.For<PlacementManager>();
        transparentMaterial = new(Shader.Find("Standard"));
        placementManager.transparentMaterial = transparentMaterial;
        GameObject ground = new();
        ground.transform.position = Vector3.zero;
        placementManager.ground = ground.transform;
        StructureRepository structureRepository = Substitute.For<StructureRepository>();
        StructPool structPool = ScriptableObject.CreateInstance<StructPool>();
        RoadStruct roadStruct = ScriptableObject.CreateInstance<RoadStruct>();
        roadStruct.name = "Road";
        GameObject roadModel = new("Road", typeof(MeshRenderer));
        roadModel.GetComponent<MeshRenderer>().material.color = Color.blue;
        GameObject road = new("Road");
        roadModel.transform.SetParent(road.transform);
        roadStruct.prefab = road;
        structPool.roadStruct = roadStruct;
        structureRepository.modelDataPool = structPool;
        ResourceManager resourceManager = Substitute.For<ResourceManager>();
        buildingManager = new(placementManager, resourceManager, structureRepository, 1, 10, 10);
    }

    [UnityTest]
    public IEnumerator PlacementConfirmationPassTest()
    {
        Vector3 position = PreparePlacement();
        buildingManager.ConfirmModification();
        yield return new WaitForEndOfFrame();
        position = position + new Vector3(1, 0, 1);
        Assert.IsNotNull(buildingManager.GetStructureFromGrid(position));
    }

    [UnityTest]
    public IEnumerator PlacementCancelTest()
    {
        Vector3 position = PreparePlacement();
        buildingManager.CancelModification();
        yield return new WaitForEndOfFrame();
        position = position + new Vector3(1, 0, 1);
        Assert.IsNull(buildingManager.GetStructureFromGrid(position));
    }

    [UnityTest]
    public IEnumerator DemolishConfirmationTest()
    {
        Vector3 position = PreparePlacement();
        position = position + new Vector3(1, 0, 1);
        PrepareDemolition(position);
        buildingManager.ConfirmModification();
        yield return new WaitForEndOfFrame();
        Assert.IsNull(buildingManager.GetStructureFromGrid(position));
    }



    [UnityTest]
    public IEnumerator DemolishNoConfirmationTest()
    {
        Vector3 position = PreparePlacement();
        position = position + new Vector3(1, 0, 1);
        PrepareDemolition(position);
        yield return new WaitForEndOfFrame();
        Assert.IsNotNull(buildingManager.GetStructureFromGrid(position));

    }

    [UnityTest]
    public IEnumerator DemolishCancelTest()
    {
        Vector3 position = PreparePlacement();
        position = position + new Vector3(1, 0, 1);
        PrepareDemolition(position);
        buildingManager.CancelModification();
        yield return new WaitForEndOfFrame();
        Assert.IsNotNull(buildingManager.GetStructureFromGrid(position));


    }

    [UnityTest]
    public IEnumerator MaterialChangeDuringPlacementTest()
    {
        Vector3 position = PreparePlacement();
        position = position + new Vector3(1, 0, 1);
        Material material = GetMaterial(position, () => buildingManager.GetStructureToBeModified(position));
        yield return new WaitForEndOfFrame();
        Assert.AreEqual(material.color, Color.green);
    }

    [UnityTest]
    public IEnumerator MaterialChangeAfterPlacementTest()
    {
        Vector3 position = PreparePlacement();
        position = position + new Vector3(1, 0, 1);
        buildingManager.ConfirmModification();
        Material material = GetMaterial(position, () => buildingManager.GetStructureFromGrid(position));
        yield return new WaitForEndOfFrame();
        Assert.AreEqual(material.color, Color.blue);
    }

    [UnityTest]
    public IEnumerator MaterialChangeDuringDemolishingTest()
    {
        Vector3 position = PreparePlacement();
        position = position + new Vector3(1, 0, 1);
        PrepareDemolition(position);
        Material material = GetMaterial(position, () => buildingManager.GetStructureToBeModified(position));
        yield return new WaitForEndOfFrame();
        Assert.AreEqual(material.color, Color.red);
    }

    [UnityTest]
    public IEnumerator MaterialChangeAfterDemolishingTest()
    {
        Vector3 position = PreparePlacement();
        position = position + new Vector3(1, 0, 1);
        PrepareDemolition(position);
        buildingManager.CancelModification();
        Material material = GetMaterial(position, () => buildingManager.GetStructureFromGrid(position));
        yield return new WaitForEndOfFrame();
        Assert.AreEqual(material.color, Color.blue);
    }

    private Material GetMaterial(Vector3 position, Func<GameObject> getMethod)
    {
        GameObject roadObject = getMethod();
        Material material = roadObject.GetComponentInChildren<MeshRenderer>().material;
        return material;
    }

    private Vector3 PreparePlacement()
    {
        Vector3 position = new(5, 0, 5);
        string structureName = "Road";
        buildingManager.PrepareBuildingManager(typeof(PlayerBuildingRoadState));
        buildingManager.PrepareStructureForModification(position, structureName, StructureType.RoadStructure);
        return position;
    }

    private void PrepareDemolition(Vector3 position)
    {
        buildingManager.ConfirmModification();
        buildingManager.PrepareBuildingManager(typeof(PlayerDemolishingState));
        buildingManager.PrepareStructureForDemolishing(position);
    }
}
