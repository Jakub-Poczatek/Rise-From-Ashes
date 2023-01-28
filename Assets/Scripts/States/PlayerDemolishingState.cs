using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDemolishingState : PlayerState
{
    BuildingManager buildingManager;

    public PlayerDemolishingState(GameManager gameManager, BuildingManager buildingManager) : base(gameManager)
    {
        this.buildingManager = buildingManager;
    }

    public override void OnCancel()
    {
        this.buildingManager.CancelDemolishing();
        this.gameManager.TransitionToState(this.gameManager.playerSelectionState, null);
    }

    public override void OnConfirmAction()
    {
        this.buildingManager.ConfirmDemolishing();
        base.OnConfirmAction();
    }

    public override void OnBuildRoad(string structureName)
    {
        this.buildingManager.CancelDemolishing();
        base.OnBuildRoad(structureName);
    }

    public override void OnBuildSingleStructure(string structureName)
    {
        this.buildingManager.CancelDemolishing();
        base.OnBuildSingleStructure(structureName);
    }

    public override void OnInputPointerChange(Vector3 position)
    {
        return;
    }

    public override void OnInputPointerDown(Vector3 position)
    {
        this.buildingManager.PrepareStructureForDemolishing(position);
    }

    public override void OnInputPointerUp()
    {
        return;
    }
}
