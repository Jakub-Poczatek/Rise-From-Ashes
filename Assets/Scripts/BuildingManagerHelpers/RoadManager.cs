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

    internal static RoadStructureHelper IfCornerRoadFirst(int neighboursStatus, RoadStructureHelper roadToReturn)
    {
        throw new NotImplementedException();
    }

    internal static RoadStructureHelper IfFourWayFits(int neighboursStatus, RoadStructureHelper roadToReturn)
    {
        throw new NotImplementedException();
    }

    internal static RoadStructureHelper IfStraightRoadFits(int neighboursStatus, RoadStructureHelper roadToReturn)
    {
        throw new NotImplementedException();
    }

    internal static RoadStructureHelper IfThreeWayFits(int neighboursStatus, RoadStructureHelper roadToReturn)
    {
        throw new NotImplementedException();
    }
}
