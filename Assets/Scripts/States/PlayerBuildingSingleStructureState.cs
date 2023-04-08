using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuildingSingleStructureState : PlayerState
{
    string structureName;

    public PlayerBuildingSingleStructureState(GameManager gameManager) 
        : base(gameManager) { }

    public override void EnterState(string structureName)
    {
        base.EnterState(structureName);
        BuildingManager.Instance.PrepareBuildingManager(this.GetType());
        this.structureName = structureName;
        //this.buildingManager.PreviewStructure(structureName, StructureType.ResourceGenStructure);
    }

    public override void OnInputPointerDown(Vector3 position)
    {
        BuildingManager.Instance.PrepareStructureForModification(position, structureName, StructureType.ResourceGenStructure);
    }
}
