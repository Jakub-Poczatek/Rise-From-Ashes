using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDemolishingState : PlayerState
{
    public PlayerDemolishingState(GameManager gameManager) 
        : base(gameManager)
    { }

    public override void EnterState(string structureName)
    {
        base.EnterState(structureName);
        BuildingManager.Instance.PrepareBuildingManager(this.GetType());
    }

    public override void OnInputPointerDown(Vector3 position)
    {
        BuildingManager.Instance.PrepareStructureForDemolishing(position);
    }
}
