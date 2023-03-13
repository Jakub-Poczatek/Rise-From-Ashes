using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelectionState : PlayerState
{
    public PlayerSelectionState(GameManager gameManager, BuildingManager buildingManager) 
        : base(gameManager, buildingManager) {}

    public override void OnInputPointerDown(Vector3 position)
    {
        RaycastHit hit;
        Physics.Raycast(position + (Vector3.up * 5), Vector3.down, out hit, 10);
        GameObject target = hit.collider.transform.parent.gameObject;

        switch (target.tag)
        {
            case "Citizen":
                this.gameManager.citizenAssignState.citizen = target;
                this.gameManager.uiController.ToggleCitizenInteraction(true, target.GetComponent<Citizen>().citizenData);
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
            this.gameManager.uiController.ToggleCitizenInteraction(false);
        }
        return;
    }
}
