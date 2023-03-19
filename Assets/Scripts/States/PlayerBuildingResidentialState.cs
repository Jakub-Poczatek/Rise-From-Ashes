using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuildingResidentialState : PlayerState
{
    string structureName;

    public PlayerBuildingResidentialState(GameManager gameManager, BuildingManager buildingManager)
        : base(gameManager, buildingManager) { }

    public override void EnterState(string structureName)
    {
        base.EnterState(structureName);
        this.buildingManager.PrepareBuildingManager(this.GetType());
        this.structureName = structureName;
        //this.buildingManager.PreviewStructure(structureName, StructureType.ResidentialStructure);
    }

    public override void OnInputPointerDown(Vector3 position)
    {
        this.buildingManager.PrepareStructureForModification(position, structureName, StructureType.ResidentialStructure);
    }
}
