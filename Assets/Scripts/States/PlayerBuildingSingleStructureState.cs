using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuildingSingleStructureState : PlayerState
{
    string structureName;
    BuildingManager buildingManager;

    public PlayerBuildingSingleStructureState(GameManager gameManager, BuildingManager buildingManager) : base(gameManager)
    {
        this.buildingManager = buildingManager;
    }

    public override void OnInputPointerChange(Vector3 position)
    {
        return;
    }

    public override void OnInputPointerDown(Vector3 position)
    {
        this.buildingManager.PrepareStructureForPlacement(position, structureName, StructureType.ResourceGenStructure);
    }

    public override void OnInputPointerUp()
    {
        return;
    }

    public override void OnBuildRoad(string structureName)
    {
        this.buildingManager.CancelPlacement();
        base.OnBuildRoad(structureName);
    }

    public override void OnConfirmAction()
    {
        this.buildingManager.ConfirmPlacement();
        base.OnConfirmAction();
    }
    public override void OnCancel()
    {
        this.buildingManager.CancelPlacement();
        this.gameManager.TransitionToState(this.gameManager.playerSelectionState, null);
    }

    public override void EnterState(string structureName)
    {
        this.structureName = structureName;
    }
}
