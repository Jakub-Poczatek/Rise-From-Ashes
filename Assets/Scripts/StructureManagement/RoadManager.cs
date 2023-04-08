using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RoadManager
{
    private static PlacementManager placementManager = PlacementManager.Instance;

    public static int GetRoadNeighboursStatus(Vector3 position, GridStructure gridStructure, Dictionary<Vector3Int, GameObject> structuresToBeModified)
    {
        int roadNeighboursStatus = 0;

        foreach (NeighbourDirection neighbourDirection in Enum.GetValues(typeof(NeighbourDirection)))
        {
            var neighbourPosition = gridStructure.GetNeighbourPositionNullable(position, neighbourDirection);
            if (neighbourPosition.HasValue) 
            {
                if(CheckIfNeighbourIsRoadOnGrid(gridStructure, neighbourPosition) 
                    || CheckIfNeighbourIsRoadInDictionary(neighbourPosition, structuresToBeModified)){
                    roadNeighboursStatus += (int)neighbourDirection;
                }
            }
        }
        return roadNeighboursStatus;
    }

    public static bool CheckIfNeighbourIsRoadOnGrid(GridStructure gridStructure, Vector3Int? neighbourPosition)
    {
        if (gridStructure.IsCellTaken(neighbourPosition.Value))
        {
            var neighbourStructure = gridStructure.GetStructureBaseFromGrid(neighbourPosition.Value);
            if (neighbourStructure != null && neighbourStructure.GetType() == typeof(RoadStruct))
            {
                return true;
            }
        }
        return false;
    }

    public static bool CheckIfNeighbourIsRoadInDictionary(Vector3Int? neighbourPosition, Dictionary<Vector3Int, GameObject> structuresToBeModified)
    {
        return structuresToBeModified.ContainsKey(neighbourPosition.Value);
    }

    public static RoadStructureHelper IfCornerRoadFits(int neighboursStatus, RoadStructureHelper roadToReturn, StructureBase structureBase)
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

    public static RoadStructureHelper IfFourWayFits(int neighboursStatus, RoadStructureHelper roadToReturn, StructureBase structureBase)
    {
        if (neighboursStatus == ((int)NeighbourDirection.Up | (int)NeighbourDirection.Right | (int)NeighbourDirection.Down | (int)NeighbourDirection.Left))
        {
            roadToReturn = new RoadStructureHelper(((RoadStruct)structureBase).fourWayPrefab, RotationValue.R0);
        }
        return roadToReturn;
    }

    public static RoadStructureHelper IfStraightRoadFits(int neighboursStatus, RoadStructureHelper roadToReturn, StructureBase structureBase)
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

    public static RoadStructureHelper IfThreeWayFits(int neighboursStatus, RoadStructureHelper roadToReturn, StructureBase structureBase)
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
            roadToReturn = new RoadStructureHelper(((RoadStruct)structureBase).threeWayPrefab, RotationValue.R180);
        }
        else if (neighboursStatus == ((int)NeighbourDirection.Up | (int)NeighbourDirection.Right | (int)NeighbourDirection.Left))
        {
            roadToReturn = new RoadStructureHelper(((RoadStruct)structureBase).threeWayPrefab, RotationValue.R270);
        }
        return roadToReturn;
    }

    public static Dictionary<Vector3Int, GameObject> GetRoadNeighboursForPosition(GridStructure gridStructure, Vector3Int position)
    {
        Dictionary<Vector3Int, GameObject> dictionaryToReturn = new Dictionary<Vector3Int, GameObject>();
        List<Vector3Int?> neighbourPossibleLocations = new List<Vector3Int?>();
        neighbourPossibleLocations.Add(gridStructure.GetNeighbourPositionNullable(position, NeighbourDirection.Up));
        neighbourPossibleLocations.Add(gridStructure.GetNeighbourPositionNullable(position, NeighbourDirection.Down));
        neighbourPossibleLocations.Add(gridStructure.GetNeighbourPositionNullable(position, NeighbourDirection.Right));
        neighbourPossibleLocations.Add(gridStructure.GetNeighbourPositionNullable(position, NeighbourDirection.Left));
        foreach (Vector3Int? possiblePosition in neighbourPossibleLocations)
        {
            if (possiblePosition.HasValue)
            {
                if(CheckIfNeighbourIsRoadOnGrid(gridStructure, possiblePosition.Value)
                    && !dictionaryToReturn.ContainsKey(possiblePosition.Value))
                {
                    dictionaryToReturn.Add(possiblePosition.Value, gridStructure.GetStructureFromTheGrid(possiblePosition.Value));
                }
            }
        }
        return dictionaryToReturn;
    }

    public static void modifyRoadCellsOnGrid(Dictionary<Vector3Int, GameObject> neighbours, StructureBase structureBase, 
        Dictionary<Vector3Int, GameObject> structuresToBeModified, GridStructure gridStructure)
    {
        foreach (KeyValuePair<Vector3Int, GameObject> kvp in neighbours)
        {
            if (structuresToBeModified.ContainsKey(kvp.Key)) structuresToBeModified.Remove(kvp.Key);
            MonoBehaviour.Destroy(kvp.Value);
            gridStructure.RemoveStructureFromTheGrid(kvp.Key);
            RoadStructureHelper roadStruct = RoadManager.GetCorrectRoadPrefab(kvp.Key, structureBase, structuresToBeModified, gridStructure);
            PlaceNewRoad(roadStruct, kvp.Key, kvp.Key, gridStructure, structuresToBeModified, structureBase);
            //var structure =
            //placementManager.InstantiateRoad(kvp.Key, roadStruct.RoadPrefab, roadStruct.RoadPrefabRotation);
            //placementManager.CreateGhostRoad(kvp.Key, roadStruct.RoadPrefab, gridStructure, roadStruct.RoadPrefabRotation);
            //gridStructure.PlaceStructureOnTheGrid(structure.Value.Item1, kvp.Key, structureBase);
            //structuresToBeModified.Add(kvp.Key, gridStructure.GetStructureFromTheGrid(kvp.Key));
        }
        placementManager.DisplayStructureOnMap(structuresToBeModified.Values);
        neighbours.Clear();
    }

    public static RoadStructureHelper GetCorrectRoadPrefab(Vector3 position, StructureBase structureBase, 
        Dictionary<Vector3Int, GameObject> structuresToBeModified, GridStructure gridStructure)
    {
        int neighboursStatus = RoadManager.GetRoadNeighboursStatus(position, gridStructure, structuresToBeModified);
        RoadStructureHelper roadToReturn = null;
        roadToReturn = RoadManager.IfStraightRoadFits(neighboursStatus, roadToReturn, structureBase);
        if (roadToReturn != null)
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

    public static Vector3Int PlaceNewRoad(RoadStructureHelper road, Vector3 gridPosition, Vector3Int gridPositionInt, 
        GridStructure gridStructure, Dictionary<Vector3Int, GameObject> structuresToBeModified, StructureBase structureBase)
    {

        (GameObject, Vector3, GameObject)? ghostReturn = placementManager.CreateGhostRoad(gridPosition, road.RoadPrefab, structureBase, gridStructure, road.RoadPrefabRotation);
        if (ghostReturn != null)
        {
            gridPositionInt = Vector3Int.FloorToInt(ghostReturn.Value.Item2);
            structuresToBeModified.Add(gridPositionInt, ghostReturn.Value.Item1);
            gridStructure.PlaceStructureOnTheGrid(ghostReturn.Value.Item1, ghostReturn.Value.Item2, structureBase, ghostReturn.Value.Item3);
        }

        return gridPositionInt;
    }
}
