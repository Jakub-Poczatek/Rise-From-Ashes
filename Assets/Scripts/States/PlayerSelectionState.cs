using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelectionState : PlayerState
{
    public PlayerSelectionState(GameManager gameManager, BuildingManager buildingManager) 
        : base(gameManager, buildingManager) {}

    public override void OnInputPointerDown(Vector3 position)
    {
        StructureBase structure = buildingManager.GetStructureBaseFromPosition(position);
        if (structure != null)
            this.gameManager.uiController.DisplayStructureInfo(structure);
        else
            this.gameManager.uiController.HideStructureInfo();
        return;
    }
}
