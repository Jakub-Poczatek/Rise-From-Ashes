using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuildingResidentialState : PlayerState
{
    string structureName;

    public PlayerBuildingResidentialState(GameManager gameManager) : base(gameManager) { }

    public override void EnterState(string structureName)
    {
        base.EnterState(structureName);
        BuildingManager.Instance.PrepareBuildingManager(this.GetType());
        this.structureName = structureName;
        //this.buildingManager.PreviewStructure(structureName, StructureType.ResidentialStructure);
    }

    public override void OnInputPointerDown(Vector3 position)
    {
        Time.timeScale = 0;
        BuildingManager.Instance.PrepareStructureForModification(position, structureName, StructureType.ResidentialStructure);
    }
}
