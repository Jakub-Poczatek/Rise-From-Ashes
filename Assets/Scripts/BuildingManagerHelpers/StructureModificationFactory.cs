using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;

public class StructureModificationFactory
{
    private readonly StructureModificationHelper singleStructurePlacementHelper;
    private readonly StructureModificationHelper structureDemolishingHelper;
    private readonly StructureModificationHelper roadStructurePlacementHelper;

    public StructureModificationFactory(StructureRepository structureRepository, GridStructure gridStructure, IPlacementManager placementManager,
        ResourceManager resourceManager)
    {
        singleStructurePlacementHelper =
            new SingleStructurePlacementHelper(structureRepository, gridStructure, placementManager, resourceManager);
        structureDemolishingHelper =
            new StructureDemolishingHelper(structureRepository, gridStructure, placementManager, resourceManager);
        roadStructurePlacementHelper =
            new RoadPlacementModificationHelper(structureRepository, gridStructure, placementManager, resourceManager);
    }

    public StructureModificationHelper GetHelper(Type classType)
    {
        if(classType == typeof(PlayerDemolishingState)) return structureDemolishingHelper;
        else if(classType == typeof(PlayerBuildingRoadState)) return roadStructurePlacementHelper;
        else return singleStructurePlacementHelper;
    }
}
