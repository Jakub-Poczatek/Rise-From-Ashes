using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDemolishingState : PlayerState
{
    public PlayerDemolishingState(GameManager gameManager, BuildingManager buildingManager) 
        : base(gameManager, buildingManager)
    { }

    public override void EnterState(string structureName)
    {
        base.EnterState(structureName);
        this.buildingManager.PrepareBuildingManager(this.GetType());
    }

    public override void OnInputPointerDown(Vector3 position)
    {
        this.buildingManager.PrepareStructureForDemolishing(position);
    }
}
