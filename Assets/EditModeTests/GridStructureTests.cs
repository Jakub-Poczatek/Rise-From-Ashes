using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class GridStructureTests
{
    GridStructure grid;

    [SetUp]
    public void SetUp()
    {
        grid = new(1, 100, 100);
    }

    #region CalculateGridPositionTests

    [Test]
    public void CalculateGridPositionPass()
    {
        // Setup
        Vector3 position = new(0, 0, 0);
        Vector3 returnPosition = grid.CalculateGridPosition(position);

        // Assert
        Assert.AreEqual(Vector3.zero, returnPosition);
    }

    [Test]
    public void CalculateGridPositionFloatsPass()
    {
        // Setup
        Vector3 position = new(0.9f, 0, 0.1f);
        Vector3 returnPosition = grid.CalculateGridPosition(position);

        // Assert
        Assert.AreEqual(Vector3.zero, returnPosition);
    }

    [Test]
    public void CalculateGridPositionFail()
    {
        // Setup
        Vector3 position = new(1.1f, 0, 0);
        Vector3 returnPosition = grid.CalculateGridPosition(position);

        // Assert
        Assert.AreNotEqual(Vector3.zero, returnPosition);
    }
    #endregion

    #region PlaceStructureAndIsCellTakenTests

    [Test]
    public void PlaceStructure101AndCheckIsTakenPass()
    {
        // Setup
        Vector3 position = new(1, 0, 1);
        GameObject gameObject = new("gameObject");
        gameObject.AddComponent<MeshRenderer>();
        gameObject.GetComponent<MeshRenderer>().bounds = new Bounds(Vector3.zero, new(4, 4, 4));
        grid.PlaceStructureOnTheGrid(gameObject, position);

        // Assert
        Assert.IsTrue(grid.IsCellTaken(position));

    }

    [Test]
    public void PlaceStructureMinAndCheckIsTakenPass()
    {
        // Setup
        Vector3 position = new(0, 0, 0);
        GameObject gameObject = new("gameObject");
        gameObject.AddComponent<MeshRenderer>();
        gameObject.GetComponent<MeshRenderer>().bounds = new Bounds(Vector3.zero, new(4, 4, 4));
        grid.PlaceStructureOnTheGrid(gameObject, position);

        // Assert
        Assert.IsTrue(grid.IsCellTaken(position));
    }

    [Test]
    public void PlaceStructureMaxAndCheckIsTakenPass()
    {
        // Setup
        Vector3 position = new(99, 0, 99);
        GameObject gameObject = new("gameObject");
        gameObject.AddComponent<MeshRenderer>();
        gameObject.GetComponent<MeshRenderer>().bounds = new Bounds(Vector3.zero, new(4, 4, 4));
        grid.PlaceStructureOnTheGrid(gameObject, position);

        // Assert
        Assert.IsTrue(grid.IsCellTaken(position));
    }

    [Test]
    public void PlaceStructure101NullObjectFail()
    {
        // Setup
        Vector3 position = new(1, 0, 1);
        GameObject nullGameObject = null;

        // Assert
        Assert.Throws<NullReferenceException>(() => grid.PlaceStructureOnTheGrid(nullGameObject, position));
    }

    [Test]
    public void PlaceStructureAndCheckIsTakenIndexOutOfBoundsFail()
    {
        // Setup
        Vector3 position = new(100, 0, 100);

        // Assert
        Assert.Throws<IndexOutOfRangeException>(() => grid.IsCellTaken(position));
    }

    [Test]
    public void PlaceStructureAndCheckIsTakenNegativeIndexOutOfBoundsFail()
    {
        // Setup
        Vector3 position = new(-1, 0, -1);

        // Assert
        Assert.Throws<IndexOutOfRangeException>(() => grid.IsCellTaken(position));
    }

    [Test]
    public void PlaceStructureAndCheckIsTakenMaxXTilePass()
    {
        // Setup
        Vector3 position = new(2, 0, 2);
        GameObject multiTileGameObject = new("multiTileGameObject");
        multiTileGameObject.transform.position = position;
        multiTileGameObject.AddComponent<MeshRenderer>();
        multiTileGameObject.GetComponent<MeshRenderer>().bounds = new Bounds(Vector3.zero, new(4, 4, 4));
        grid.PlaceStructureOnTheGrid(multiTileGameObject, position);

        // Assert
        Assert.IsTrue(grid.IsCellTaken(new(4, position.y, position.z)));
    }

    [Test]
    public void PlaceStructureAndCheckIsTakenMaxZTilePass()
    {
        // Setup
        Vector3 position = new(2, 0, 2);
        GameObject multiTileGameObject = new("multiTileGameObject");
        multiTileGameObject.transform.position = position;
        multiTileGameObject.AddComponent<MeshRenderer>();
        multiTileGameObject.GetComponent<MeshRenderer>().bounds = new Bounds(Vector3.zero, new(4, 4, 4));
        grid.PlaceStructureOnTheGrid(multiTileGameObject, position);

        // Assert
        Assert.IsTrue(grid.IsCellTaken(new(position.x, position.y, 4)));
    }

    [Test]
    public void PlaceStructureAndCheckIsTakenMinXTilePass()
    {
        // Setup
        Vector3 position = new(2, 0, 2);
        GameObject multiTileGameObject = new("multiTileGameObject");
        multiTileGameObject.transform.position = position;
        multiTileGameObject.AddComponent<MeshRenderer>();
        multiTileGameObject.GetComponent<MeshRenderer>().bounds = new Bounds(Vector3.zero, new(4, 4, 4));
        grid.PlaceStructureOnTheGrid(multiTileGameObject, position);

        // Assert
        Assert.IsTrue(grid.IsCellTaken(new(0, position.y, position.z)));
    }

    [Test]
    public void PlaceStructureAndCheckIsTakenMinZTilePass()
    {
        // Setup
        Vector3 position = new(2, 0, 2);
        GameObject multiTileGameObject = new("multiTileGameObject");
        multiTileGameObject.transform.position = position;
        multiTileGameObject.AddComponent<MeshRenderer>();
        multiTileGameObject.GetComponent<MeshRenderer>().bounds = new Bounds(Vector3.zero, new(4, 4, 4));
        grid.PlaceStructureOnTheGrid(multiTileGameObject, position);

        // Assert
        Assert.IsTrue(grid.IsCellTaken(new(position.x, position.y, 0)));
    }

    [Test]
    public void PlaceStructureAndCheckIsTakenOutOfRangeXTileFail()
    {
        // Setup
        Vector3 position = new(2, 0, 2);
        GameObject multiTileGameObject = new("multiTileGameObject");
        multiTileGameObject.transform.position = position;
        multiTileGameObject.AddComponent<MeshRenderer>();
        multiTileGameObject.GetComponent<MeshRenderer>().bounds = new Bounds(Vector3.zero, new(4, 4, 4));
        grid.PlaceStructureOnTheGrid(multiTileGameObject, position);

        // Assert
        Assert.IsFalse(grid.IsCellTaken(new(5, position.y, position.z)));
    }

    [Test]
    public void PlaceStructureAndCheckIsTakenOutOfRangeZTileFail()
    {
        // Setup
        Vector3 position = new(2, 0, 2);
        GameObject multiTileGameObject = new("multiTileGameObject");
        multiTileGameObject.transform.position = position;
        multiTileGameObject.AddComponent<MeshRenderer>();
        multiTileGameObject.GetComponent<MeshRenderer>().bounds = new Bounds(Vector3.zero, new(4, 4, 4));
        grid.PlaceStructureOnTheGrid(multiTileGameObject, position);

        // Assert
        Assert.IsFalse(grid.IsCellTaken(new(position.x, position.y, 5)));
    }
    #endregion

    #region CheckIfStructureFitsTests

    [Test]
    public void CheckIfStructureFitsPass()
    {
        // Setup
        Vector3 position = new(2, 0, 2);
        GameObject testGameObject = new("testGameObject");
        testGameObject.transform.position = position;
        testGameObject.AddComponent<MeshRenderer>();
        testGameObject.GetComponent<MeshRenderer>().bounds = new Bounds(Vector3.zero, new(4, 4, 4));

        // Assert
        Assert.IsTrue(grid.CheckIfStructureFits(testGameObject, position)); 
    }

    [Test]
    public void CheckIfStructureFitsMinPass()
    {
        // Setup
        Vector3 position = new(2, 0, 2);
        GameObject testGameObject = new("testGameObject");
        testGameObject.transform.position = position;
        testGameObject.AddComponent<MeshRenderer>();
        testGameObject.GetComponent<MeshRenderer>().bounds = new Bounds(Vector3.zero, new(4, 4, 4));

        // Assert
        Assert.IsTrue(grid.CheckIfStructureFits(testGameObject, position));
    }

    [Test]
    public void CheckIfStructureFitsMaxPass()
    {
        // Setup
        Vector3 position = new(97, 0, 97);
        GameObject testGameObject = new("testGameObject");
        testGameObject.transform.position = position;
        testGameObject.AddComponent<MeshRenderer>();
        testGameObject.GetComponent<MeshRenderer>().bounds = new Bounds(Vector3.zero, new(4, 4, 4));

        // Assert
        Assert.IsTrue(grid.CheckIfStructureFits(testGameObject, position));
    }

    [Test]
    public void CheckIfStructureFitsUnderMinFail()
    {
        // Setup
        Vector3 position = new(1, 0, 1);
        GameObject testGameObject = new("testGameObject");
        testGameObject.transform.position = position;
        testGameObject.AddComponent<MeshRenderer>();
        testGameObject.GetComponent<MeshRenderer>().bounds = new Bounds(Vector3.zero, new(4, 4, 4));

        // Assert
        Assert.IsFalse(grid.CheckIfStructureFits(testGameObject, position));
    }

    [Test]
    public void CheckIfStructureFitsOverMaxFail()
    {
        // Setup
        Vector3 position = new(98, 0, 98);
        GameObject testGameObject = new("testGameObject");
        testGameObject.transform.position = position;
        testGameObject.AddComponent<MeshRenderer>();
        testGameObject.GetComponent<MeshRenderer>().bounds = new Bounds(Vector3.zero, new(4, 4, 4));

        // Assert
        Assert.IsFalse(grid.CheckIfStructureFits(testGameObject, position));
    }
    #endregion

    #region CheckIfStructureExistsTests

    [Test]
    public void CheckIfStructureExistsPass()
    {
        // Setup
        Vector3 position = new(50, 0, 50);
        GameObject testGameObject = new("testGameObject");
        testGameObject.transform.position = position;
        testGameObject.AddComponent<MeshRenderer>();
        testGameObject.GetComponent<MeshRenderer>().bounds = new Bounds(Vector3.zero, new(4, 4, 4));
        grid.PlaceStructureOnTheGrid(testGameObject, position);

        // Assert
        Assert.IsTrue(grid.CheckIfStructureExists(testGameObject, position));
    }

    [Test]
    public void CheckIfStructureExistsMaxPass()
    {
        // Setup
        Vector3 position = new(50, 0, 50);
        GameObject testGameObject = new("testGameObject");
        testGameObject.transform.position = position;
        testGameObject.AddComponent<MeshRenderer>();
        testGameObject.GetComponent<MeshRenderer>().bounds = new Bounds(Vector3.zero, new(4, 4, 4));
        grid.PlaceStructureOnTheGrid(testGameObject, position);
        testGameObject.transform.position = new(54, 0, 54);

        // Assert
        Assert.IsTrue(grid.CheckIfStructureExists(testGameObject, new(54, 0, 54)));
    }

    [Test]
    public void CheckIfStructureExistsMinPass()
    {
        // Setup
        Vector3 position = new(50, 0, 50);
        GameObject testGameObject = new("testGameObject");
        testGameObject.transform.position = position;
        testGameObject.AddComponent<MeshRenderer>();
        testGameObject.GetComponent<MeshRenderer>().bounds = new Bounds(Vector3.zero, new(6, 6, 6));
        grid.PlaceStructureOnTheGrid(testGameObject, position);
        testGameObject.transform.position = new(46, 0, 46);

        // Assert
        Assert.IsTrue(grid.CheckIfStructureExists(testGameObject, new(46, 0, 46)));
    }

    [Test]
    public void CheckIfStructureExistsOverMaxFail()
    {
        // Setup
        Vector3 position = new(50, 0, 50);
        GameObject testGameObject = new("testGameObject");
        testGameObject.transform.position = position;
        testGameObject.AddComponent<MeshRenderer>();
        testGameObject.GetComponent<MeshRenderer>().bounds = new Bounds(Vector3.zero, new(4, 4, 4));
        grid.PlaceStructureOnTheGrid(testGameObject, position);
        testGameObject.transform.position = new(55, 0, 55);

        // Assert
        Assert.IsFalse(grid.CheckIfStructureExists(testGameObject, new(55, 0, 55)));
    }

    [Test]
    public void CheckIfStructureExistsUnderMinFail()
    {
        // Setup
        Vector3 position = new(50, 0, 50);
        GameObject testGameObject = new("testGameObject");
        testGameObject.transform.position = position;
        testGameObject.AddComponent<MeshRenderer>();
        testGameObject.GetComponent<MeshRenderer>().bounds = new Bounds(Vector3.zero, new(6, 6, 6));
        grid.PlaceStructureOnTheGrid(testGameObject, position);
        testGameObject.transform.position = new(43, 0, 43);

        // Assert
        Assert.IsFalse(grid.CheckIfStructureExists(testGameObject, new(43, 0, 43))); 
    }


    #endregion
}
