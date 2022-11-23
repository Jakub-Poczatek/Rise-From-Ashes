using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuildingSingleStructureState : PlayerState
{
    PlacementManager placementManager;
    GridStructure gridStructure;

    public PlayerBuildingSingleStructureState(GameManager gameManager, PlacementManager placementManager,
            GridStructure gridStructure) : base(gameManager)
    {
        this.placementManager = placementManager;
        this.gridStructure = gridStructure;
    }

    public override void OnInputPanChange(Vector3 position)
    {
        return;
    }

    public override void OnInputPanUp()
    {
        return;
    }

    public override void OnInputPointerChange(Vector3 position)
    {
        return;
    }

    public override void OnInputPointerDown(Vector3 position)
    {
        Vector3 gridPosition = gridStructure.CalculateGridPosition(position);
        if (gridStructure.IsCellTaken(gridPosition) == false)
        {
            placementManager.CreateBuilding(gridPosition, gridStructure);
        }
    }

    public override void OnInputPointerUp()
    {
        return;
    }

    public override void OnCancel()
    {
        this.gameManager.TransitionToState(this.gameManager.playerSelectionState);
    }
}
