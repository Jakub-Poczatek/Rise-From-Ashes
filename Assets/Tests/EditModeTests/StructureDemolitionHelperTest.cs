using System.Collections;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

[TestFixture]
public class StructureDemolitionHelperTest
{
    GameObject tempObject;
    GridStructure gridStructure;
    Vector3 gridPosition1 = Vector3.zero;
    Vector3 gridPosition2 = new(5, 0, 5);
    StructureModificationHelper helper;

    [SetUp]
    public void Init()
    {
        StructureRepository structureRepository = TestHelpers.CreateStructureRepositoryContainingRoad();
        PlacementManager placementManager = Substitute.For<PlacementManager>();
        tempObject = new GameObject();
        tempObject.AddComponent<MeshRenderer>();
        placementManager.CreateGhostResGen(default, default, default)
            .ReturnsForAnyArgs((tempObject, gridPosition1, tempObject), (tempObject, gridPosition2, tempObject));
        gridStructure = new GridStructure(1, 10, 10);

        gridStructure.PlaceStructureOnTheGrid(tempObject, gridPosition1, null);
        gridStructure.PlaceStructureOnTheGrid(tempObject, gridPosition2, null);

        helper = new StructureDemolishingHelper(structureRepository, gridStructure);    
    }

    [Test]
    public void SelectForDemolishingPasses()
    {
        helper.PrepareStructureForModification(gridPosition1);
        GameObject objectInDictionary = helper.GetStructureToBeModified(gridPosition1);
        Assert.AreEqual(tempObject, objectInDictionary);
    }

    [Test]
    public void CancelDemolishingPasses()
    {
        helper.PrepareStructureForModification(gridPosition1);
        helper.CancelModifications();
        Assert.IsTrue(gridStructure.IsCellTaken(gridPosition1));
    }

    [Test]
    public void StructureDemolitionHelperConfirmForDemolitionPasses()
    {
        helper.PrepareStructureForModification(gridPosition1);
        helper.ConfirmModifications();
        Assert.IsFalse(gridStructure.IsCellTaken(gridPosition1));
    }
}