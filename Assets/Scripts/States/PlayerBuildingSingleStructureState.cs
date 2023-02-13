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
        this.buildingManager.PrepareStructureForModification(position, structureName, StructureType.ResourceGenStructure);
    }

    public override void OnInputPointerUp()
    {
        return;
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

    public override void OnConfirmAction()
    {
        this.buildingManager.ConfirmModification();
        base.OnConfirmAction();
    }
    public override void OnCancel()
    {
        this.buildingManager.CancelModification();
        this.gameManager.TransitionToState(this.gameManager.playerSelectionState, null);
    }

    public override void EnterState(string structureName)
    {
        base.EnterState(structureName);
        this.buildingManager.PrepareBuildingManager(this.GetType());
        this.structureName = structureName;
    }
}
