using System.Collections;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

[TestFixture]
public class PlacementManagerTests
{
    Material transparentMaterial;
    PlacementManager placementManager;
    GameObject testGameObject;
    StructureBase structureBase;
    GridStructure gridStructure;
    Vector3 gridPosition1 = Vector3.zero;
    Vector3 gridPosition2 = new(5, 0, 5);

    [SetUp]
    public void Init()
    {
        GameObject ground = new();
        ground.transform.position = gridPosition1;
        testGameObject = TestHelpers.GetGameObjectWithMaterial();
        transparentMaterial = new(Shader.Find("Standard"));

        placementManager = Substitute.For<PlacementManager>();
        placementManager.ground = ground.transform;
        placementManager.transparentMaterial = transparentMaterial;

        structureBase = Substitute.For<ResourceGenStruct>();
        structureBase.prefab = testGameObject;
        gridStructure = new(1, 10, 10);
    }

    [UnityTest]
    public IEnumerator CreateGhostStructurePass()
    {
        (GameObject, Vector3, GameObject)? ghostReturn = 
            placementManager.CreateGhostStructure(gridPosition1, structureBase, gridStructure);
        yield return new WaitForEndOfFrame();
        foreach (MeshRenderer renderer in ghostReturn.Value.Item1.GetComponentsInChildren<MeshRenderer>())
        {
            Assert.AreEqual(renderer.material.color, Color.green);
        }
    }

    [UnityTest]
    public IEnumerator DisplayStructureOnMapMaterialPasses()
    {
        (GameObject, Vector3, GameObject)? ghostReturn =
            placementManager.CreateGhostStructure(gridPosition1, structureBase, gridStructure);
        placementManager.DisplayStructureOnMap(new List<GameObject>() { ghostReturn.Value.Item1 });
        yield return new WaitForEndOfFrame();
        foreach (MeshRenderer renderer in ghostReturn.Value.Item1.GetComponentsInChildren<MeshRenderer>())
        {
            Assert.AreEqual(renderer.material.color, Color.blue);
        }

    }

    [UnityTest]
    public IEnumerator PlacementManagerDestroyStructurePasses()
    {
        placementManager.SetStructureForDemolishing(testGameObject);
        yield return new WaitForEndOfFrame();
        foreach (MeshRenderer renderer in testGameObject.GetComponentsInChildren<MeshRenderer>())
        {
            Assert.AreEqual(renderer.material.color, Color.red);
        }


    }
}
