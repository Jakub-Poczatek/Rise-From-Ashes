using Codice.Client.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager
{
    private GridStructure gridStructure;
    private StructureRepository structureRepository;
    private StructureModificationHelper helper;

    public BuildingManager(StructureRepository structureRepository,
        int cellSize, int width, int length)
    {
        this.gridStructure = new GridStructure(cellSize, width, length);
        this.structureRepository = structureRepository;
        StructureModificationFactory.PrepareStructureModificationFactory(structureRepository, gridStructure);
    }

    public void PrepareBuildingManager(Type classType)
    {
        helper = StructureModificationFactory.GetHelper(classType);
    }

    public void PrepareStructureForModification(Vector3 position, string structureName, StructureType structureType)
    {
        helper.PrepareStructureForModification(position, structureName, structureType);
    }

    public void ConfirmModification()
    {
        helper.ConfirmModifications();
        NavMeshManager.Instance.Rebake();
    }

    public void CancelModification()
    {
        helper?.CancelModifications();
    }

    public void PrepareStructureForDemolishing(Vector3 position)
    {
        helper.PrepareStructureForModification(position);
    }

    public GameObject GetStructureFromGrid(Vector3 position)
    {
        Vector3 gridPosition = gridStructure.CalculateGridPosition(position);
        if(gridStructure.IsCellTaken(gridPosition))
        {
            return gridStructure.GetStructureFromTheGrid(gridPosition);
        }
        return null;
    }

    public GameObject GetStructureToBeModified(Vector3 position)
    {
        Vector3 gridPosition = gridStructure.CalculateGridPosition(position);
        GameObject structureToReturn = helper.GetStructureToBeModified(gridPosition);
        if(structureToReturn != null)
        {
            return structureToReturn;
        }
        structureToReturn = helper.GetStructureToBeModified(gridPosition);
        return structureToReturn;
    }

    public IEnumerable<StructureBase> GetAllStructuresData()
    {
        return gridStructure.GetAllStructuresData();
    }

    public IEnumerable<GameObject> GetAllStructures()
    {
        return gridStructure.GetAllStructures();
    }

    public StructureBase GetStructureBaseFromPosition(Vector3 position)
    {
        Vector3 gridPosition = gridStructure.CalculateGridPosition(position);
        if (gridStructure.IsCellTaken(gridPosition))
        {
            return gridStructure.GetStructureBaseFromGrid(gridPosition);
        }
        return null;
    }

    public void PreviewStructure(string structureName, StructureType structureType)
    {
        StructureBase structureBase = structureRepository.GetStructureByName(structureName, structureType);
        GameObject model = structureBase.prefab.transform.Find("ModelParent").gameObject;
        GameObject preview = GameObject.Instantiate(model, model.transform.position, model.transform.rotation);
        preview.AddComponent<FollowMouse>();

        // Set preview materials

    }
}
