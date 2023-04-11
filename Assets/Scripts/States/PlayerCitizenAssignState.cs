using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCitizenAssignState : PlayerState
{
    public GameObject citizen;
	public PlayerCitizenAssignState(GameManager gameManager) : base(gameManager) { }

    public override void EnterState(string structureName)
    {
        base.EnterState(structureName);
        Time.timeScale = 0;
    }

    public override void OnInputPointerDown(Vector3 position)
    {
        GameObject structure = BuildingManager.Instance.GetStructureFromGrid(position);
        if (structure != null)
        {
            if (structure.CompareTag("ResGenStructure"))
            {
                citizen.GetComponent<Citizen>().AssignWork(structure);
                OnConfirmAction();
            }
        }
    }
}
