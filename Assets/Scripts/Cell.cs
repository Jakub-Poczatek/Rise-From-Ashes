using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    private bool isTaken = false;
    private GameObject structureModel;

    public bool IsTaken { get => isTaken; }

    public void SetContruction(GameObject structureModel)
    {
        if (structureModel == null)
            return;
        this.structureModel = structureModel;
        this.isTaken = true;
    }
}
