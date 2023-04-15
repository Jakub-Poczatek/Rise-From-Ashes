using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelectionState : PlayerState
{
    public PlayerSelectionState(GameManager gameManager) 
        : base(gameManager) {}

    public override void OnInputPointerDown(Vector3 position)
    {
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(cameraRay.origin, cameraRay.direction, out hit, Mathf.Infinity);
        GameObject target = hit.collider.transform.parent.gameObject;

        Debug.Log("Hit this gameObject: " + hit.collider.transform.parent.gameObject.name);

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
