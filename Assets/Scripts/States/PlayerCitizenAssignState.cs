using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCitizenAssignState : PlayerState
{
    public GameObject citizen;
	public PlayerCitizenAssignState(GameManager gameManager, BuildingManager buildingManager) 
		: base(gameManager, buildingManager) { }

    public override void OnInputPointerDown(Vector3 position)
    {
        GameObject structure = buildingManager.GetStructureFromGrid(position);
        if (structure != null)
        {
            if (structure.CompareTag("Structure"))
            {
                citizen.GetComponent<Citizen>().AssignWork(structure);
            }
            gameManager.uiController.ToggleCitizenInteraction(false);
            OnConfirmAction();
        }
    }
}
