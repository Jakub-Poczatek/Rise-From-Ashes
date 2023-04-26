using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelectionState : PlayerState
{
    public PlayerSelectionState(GameManager gameManager) 
        : base(gameManager) {}

    public override void OnInputPointerDown(Vector3 position)
    {
        base.OnInputPointerDown(position);
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        int layerMask = ~(1 << LayerMask.NameToLayer("Terrain"));
        Physics.Raycast(cameraRay.origin, cameraRay.direction, out hit, Mathf.Infinity, layerMask);
        GameObject target = hit.collider.transform.parent.gameObject;
        switch (target.tag)
        {
            case "Citizen":
                this.gameManager.citizenAssignState.citizen = target;
                this.gameManager.uiController.ToggleCitizenInteractionPanel(true, target.GetComponent<Citizen>().citizenData);
                this.gameManager.uiController.ToggleStructureInteractionPanel(false);
                this.gameManager.uiController.ToggleChallengesPanel(false);
                this.gameManager.uiController.ToggleCitizenListPanel(false);
                return;
            case "Structure" or "ResGenStructure":
                this.gameManager.uiController.ToggleStructureInteractionPanel(true, target.GetComponent<Structure>());
                this.gameManager.uiController.ToggleCitizenInteractionPanel(false);
                this.gameManager.uiController.ToggleChallengesPanel(false);
                this.gameManager.uiController.ToggleCitizenListPanel(false);
                return;
            default:
                this.gameManager.uiController.ToggleStructureInteractionPanel(false);
                this.gameManager.uiController.ToggleCitizenInteractionPanel(false);
                this.gameManager.uiController.ToggleChallengesPanel(false);
                this.gameManager.uiController.ToggleCitizenListPanel(false);
                return;
        }
    }

    public override void EnterState(string structureName)
    {
        base.EnterState(structureName);
        gameManager.uiController.ToggleCancelConfirmPanel(false);
    }
}
