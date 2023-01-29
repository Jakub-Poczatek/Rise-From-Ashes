using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    private bool isTaken = false;
    private GameObject structureModel = null;
    private Cell previous = null;
    private Cell next = null;

    public bool IsTaken { get => isTaken; }
    public Cell Previous { get => previous; set => previous = value; }
    public Cell Next { get => next; set => next = value; }

    public void SetContruction(GameObject structureModel)
    {
        if (structureModel == null)
            return;
        this.structureModel = structureModel;
        this.isTaken = true;
    }

    public GameObject GetStructure()
    {
        return structureModel;
    }

    public void RemoveStructure()
    {
        structureModel = null;
        isTaken = false;
    }
}
