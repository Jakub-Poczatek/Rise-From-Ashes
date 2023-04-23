using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState
{
    protected GameManager gameManager;
    protected CameraMovement cameraMovement;

    public PlayerState(GameManager gameManager)
    {
        this.gameManager = gameManager;
        cameraMovement = gameManager.cameraMovement;
    }

    public virtual void OnInputPointerDown(Vector3 position)
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.click);
    }

    public virtual void OnInputPointerChange(Vector3 position)
    {
        HoverTipManager.Instance.HideTip();
    }

    public virtual void OnInputPointerUp()
    {}

    public virtual void OnInputPanChange(Vector3 position)
    {
        cameraMovement.PanCamera(position);
    }

    public virtual void OnInputPanUp()
    {
        cameraMovement.StopCameraMovement();
    }

    public virtual void OnRotate(float angle)
    {
        cameraMovement.RotateCamera(angle);
    }

    public virtual void OnCameraZoom(float zoom)
    {
        cameraMovement.ZoomCamera(zoom);
    }

    public virtual void OnCameraMove(Vector2 direction)
    {
        cameraMovement.MoveCamera(direction);
    }

    public virtual void EnterState(string structureName)
    {
        gameManager.uiController.ToggleStructureInteractionPanel(false);
    }

    public virtual void OnCancel()
    {
        BuildingManager.Instance.CancelModification();
        this.gameManager.TransitionToState(this.gameManager.selectionState, null);
    }

    public virtual void OnConfirmAction()
    {
        BuildingManager.Instance.ConfirmModification();
        this.gameManager.TransitionToState(this.gameManager.selectionState, null);
    }

    public virtual void OnBuildSingleStructure(string structureName)
    {
        BuildingManager.Instance.CancelModification();
        this.gameManager.TransitionToState(this.gameManager.buildingResGenStructureState, structureName);
    }

    public virtual void OnBuildRoad(string structureName)
    {
        BuildingManager.Instance.CancelModification();
        this.gameManager.TransitionToState(this.gameManager.buildingRoadState, structureName);
    }

    public virtual void OnBuildResidential(string structureName)
    {
        BuildingManager.Instance.CancelModification();
        this.gameManager.TransitionToState(this.gameManager.buildingResidentialState, structureName);
    }

    public virtual void OnCitizenAssign()
    {
        BuildingManager.Instance.CancelModification();
        this.gameManager.TransitionToState(this.gameManager.citizenAssignState, null);
    }

    public virtual void OnDemolish()
    {
        BuildingManager.Instance.CancelModification();
        this.gameManager.TransitionToState(this.gameManager.removeBuildingState, null);
    }
}
