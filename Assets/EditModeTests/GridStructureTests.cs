using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class GridStructureTests
{
    GridStructure grid;

    [OneTimeSetUp]
    public void Init()
    {
        grid = new(1, 100, 100);
    }

    #region CalculateGridPositionTests
    [Test]
    public void CalculateGridPositionPasses()
    {
        // Arrange
        Vector3 position = new(0, 0, 0);

        // Act
        Vector3 returnPosition = grid.CalculateGridPosition(position);

        // Assert
        Assert.AreEqual(Vector3.zero, returnPosition);
    }

    [Test]
    public void CalculateGridPositionFloatsPasses()
    {
        // Arrange
        Vector3 position = new(0.9f, 0, 0.1f);

        // Act
        Vector3 returnPosition = grid.CalculateGridPosition(position);

        // Assert
        Assert.AreEqual(Vector3.zero, returnPosition);
    }

    [Test]
    public void CalculateGridPositionFail()
    {
        // Arrange
        Vector3 position = new(1.1f, 0, 0);

        // Act
        Vector3 returnPosition = grid.CalculateGridPosition(position);

        // Assert
        Assert.AreNotEqual(Vector3.zero, returnPosition);
    }
    #endregion

    #region IsCellTakenTests

    [Test]
    public void PlaceStructure101AndCheckIsTakenPass()
    {
        // Arrage
        Vector3 position = new Vector3(1, 0, 1);

        // Act
        GameObject testGameObject = new GameObject("TestGameObject");
        grid.PlaceStructureOnTheGrid(testGameObject, position);

        // Assert
        Assert.IsTrue(grid.IsCellTaken(position));

    }

    [Test]
    public void PlaceStructureMinAndCheckIsTakenPass()
    {
        // Arrange
        Vector3 position = new Vector3(0, 0, 0);

        // Act
        GameObject testGameObject = new GameObject("TestGameObject");
        grid.PlaceStructureOnTheGrid(testGameObject, position);

        // Assert
        Assert.IsTrue(grid.IsCellTaken(position));
    }

    [Test]
    public void PlaceStructureMaxAndCheckIsTakenPass()
    {
        // Arrange
        Vector3 position = new Vector3(99, 0, 99);

        // Act
        GameObject testGameObject = new GameObject("TestGameObject");
        grid.PlaceStructureOnTheGrid(testGameObject, position);

        // Assert
        Assert.IsTrue(grid.IsCellTaken(position));
    }

    [Test]
    public void PlaceStructure101AndCheckIsTakenNullObjectFail()
    {
        // Arrange
        Vector3 position = new Vector3(1, 0, 1);

        // Act
        GameObject testGameObject = null;
        grid.PlaceStructureOnTheGrid(testGameObject, position);

        // Assert
        Assert.IsFalse(grid.IsCellTaken(position));
    }

    [Test]
    public void PlaceStructureAndCheckIsTakenIndexOutOfBoundsFail()
    {
        // Arrange
        Vector3 position = new Vector3(100, 0, 100);

        // Assert
        Assert.Throws<IndexOutOfRangeException>(() => grid.IsCellTaken(position));
    }

    [Test]
    public void PlaceStructureAndCheckIsTakenNegativeIndexOutOfBoundsFail()
    {
        // Arrange
        Vector3 position = new Vector3(-1, 0, -1);

        // Assert
        Assert.Throws<IndexOutOfRangeException>(() => grid.IsCellTaken(position));
    }

    #endregion
}
