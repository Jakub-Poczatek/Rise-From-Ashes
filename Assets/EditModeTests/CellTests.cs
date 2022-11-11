using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class CellTests
{
    Cell cell;

    [SetUp]
    public void SetUp()
    {
        cell = new();
    }

    #region SetContructionTests

    [Test]
    public void SetContructionPass()
    {
        // Act
        cell.SetContruction(new GameObject());

        // Assert
        Assert.IsTrue(cell.IsTaken);
    }

    [Test]
    public void SetContructionNullFail()
    {
        // Act
        cell.SetContruction(null);

        // Assert
        Assert.IsFalse(cell.IsTaken);
    }

    #endregion
}
