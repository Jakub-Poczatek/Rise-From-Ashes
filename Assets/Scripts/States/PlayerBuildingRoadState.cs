using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuildingRoadState : PlayerState
{
    string structureName;

    public PlayerBuildingRoadState(GameManager gameManager) : base(gameManager) {}

    public override void EnterState(string structureName)
    {
        base.EnterState(structureName);
        BuildingManager.Instance.PrepareBuildingManager(this.GetType());
        this.structureName = structureName;
    }

    public override void OnInputPointerDown(Vector3 position)
    {
        BuildingManager.Instance.PrepareStructureForModification(position, structureName, StructureType.RoadStructure);
    }
}
