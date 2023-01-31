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

    public override void PrepareStructureForModification(Vector3 position, string structureName = "", StructureType structureType = StructureType.None)
    {
        structureBase = structureRepository.GetStructureByName(structureName, structureType);
        Vector3 gridPosition = gridStructure.CalculateGridPosition(position);
        Vector3Int gridPositionInt = Vector3Int.FloorToInt(gridPosition);

        if (structuresToBeModified.ContainsKey(gridPositionInt))
        {
            GameObject structure = structuresToBeModified[gridPositionInt];
            MonoBehaviour.Destroy(structure);
            gridStructure.RemoveStructureFromTheGrid(gridPosition);
            structuresToBeModified.Remove(gridPositionInt);
        }
        else if (!gridStructure.IsCellTaken(gridPosition))
        {
            //Debug.Log("Cell is not taken");
            RoadStructureHelper road = GetCorrectRoadPrefab(gridPosition);
            //Debug.Log("This is the road: " + road.ToString());
            (GameObject, Vector3, GameObject)? ghostReturn = placementManager.CreateGhostRoad(gridPosition, road.RoadPrefab, gridStructure, road.RoadPrefabRotation);
            //Debug.Log("This is the ghost return: " + ghostReturn.ToString());
            if (ghostReturn != null)
            {
                //Debug.Log("Ghost return is not null");
                gridPositionInt = Vector3Int.FloorToInt(ghostReturn.Value.Item2);
                structuresToBeModified.Add(gridPositionInt, ghostReturn.Value.Item1);
                gridStructure.PlaceStructureOnTheGrid(ghostReturn.Value.Item1, ghostReturn.Value.Item2, structureBase, ghostReturn.Value.Item3);
            }
        }
    }

    private RoadStructureHelper GetCorrectRoadPrefab(Vector3 position)
    {
        int neighboursStatus = RoadManager.GetRoadNeighboursStatus(position, gridStructure, structuresToBeModified);
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
