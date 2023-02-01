using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;

public static class StructureModificationFactory
{
    private static StructureModificationHelper singleStructurePlacementHelper;
    private static StructureModificationHelper structureDemolishingHelper;
    private static StructureModificationHelper roadStructurePlacementHelper;

    public static void PrepareStructureModificationFactory(StructureRepository structureRepository, GridStructure gridStructure, IPlacementManager placementManager,
        ResourceManager resourceManager)
    {
        singleStructurePlacementHelper =
            new SingleStructurePlacementHelper(structureRepository, gridStructure, placementManager, resourceManager);
        structureDemolishingHelper =
            new StructureDemolishingHelper(structureRepository, gridStructure, placementManager, resourceManager);
        roadStructurePlacementHelper =
            new RoadPlacementModificationHelper(structureRepository, gridStructure, placementManager, resourceManager);
    }

    public static StructureModificationHelper GetHelper(Type classType)
    {
        if(classType == typeof(PlayerDemolishingState)) return structureDemolishingHelper;
        else if(classType == typeof(PlayerBuildingRoadState)) return roadStructurePlacementHelper;
        else return singleStructurePlacementHelper;
    }
}
