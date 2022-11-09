using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class CellTests
{
    Cell cell;

    [OneTimeSetUp]
    public void Init()
    {
        cell = new();
    }

    #region SetContructionTests

    [Test]
    public void CellSetGameObjectPass()
    {
        // Act
        cell.SetContruction(new GameObject());

        // Assert
        Assert.IsTrue(cell.IsTaken);
    }

    [Test]
    public void CellSetGameObjectNullFail()
    {
        // Act
        cell.SetContruction(null);

        // Assert
        Assert.IsFalse(cell.IsTaken);
    }

    #endregion
}
