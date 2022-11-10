using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalData;

public class GridStructure
{
    private int cellSize;
    private int width, length;
    private Vector3 strPosition, strSize;
    private Cell[,] grid;

    public GridStructure(int cellSize, int width, int length)
    {
        this.cellSize = cellSize;
        this.width = width;
        this.length = length;
        grid = new Cell[this.width, this.length];

        // Create a cell for every spot on the map
        for (int row = 0; row < grid.GetLength(0); row++)
        {
            for (int col = 0; col < grid.GetLength(1); col++)
            {
                grid[row, col] = new Cell();
            }
        }
    }

    public Vector3 CalculateGridPosition(Vector3 inputPosition)
    {
        // Divide and multiple by cellSize incase cellSize is bigger than 1. 
        int x = Mathf.FloorToInt((float)inputPosition.x / cellSize);
        int z = Mathf.FloorToInt((float)inputPosition.z / cellSize);
        return new Vector3(x * cellSize, 0, z * cellSize);
    }

    public bool IsCellTaken(Vector3 gridPosition)
    {
        var cellIndex = CalculateGridIndex(gridPosition);
        if (CheckIndexValidity(cellIndex))
            return grid[cellIndex.y, cellIndex.x].IsTaken;

        throw new IndexOutOfRangeException("No index " + cellIndex + " in grid");
    }

    public void PlaceStructureOnTheGrid(GameObject structure, Vector3 gridPosition)
    {
        var cellIndex = CalculateGridIndex(gridPosition);
        if (CheckIndexValidity(cellIndex))
        {
            grid[cellIndex.y, cellIndex.x].SetContruction(structure);
        }

        InitialiseStructure(structure);

        foreach (Direction direction in Directions.CardinalDirections)
        {
            // Move index in one of the directions
            float x = gridPosition.x + direction.x;
            float z = gridPosition.z + direction.z;
            while (CheckIfIndexInRange(new(x, z), new(strPosition.x, strPosition.z), new(strSize.x, strSize.z)))
            {
                cellIndex = CalculateGridIndex(new(x, gridPosition.y, z));
                // Check if cell exists
                if (CheckIndexValidity(cellIndex))
                {
                    //Debug.Log(x + " .. " + z);
                    grid[cellIndex.y, cellIndex.x].SetContruction(structure);
                }
                // If yes, then add structure and check next cell
                x += direction.x;
                z += direction.z;
            }
        }
    }

    public bool CheckIfStructureFits(GameObject structure, Vector3 gridPosition)
    {
        InitialiseStructure(structure);

        for (int i = 0; i < 4; i++)
        {
            // Move index in one of the first 4 directions, N,E,S,W
            Direction direction = Directions.CardinalDirections[i];
            float x = gridPosition.x + direction.x;
            float z = gridPosition.z + direction.z;
            while (CheckIfIndexInRange(new(x, z), new(strPosition.x, strPosition.z), new(strSize.x, strSize.z)))
            {
                var cellIndex = CalculateGridIndex(new Vector3(x, gridPosition.y, z));
                // Check if cell exists
                if (!CheckIndexValidity(cellIndex))
                {
                    return false;
                }
                // If yes, check next cell
                x += direction.x;
                z += direction.z;
            }
        }
        return true;
    }

    public bool CheckIfStructureExists(GameObject structure, Vector3 gridPosition)
    {
        InitialiseStructure(structure);

        foreach (Direction direction in Directions.CardinalDirections)
        {
            // Move index in one of the directions
            float x = gridPosition.x + direction.x;
            float z = gridPosition.z + direction.z;
            while (CheckIfIndexInRange(new(x, z), new(strPosition.x, strPosition.z), new(strSize.x, strSize.z)))
            {
                Debug.Log(x + " .. " + z);
                // Check if the cell is already taken
                if (IsCellTaken(new(x, gridPosition.y, z)))
                {
                    return true;
                }
                // If not, check next cell
                x += direction.x;
                z += direction.z;
            }
        }
        return false;
    }

    private Vector2Int CalculateGridIndex(Vector3 gridPosition)
    {
        return new Vector2Int((int)(gridPosition.x / cellSize), (int)(gridPosition.z / cellSize));
    }

    private bool CheckIndexValidity(Vector2Int cellIndex)
    {
        // Check if cellIndex is within grid bounds
        if (cellIndex.x >= 0 && cellIndex.x < grid.GetLength(1)
                && cellIndex.y >= 0 && cellIndex.y < grid.GetLength(0))
        {
            return true;
        }
        return false;
    }

    private bool CheckIfIndexInRange(Vector2 index, Vector2 strPos, Vector2 strSize)
    {
        // Check if index is withing structure bounds
        return (index.x >= strPos.x - (strSize.x / 2) && index.x <= strPos.x + (strSize.x / 2) &&
                    index.y >= strPos.y - (strSize.y / 2) && index.y <= strPos.y + (strSize.y) / 2);
    }

    private void InitialiseStructure(GameObject structure)
    {
        strSize = structure.GetComponentInChildren<MeshRenderer>().bounds.size;
        strPosition = structure.GetComponentInChildren<Transform>().position;
    }
}
