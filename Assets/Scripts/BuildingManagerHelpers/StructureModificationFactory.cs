using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;

public class StructureModificationFactory
{
    private readonly StructureModificationHelper singleStructurePlacementHelper;
    private readonly StructureModificationHelper structureDemolishingHelper;

    public StructureModificationFactory(StructureRepository structureRepository, GridStructure gridStructure, IPlacementManager placementManager,
        ResourceManager resourceManager)
    {
        singleStructurePlacementHelper =
            new SingleStructurePlacementHelper(structureRepository, gridStructure, placementManager, resourceManager);
        structureDemolishingHelper =
            new StructureDemolishingHelper(structureRepository, gridStructure, placementManager, resourceManager);
    }

    public StructureModificationHelper GetHelper(Type classType)
    {
        if(classType == typeof(PlayerDemolishingState))
        {
            return structureDemolishingHelper;
        } else
        {
            return singleStructurePlacementHelper;
        }
    }
}
