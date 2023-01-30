using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RoadManager
{
    public static int GetRoadNeighboursStatus(Vector3 position, GridStructure gridStructure)
    {
        int roadNeighboursStatus = 0;

        foreach (NeighbourDirection neighbourDirection in Enum.GetValues(typeof(NeighbourDirection)))
        {
            var neighbourPosition = gridStructure.GetNeighbourPositionNullable(position, neighbourDirection);
            if (neighbourPosition.HasValue && gridStructure.IsCellTaken(neighbourPosition.Value))
            {
                var neighbourStructureBase = gridStructure.GetStructureDataFromGrid(neighbourPosition.Value);
                if (neighbourStructureBase != null)
                {
                    roadNeighboursStatus += (int)neighbourDirection;
                }
            }
        }
        return roadNeighboursStatus;
    }

    internal static RoadStructureHelper IfCornerRoadFits(int neighboursStatus, RoadStructureHelper roadToReturn, StructureBase structureBase)
    {
        if (neighboursStatus == ((int)NeighbourDirection.Up | (int)NeighbourDirection.Right))
        {
            roadToReturn = new RoadStructureHelper(((RoadStruct)structureBase).cornerPrefab, RotationValue.R0);
        } 
        else if(neighboursStatus == ((int)NeighbourDirection.Down | (int)NeighbourDirection.Right))
        {
            roadToReturn = new RoadStructureHelper(((RoadStruct)structureBase).cornerPrefab, RotationValue.R90);
        }
        else if (neighboursStatus == ((int)NeighbourDirection.Down | (int)NeighbourDirection.Left))
        {
            roadToReturn = new RoadStructureHelper(((RoadStruct)structureBase).cornerPrefab, RotationValue.R180);
        }
        else if (neighboursStatus == ((int)NeighbourDirection.Up | (int)NeighbourDirection.Left))
        {
            roadToReturn = new RoadStructureHelper(((RoadStruct)structureBase).cornerPrefab, RotationValue.R270);
        }
        return roadToReturn;
    }

    internal static RoadStructureHelper IfFourWayFits(int neighboursStatus, RoadStructureHelper roadToReturn, StructureBase structureBase)
    {
        if (neighboursStatus == ((int)NeighbourDirection.Up | (int)NeighbourDirection.Right | (int)NeighbourDirection.Down | (int)NeighbourDirection.Left))
        {
            roadToReturn = new RoadStructureHelper(((RoadStruct)structureBase).fourWayPrefab, RotationValue.R0);
        }
        return roadToReturn;
    }

    internal static RoadStructureHelper IfStraightRoadFits(int neighboursStatus, RoadStructureHelper roadToReturn, StructureBase structureBase)
    {
        if (neighboursStatus == ((int)NeighbourDirection.Up | (int)NeighbourDirection.Down)
            || neighboursStatus == (int)NeighbourDirection.Up
            || neighboursStatus == (int)NeighbourDirection.Down)
        {
            roadToReturn = new RoadStructureHelper(((RoadStruct)structureBase).prefab, RotationValue.R90);
        }
        else if (neighboursStatus == ((int)NeighbourDirection.Right | (int)NeighbourDirection.Left)
            || neighboursStatus == (int)NeighbourDirection.Right
            || neighboursStatus == (int)NeighbourDirection.Left
            || neighboursStatus == 0)
        {
            roadToReturn = new RoadStructureHelper(((RoadStruct)structureBase).prefab, RotationValue.R0);
        }
        return roadToReturn;
    }

    internal static RoadStructureHelper IfThreeWayFits(int neighboursStatus, RoadStructureHelper roadToReturn, StructureBase structureBase)
    {
        if (neighboursStatus == ((int)NeighbourDirection.Up | (int)NeighbourDirection.Down | (int)NeighbourDirection.Right))
        {
            roadToReturn = new RoadStructureHelper(((RoadStruct)structureBase).threeWayPrefab, RotationValue.R0);
        }
        else if (neighboursStatus == ((int)NeighbourDirection.Down | (int)NeighbourDirection.Right | (int)NeighbourDirection.Left))
        {
            roadToReturn = new RoadStructureHelper(((RoadStruct)structureBase).threeWayPrefab, RotationValue.R90);
        }
        else if (neighboursStatus == ((int)NeighbourDirection.Up | (int)NeighbourDirection.Down | (int)NeighbourDirection.Left))
        {
            roadToReturn = new RoadStructureHelper(((RoadStruct)structureBase).cornerPrefab, RotationValue.R180);
        }
        else if (neighboursStatus == ((int)NeighbourDirection.Up | (int)NeighbourDirection.Right | (int)NeighbourDirection.Left))
        {
            roadToReturn = new RoadStructureHelper(((RoadStruct)structureBase).cornerPrefab, RotationValue.R270);
        }
        return roadToReturn;
    }
}
