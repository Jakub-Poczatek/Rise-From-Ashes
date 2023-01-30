using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadPlacementModificationHelper : StructureModificationHelper
{
    StructureBase structureBase;

    public RoadPlacementModificationHelper(StructureRepository structureRepository, GridStructure gridStructure, IPlacementManager placementManager, 
        ResourceManager resourceManager) : base(structureRepository, gridStructure, placementManager, resourceManager)
    {

    }

    public override void PrepareStructureForModification(Vector3 position, string structureName = "", StructureType structureType = StructureType.None, StructureBase structureBase = null)
    {
        this.structureBase = structureBase;
        Vector3 gridPosition = gridStructure.CalculateGridPosition(position);
        if (gridStructure.IsCellTaken(gridPosition))
        {
            Vector3Int gridPositionInt = Vector3Int.FloorToInt(gridPosition);
            RoadStructureHelper road = GetCorrectRoadPrefab(gridPosition);
            if (structuresToBeModified.ContainsKey(gridPositionInt))
            {
                
            } else
            {

            }
        }
    }

    private RoadStructureHelper GetCorrectRoadPrefab(Vector3 position)
    {
        int neighboursStatus = RoadManager.GetRoadNeighboursStatus(position, gridStructure);
        RoadStructureHelper roadToReturn = null;
        roadToReturn = RoadManager.IfStraightRoadFits(neighboursStatus, roadToReturn, structureBase);
        if(roadToReturn != null)
        {
            return roadToReturn;
        }
        roadToReturn = RoadManager.IfCornerRoadFits(neighboursStatus, roadToReturn, structureBase);
        if (roadToReturn != null)
        {
            return roadToReturn;
        }
        roadToReturn = RoadManager.IfThreeWayFits(neighboursStatus, roadToReturn, structureBase);
        if (roadToReturn != null)
        {
            return roadToReturn;
        }
        roadToReturn = RoadManager.IfFourWayFits(neighboursStatus, roadToReturn, structureBase);
        return roadToReturn;
    }

    
}
