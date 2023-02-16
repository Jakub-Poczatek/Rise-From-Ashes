using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState
{
    protected GameManager gameManager;
    protected BuildingManager buildingManager;
    protected CameraMovement cameraMovement;

    public PlayerState(GameManager gameManager, BuildingManager buildingManager)
    {
        this.gameManager = gameManager;
        cameraMovement = gameManager.cameraMovement;
        this.buildingManager = buildingManager;
    }

    public virtual void OnInputPointerDown(Vector3 position)
    {}

    public virtual void OnInputPointerChange(Vector3 position)
    {}

    public virtual void OnInputPointerUp()
    {}

    public virtual void OnInputPanChange(Vector3 position)
    {
        cameraMovement.MoveCamera(position);
    }

    public virtual void OnInputPanUp()
    {
        cameraMovement.StopCameraMovement();
    }

    public virtual void EnterState(string structureName)
    {
        gameManager.uiController.HideStructureInfo();
    }

    public virtual void OnCancel()
    {
        this.buildingManager.CancelModification();
        this.gameManager.TransitionToState(this.gameManager.playerSelectionState, null);
    }

    public virtual void OnConfirmAction()
    {
        this.buildingManager.ConfirmModification();
        this.gameManager.TransitionToState(this.gameManager.playerSelectionState, null);
    }

    public virtual void OnBuildSingleStructure(string structureName)
    {
        this.buildingManager.CancelModification();
        this.gameManager.TransitionToState(this.gameManager.buildingSingleStructureState, structureName);
    }

    public virtual void OnBuildRoad(string structureName)
    {
        this.buildingManager.CancelModification();
        this.gameManager.TransitionToState(this.gameManager.buildingRoadState, structureName);
    }

    public virtual void OnDemolish()
    {
        this.buildingManager.CancelModification();
        this.gameManager.TransitionToState(this.gameManager.playerRemoveBuildingState, null);
    }
}
