using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelectionState : PlayerState
{
    BuildingManager buildingManager;

    public PlayerSelectionState(GameManager gameManager, BuildingManager buildingManager) : base(gameManager)
    {
        this.buildingManager = buildingManager;
    }

    public override void OnInputPointerChange(Vector3 position)
    {
        return;
    }

    public override void OnInputPointerDown(Vector3 position)
    {
        StructureBase structure = buildingManager.GetStructureBaseFromPosition(position);
        if (structure != null)
            this.gameManager.uiController.DisplayStructureInfo(structure);
        else
            this.gameManager.uiController.HideStructureInfo();
        return;
    }

    public override void OnInputPointerUp()
    {
        return;
    }

    public override void OnCancel()
    {
        return;
    }
}
