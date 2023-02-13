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
        this.buildingManager.CancelModification();
        this.gameManager.TransitionToState(this.gameManager.playerSelectionState, null);
    }

    public override void OnConfirmAction()
    {
        this.buildingManager.ConfirmModification();
        base.OnConfirmAction();
    }

    public override void OnBuildRoad(string structureName)
    {
        this.buildingManager.CancelModification();
        base.OnBuildRoad(structureName);
    }

    public override void OnDemolish()
    {
        this.buildingManager.CancelModification();
        base.OnDemolish();
    }

    public override void OnBuildSingleStructure(string structureName)
    {
        this.buildingManager.CancelModification();
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

    public override void EnterState(string structureName)
    {
       // this.buildingManager.CancelModification();
        base.EnterState(structureName);
        this.buildingManager.PrepareBuildingManager(this.GetType());
    }
}
