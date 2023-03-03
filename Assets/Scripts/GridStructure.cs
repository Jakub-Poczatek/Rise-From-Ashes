using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalData;
using System.Linq;

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
            return grid[(int) cellIndex.y, (int) cellIndex.x].IsTaken;

        throw new IndexOutOfRangeException("No index " + cellIndex + " in grid");
    }

    public void PlaceStructureOnTheGrid(GameObject structure, Vector3 gridPosition, StructureBase structureBase, GameObject gridOutline = null)
    {
        Cell previousCell = null;
        var cellIndex = CalculateGridIndex(gridPosition);

        List<Vector2> takenCells = new();

        if (CheckIndexValidity(cellIndex))
        {
            //grid[(int) cellIndex.y, (int) cellIndex.x].SetContruction(structure);
            //previousCell = grid[(int)cellIndex.y, (int)cellIndex.x];

            takenCells.Add(new(cellIndex.x, cellIndex.y));
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
                    takenCells.Add(new(cellIndex.x, cellIndex.y));
                }
                // If yes, then add structure and check next cell
                x += direction.x;
                z += direction.z;
            }
        }

        Vector2 topLeft = new(takenCells.Min(c => c.x), takenCells.Max(c => c.y));
        Vector2 bottomLeft = new(takenCells.Min(c => c.x), takenCells.Min(c => c.y));
        Vector2 topRight = new(takenCells.Max(c => c.x), takenCells.Max(c => c.y));
        Vector2 bottomRight = new(takenCells.Max(c => c.x), takenCells.Min(c => c.y));

        for(float i = takenCells.Min(cI => cI.x); i <= takenCells.Max(dI => dI.x); i++)
        {
            for(float j = takenCells.Min(cJ => cJ.y); j <= takenCells.Max(dJ => dJ.y); j++)
            {
                grid[(int) j, (int) i].SetContruction(structure, structureBase);

                if(previousCell != null) previousCell.Next = grid[(int) j, (int) i];
                grid[(int) j, (int) i].Previous = previousCell;
                previousCell = grid[(int) j, (int) i];

                // Debugging
                if (gridOutline != null)
                {   
                    GameObject newGridOutline = MonoBehaviour.Instantiate(gridOutline, new(i, 0, j), Quaternion.identity);
                    newGridOutline.transform.parent = structure.transform;
                }
            }
        }
    }
    
    public StructureBase GetStructureBaseFromGrid(Vector3 gridPosition)
    {
        var cellIndex = CalculateGridIndex(gridPosition);
        return grid[(int)cellIndex.y, (int)cellIndex.x].GetStructureBase();
    }

    public GameObject GetStructureFromTheGrid(Vector3 gridPosition)
    {
        var cellIndex = CalculateGridIndex(gridPosition);
        return grid[(int) cellIndex.y, (int) cellIndex.x].GetStructure();
    }

    public void RemoveStructureFromTheGrid(Vector3 gridPosition)
    {
        var cellIndex = CalculateGridIndex(gridPosition);
        Cell cell = grid[(int)cellIndex.y, (int)cellIndex.x];
        Cell tempCell = cell;

        cell.RemoveStructure();
        if (cell.Previous != null)
        {
            do
            {
                cell = cell.Previous;

                tempCell.Previous = null;
                tempCell = cell;
                tempCell.Next = null;

                cell.RemoveStructure();
            } while (cell.Previous != null);
        }

        cell = grid[(int)cellIndex.y, (int)cellIndex.x];
        tempCell = cell;

        if (cell.Next != null)
        {
            do
            {
                cell = cell.Next;

                tempCell.Next = null;
                tempCell.Previous = null;
                tempCell = cell;

                cell.RemoveStructure();
            } while (cell.Next != null);
        }

        tempCell.Previous = null;
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
                //Debug.Log(direction.x + " " + direction.z);
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

    private Vector2 CalculateGridIndex(Vector3 gridPosition)
    {
        return new Vector2(gridPosition.x / cellSize, gridPosition.z / cellSize);
    }

    private bool CheckIndexValidity(Vector2 cellIndex)
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
        // Check if index is within structure bounds
        return (index.x >= strPos.x - (strSize.x / 2f) && index.x <= strPos.x + (strSize.x / 2f) &&
                    index.y >= strPos.y - (strSize.y / 2f) && index.y <= strPos.y + (strSize.y) / 2f);
    }

    private void InitialiseStructure(GameObject structure)
    {
        strSize = structure.GetComponentInChildren<MeshRenderer>().bounds.size;
        strSize = new Vector3(
            (Mathf.Ceil(strSize.x) % 2 != 0 && strSize.x > 1) ? Mathf.Ceil(strSize.x) + 1 : Mathf.Ceil(strSize.x),
            (Mathf.Ceil(strSize.y) % 2 != 0 && strSize.y > 1) ? Mathf.Ceil(strSize.y) + 1 : Mathf.Ceil(strSize.y),
            (Mathf.Ceil(strSize.z) % 2 != 0 && strSize.z > 1) ? Mathf.Ceil(strSize.z) + 1 : Mathf.Ceil(strSize.z)
            );
        strPosition = structure.GetComponentInChildren<Transform>().position;
    }

    public Vector3Int? GetNeighbourPositionNullable(Vector3 gridPosition, NeighbourDirection direction)
    {
        Vector3Int? neightbourPosition = Vector3Int.FloorToInt(gridPosition);
        switch (direction)
        {
            case NeighbourDirection.Up:
                neightbourPosition += new Vector3Int(0, 0, cellSize);
                break;
            case NeighbourDirection.Right:
                neightbourPosition += new Vector3Int(cellSize, 0, 0);
                break;
            case NeighbourDirection.Down:
                neightbourPosition += new Vector3Int(0, 0, -cellSize);
                break;
            case NeighbourDirection.Left:
                neightbourPosition += new Vector3Int(-cellSize, 0, 0);
                break;
        }
        var index = CalculateGridIndex(neightbourPosition.Value);
        if (!CheckIndexValidity(index)) return null;
        return neightbourPosition;
    }

    public IEnumerable<StructureBase> GetAllStructuresData()
    {
        return GetAllStructuresAndData().Item1;
    }

    public IEnumerable<GameObject> GetAllStructures()
    {
        return GetAllStructuresAndData().Item2;
    }

    private (IEnumerable<StructureBase>, IEnumerable<GameObject>) GetAllStructuresAndData()
    {
        List<StructureBase> structureBases = new List<StructureBase>();
        List<GameObject> structures = new List<GameObject>();
        for (int row = 0; row < grid.GetLength(0); row++)
        {
            for (int col = 0; col < grid.GetLength(1); col++)
            {
                var data = grid[row, col].GetStructureBase();
                var structure = grid[row, col].GetStructure();
                if (data != null && !structures.Contains(structure))
                {
                    structureBases.Add(data);
                    structures.Add(structure);
                }
            }
        }
        return (structureBases, structures);
    }
}

public enum NeighbourDirection
{
    Up = 1,
    Right = 2,
    Down = 4,
    Left = 8,
}
