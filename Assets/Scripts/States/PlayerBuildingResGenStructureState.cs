using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuildingResGenStructureState : PlayerState
{
    string structureName;
    public GameObject structureModel = null;

    public PlayerBuildingResGenStructureState(GameManager gameManager) 
        : base(gameManager) { }

    public override void EnterState(string structureName)
    {
        base.EnterState(structureName);
        structureModel = null;
        BuildingManager.Instance.PrepareBuildingManager(this.GetType());
        this.structureName = structureName;
        //BuildingManager.Instance.PreviewStructure(structureName, StructureType.ResourceGenStructure);
    }

    public override void OnRotate(float angle)
    {
        if (structureModel != null)
        {
            structureModel.transform.Rotate(0, angle, 0);
        }
    }

    public override void OnInputPointerDown(Vector3 position)
    {
        base.OnInputPointerDown(position);
        BuildingManager.Instance.PrepareStructureForModification(position, structureName, StructureType.ResourceGenStructure);
    }
}
