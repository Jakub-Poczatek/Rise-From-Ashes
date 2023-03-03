using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StructureModificationHelper
{
    protected Dictionary<Vector3Int, GameObject> structuresToBeModified = new Dictionary<Vector3Int, GameObject>();
    protected readonly StructureRepository structureRepository;
    protected readonly GridStructure gridStructure;
    protected readonly IPlacementManager placementManager;
    protected IResourceManager resourceManager;
    protected StructureBase structureBase;


    public StructureModificationHelper(StructureRepository structureRepository, GridStructure gridStructure,
        IPlacementManager placementManager, IResourceManager resourceManager)
    {
        this.structureRepository = structureRepository;
        this.gridStructure = gridStructure;
        this.placementManager = placementManager;
        this.resourceManager = resourceManager;
        structureBase = ScriptableObject.CreateInstance<NullStructure>();
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

    public virtual void ConfirmModifications()
    {
        placementManager.DisplayStructureOnMap(structuresToBeModified.Values);
        ResetHelper();
    }

    public virtual void CancelModifications()
    {
        placementManager.DestroyDisplayedStructures(structuresToBeModified.Values);
        foreach (var keyValuePair in structuresToBeModified)
        {
            gridStructure.RemoveStructureFromTheGrid(keyValuePair.Key);
        }
        ResetHelper();
    }

    public virtual void PrepareStructureForModification(Vector3 position, string structureName = "", StructureType structureType = StructureType.None)
    {
        if (structureBase.GetType() == typeof(NullStructure))
            structureBase = this.structureRepository.GetStructureByName(structureName, structureType);
    }

    private void ResetHelper()
    {
        structuresToBeModified.Clear();
        structureBase = ScriptableObject.CreateInstance<NullStructure>();
    }
}
