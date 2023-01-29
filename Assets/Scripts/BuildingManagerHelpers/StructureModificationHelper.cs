using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StructureModificationHelper
{
    protected Dictionary<Vector3Int, GameObject> structuresToBeModified = new Dictionary<Vector3Int, GameObject>();
    protected readonly StructureRepository structureRepository;
    protected readonly GridStructure gridStructure;
    protected readonly PlacementManager placementManager;
    protected readonly ResourceManager resourceManager;


    public StructureModificationHelper(StructureRepository structureRepository, GridStructure gridStructure,
        PlacementManager placementManager, ResourceManager resourceManager)
    {
        this.structureRepository = structureRepository;
        this.gridStructure = gridStructure;
        this.placementManager = placementManager;
        this.resourceManager = resourceManager;
    }

    public GameObject GetStructureToBeModified(Vector3 gridPosition)
    {
        Vector3Int gridPositionInt = Vector3Int.FloorToInt(gridPosition);
        if (structuresToBeModified.ContainsKey(gridPositionInt))
        {
            return structuresToBeModified[gridPositionInt];
        }
        return null;
    }

    public abstract void CancelModifications();
    public abstract void ConfirmModifications();
    public abstract void PrepareStructureForModification(Vector3 position, string structureName = "", StructureType structureType = StructureType.None);
}
