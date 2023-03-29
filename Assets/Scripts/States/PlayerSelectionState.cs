using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelectionState : PlayerState
{
    public PlayerSelectionState(GameManager gameManager, BuildingManager buildingManager) 
        : base(gameManager, buildingManager) {}

    public override void OnInputPointerDown(Vector3 position)
    {
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(cameraRay.origin, cameraRay.direction, out hit, Mathf.Infinity);
        GameObject target = hit.collider.transform.parent.gameObject;

        switch (target.tag)
        {
            case "Citizen":
                this.gameManager.citizenAssignState.citizen = target;
                this.gameManager.uiController.ToggleCitizenInteractionPanel(true, target.GetComponent<Citizen>().citizenData);
                return;
            default:
                break;
        }

        StructureBase structure = buildingManager.GetStructureBaseFromPosition(position);
        if (structure != null)
            this.gameManager.uiController.DisplayStructureInfo(structure);
        else
        {
            this.gameManager.uiController.HideStructureInfo();
            this.gameManager.uiController.ToggleCitizenInteractionPanel(false);
        }
        return;
    }

    public override void EnterState(string structureName)
    {
        base.EnterState(structureName);
        gameManager.uiController.ToggleCancelConfirmPanel(false);
    }
}
