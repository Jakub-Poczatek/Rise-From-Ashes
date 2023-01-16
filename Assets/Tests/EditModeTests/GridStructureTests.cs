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
    public void CalculateGridPositionMaxPass()
    {
        // Setup
        Vector3 position = new(0.9f, 0, 0.9f);
        Vector3 returnPosition = grid.CalculateGridPosition(position);

        // Assert
        Assert.AreEqual(Vector3.zero, returnPosition);
    }

    [Test]
    public void CalculateGridPositionFail()
    {
        // Setup
        Vector3 position = new(1.1f, 0, 1.1f);
        Vector3 returnPosition = grid.CalculateGridPosition(position);

        // Assert
        Assert.AreNotEqual(Vector3.zero, returnPosition);
    }
    #endregion

    #region IsCellTakenTests

    [Test]
    public void CheckIsTakenOverMaxFail()
    {
        // Setup
        Vector3 position = new(100, 0, 100);

        // Assert
        Assert.Throws<IndexOutOfRangeException>(() => grid.IsCellTaken(position));
    }

    [Test]
    public void CheckIsTakenUnderMinFail()
    {
        // Setup
        Vector3 position = new(-1, 0, -1);

        // Assert
        Assert.Throws<IndexOutOfRangeException>(() => grid.IsCellTaken(position));
    }

    #endregion

    #region PlaceStructureTests

    [Test]
    public void PlaceStructure101AndCheckIsTakenPass()
    {
        // Setup
        Vector3 position = new(50, 0, 50);
        GameObject gameObject = new("gameObject", typeof(MeshRenderer));
        gameObject.GetComponent<MeshRenderer>().bounds = new Bounds(Vector3.zero, new(1, 0, 1));
        gameObject.transform.position = position;
        grid.PlaceStructureOnTheGrid(gameObject, position);

        // Assert
        Assert.IsTrue(grid.IsCellTaken(position));

    }

    [Test]
    public void PlaceStructureMinAndCheckIsTakenPass()
    {
        // Setup
        Vector3 position = new(0, 0, 0);
        GameObject gameObject = new("gameObject", typeof(MeshRenderer));
        gameObject.GetComponent<MeshRenderer>().bounds = new Bounds(Vector3.zero, new(1, 0, 1));
        gameObject.transform.position = position;
        grid.PlaceStructureOnTheGrid(gameObject, position);

        // Assert
        Assert.IsTrue(grid.IsCellTaken(position));
    }

    [Test]
    public void PlaceStructureMaxAndCheckIsTakenPass()
    {
        // Setup
        Vector3 position = new(99, 0, 99);
        GameObject gameObject = new("gameObject", typeof(MeshRenderer));
        gameObject.GetComponent<MeshRenderer>().bounds = new Bounds(Vector3.zero, new(1, 0, 1));
        gameObject.transform.position = position;
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
    public void PlaceStructureAndCheckIsTakenMaxPass()
    {
        // Setup
        Vector3 position = new(50, 0, 50);
        GameObject gameObject = new("gameObject", typeof(MeshRenderer));
        gameObject.GetComponent<MeshRenderer>().bounds = new Bounds(Vector3.zero, new(3, 0, 3));
        gameObject.transform.position = position;
        grid.PlaceStructureOnTheGrid(gameObject, position);

        // Assert
        Assert.IsTrue(grid.IsCellTaken(position + new Vector3(2, 0, 2)));
    }

    [Test]
    public void PlaceStructureAndCheckIsTakenMinPass()
    {
        // Setup
        Vector3 position = new(50, 0, 50);
        GameObject gameObject = new("gameObject", typeof(MeshRenderer));
        gameObject.GetComponent<MeshRenderer>().bounds = new Bounds(Vector3.zero, new(3, 0, 3));
        gameObject.transform.position = position;
        grid.PlaceStructureOnTheGrid(gameObject, position);

        // Assert
        Assert.IsTrue(grid.IsCellTaken(position - new Vector3(2, 0, 2)));
    }

    [Test]
    public void PlaceStructureAndCheckIsTakenOverMaxFail()
    {
        // Setup
        Vector3 position = new(50, 0, 50);
        GameObject gameObject = new("gameObject", typeof(MeshRenderer));
        gameObject.GetComponent<MeshRenderer>().bounds = new Bounds(Vector3.zero, new(3, 0, 3));
        gameObject.transform.position = position;
        grid.PlaceStructureOnTheGrid(gameObject, position);

        // Assert
        Assert.IsFalse(grid.IsCellTaken(position + new Vector3(3, 0, 3)));
    }

    [Test]
    public void PlaceStructureAndCheckIsTakenUnderMinFail()
    {
        // Setup
        Vector3 position = new(50, 0, 50);
        GameObject gameObject = new("gameObject", typeof(MeshRenderer));
        gameObject.GetComponent<MeshRenderer>().bounds = new Bounds(Vector3.zero, new(3, 0, 3));
        gameObject.transform.position = position;
        grid.PlaceStructureOnTheGrid(gameObject, position);

        // Assert
        Assert.IsFalse(grid.IsCellTaken(position - new Vector3(3, 0, 3)));
    }
    #endregion

    #region CheckIfStructureFitsTests

    [Test]
    public void CheckIfStructureFitsPass()
    {
        // Setup
        Vector3 position = new(50, 0, 50);
        GameObject gameObject = new("gameObject", typeof(MeshRenderer));
        gameObject.GetComponent<MeshRenderer>().bounds = new Bounds(Vector3.zero, new(3, 0, 3));
        gameObject.transform.position = position;

        // Assert
        Assert.IsTrue(grid.CheckIfStructureFits(gameObject, position)); 
    }

    [Test]
    public void CheckIfStructureFitsMinPass()
    {
        // Setup
        Vector3 position = new(2, 0, 2);
        GameObject gameObject = new("gameObject", typeof(MeshRenderer));
        gameObject.GetComponent<MeshRenderer>().bounds = new Bounds(Vector3.zero, new(3, 0, 3));
        gameObject.transform.position = position;

        // Assert
        Assert.IsTrue(grid.CheckIfStructureFits(gameObject, position));
    }

    [Test]
    public void CheckIfStructureFitsMaxPass()
    {
        // Setup
        Vector3 position = new(97, 0, 97);
        GameObject gameObject = new("gameObject", typeof(MeshRenderer));
        gameObject.GetComponent<MeshRenderer>().bounds = new Bounds(Vector3.zero, new(3, 0, 3));
        gameObject.transform.position = position;

        // Assert
        Assert.IsTrue(grid.CheckIfStructureFits(gameObject, position));
    }

    [Test]
    public void CheckIfStructureFitsUnderMinFail()
    {
        // Setup
        Vector3 position = new(1, 0, 1);
        GameObject gameObject = new("gameObject", typeof(MeshRenderer));
        gameObject.GetComponent<MeshRenderer>().bounds = new Bounds(Vector3.zero, new(3, 0, 3));
        gameObject.transform.position = position;

        // Assert
        Assert.IsFalse(grid.CheckIfStructureFits(gameObject, position));
    }

    [Test]
    public void CheckIfStructureFitsOverMaxFail()
    {
        // Setup
        Vector3 position = new(98, 0, 98);
        GameObject gameObject = new("gameObject", typeof(MeshRenderer));
        gameObject.GetComponent<MeshRenderer>().bounds = new Bounds(Vector3.zero, new(3, 0, 3));
        gameObject.transform.position = position;

        // Assert
        Assert.IsFalse(grid.CheckIfStructureFits(gameObject, position));
    }
    #endregion

    #region CheckIfStructureExistsTests

    [Test]
    public void CheckIfStructureExistsPass()
    {
        // Setup
        Vector3 position = new(50, 0, 50);
        GameObject gameObject = new("gameObject", typeof(MeshRenderer));
        gameObject.GetComponent<MeshRenderer>().bounds = new Bounds(Vector3.zero, new(3, 0, 3));
        gameObject.transform.position = position;
        grid.PlaceStructureOnTheGrid(gameObject, position);

        // Assert
        Assert.IsTrue(grid.CheckIfStructureExists(gameObject, position));
    }

    [Test]
    public void CheckIfStructureExistsMaxPass()
    {
        // Setup
        Vector3 position = new(50, 0, 50);
        GameObject gameObject = new("gameObject", typeof(MeshRenderer));
        gameObject.GetComponent<MeshRenderer>().bounds = new Bounds(Vector3.zero, new(3, 0, 3));
        gameObject.transform.position = position;
        grid.PlaceStructureOnTheGrid(gameObject, position);
        position += new Vector3(4, 0, 4);
        gameObject.transform.position = position;

        // Assert
        Assert.IsTrue(grid.CheckIfStructureExists(gameObject, position));
    }

    [Test]
    public void CheckIfStructureExistsMinPass()
    {
        // Setup
        Vector3 position = new(50, 0, 50);
        GameObject gameObject = new("gameObject", typeof(MeshRenderer));
        gameObject.GetComponent<MeshRenderer>().bounds = new Bounds(Vector3.zero, new(3, 0, 3));
        gameObject.transform.position = position;
        grid.PlaceStructureOnTheGrid(gameObject, position);
        position -= new Vector3(4, 0, 4);
        gameObject.transform.position = position;

        // Assert
        Assert.IsTrue(grid.CheckIfStructureExists(gameObject, position));
    }

    [Test]
    public void CheckIfStructureExistsOverMaxFail()
    {
        // Setup
        Vector3 position = new(50, 0, 50);
        GameObject gameObject = new("gameObject", typeof(MeshRenderer));
        gameObject.GetComponent<MeshRenderer>().bounds = new Bounds(Vector3.zero, new(3, 0, 3));
        gameObject.transform.position = position;
        grid.PlaceStructureOnTheGrid(gameObject, position);
        position += new Vector3(5, 0, 5);
        gameObject.transform.position = position;

        // Assert
        Assert.IsFalse(grid.CheckIfStructureExists(gameObject, position));
    }

    [Test]
    public void CheckIfStructureExistsUnderMinFail()
    {
        // Setup
        Vector3 position = new(50, 0, 50);
        GameObject gameObject = new("gameObject", typeof(MeshRenderer));
        gameObject.GetComponent<MeshRenderer>().bounds = new Bounds(Vector3.zero, new(3, 0, 3));
        gameObject.transform.position = position;
        grid.PlaceStructureOnTheGrid(gameObject, position);
        position -= new Vector3(5, 0, 5);
        gameObject.transform.position = position;

        // Assert
        Assert.IsFalse(grid.CheckIfStructureExists(gameObject, position)); 
    }

    #endregion
}
