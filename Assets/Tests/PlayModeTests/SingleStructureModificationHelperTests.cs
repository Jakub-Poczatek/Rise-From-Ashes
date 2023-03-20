using System.Collections;
using System.Collections.Generic;
using NSubstitute;
using NSubstitute.Extensions;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

[TestFixture]
public class SingleStructureModificationHelperTests
{
    GameObject tempObject;
    GridStructure gridStructure;
    StructureType structureType = StructureType.RoadStructure;
    string structureName = "Road";
    Vector3 gridPosition1 = Vector3.zero;
    Vector3 gridPosition2 = new(5, 0, 5);
    StructureModificationHelper helper;

    [SetUp]
    public void Init()
    {
        StructureRepository structureRepository = TestHelpers.CreateStructureRepositoryContainingRoad();
        //IPlacementManager placementManager = Substitute.For<IPlacementManager>();
        //PlacementManager placementManager = PlacementManager.Instance;
        ResourceManager resourceManager = ResourceManager.Instance;
        GameObject resourceManagerObject = new GameObject();
        resourceManagerObject.AddComponent<ResourceManager>();
        //resourceManager.SetUpForTests();
        //PlacementManager placementManager = Substitute.For<PlacementManager>();
        tempObject = new GameObject();
        tempObject.AddComponent<MeshRenderer>();
        //placementManager.CreateGhostStructure(default, default, default)
        //    .Returns((tempObject, gridPosition1, tempObject), (tempObject, gridPosition2, tempObject));
        gridStructure = new GridStructure(1, 10, 10);
        helper = new ResGenModificationHelper(structureRepository, gridStructure);
    }

    [UnityTest]
    public IEnumerator AddPositionPass()
    {
        yield return new WaitForEndOfFrame();
        helper.PrepareStructureForModification(gridPosition1, structureName, structureType);
        GameObject objectInDictionary = helper.GetStructureToBeModified(gridPosition1);
        Assert.AreEqual(tempObject, objectInDictionary);
    }

    [Test]
    public void RemoveFromPositionsPasses()
    {
        helper.PrepareStructureForModification(gridPosition1, structureName, structureType);
        helper.CancelModifications();
        GameObject objectInDictionary = helper.GetStructureToBeModified(gridPosition1);
        Assert.IsNull(objectInDictionary);
    }

    [Test]
    public void AddToPositionsTwoTimesPasses()
    {
        helper.PrepareStructureForModification(gridPosition1, structureName, structureType);
        helper.PrepareStructureForModification(gridPosition2, structureName, structureType);
        GameObject objectInDictionary1 = helper.GetStructureToBeModified(gridPosition1);
        GameObject objectInDictionary2 = helper.GetStructureToBeModified(gridPosition2);
        Assert.AreEqual(tempObject, objectInDictionary1);
        Assert.AreEqual(tempObject, objectInDictionary2);
    }

    [Test]
    public void RemoveFromAllPositionsPasses()
    {
        helper.PrepareStructureForModification(gridPosition1, structureName, structureType);
        helper.PrepareStructureForModification(gridPosition2, structureName, structureType);
        helper.CancelModifications();
        GameObject objectInDictionary1 = helper.GetStructureToBeModified(gridPosition1);
        GameObject objectInDictionary2 = helper.GetStructureToBeModified(gridPosition2);
        Assert.IsNull(objectInDictionary1);
        Assert.IsNull(objectInDictionary2);
    }

    [Test]
    public void AddToGridPasses()
    {
        helper.PrepareStructureForModification(gridPosition1, structureName, structureType);
        helper.PrepareStructureForModification(gridPosition2, structureName, structureType);
        helper.ConfirmModifications();
        Assert.IsTrue(gridStructure.IsCellTaken(gridPosition1));
        Assert.IsTrue(gridStructure.IsCellTaken(gridPosition2));
    }
}
