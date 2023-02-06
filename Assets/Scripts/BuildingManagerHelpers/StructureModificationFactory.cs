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

    public static void PrepareStructureModificationFactory(StructureRepository structureRepository, GridStructure gridStructure, IPlacementManager placementManager)
    {
        singleStructurePlacementHelper =
            new SingleStructurePlacementHelper(structureRepository, gridStructure, placementManager);
        structureDemolishingHelper =
            new StructureDemolishingHelper(structureRepository, gridStructure, placementManager);
        roadStructurePlacementHelper =
            new RoadPlacementModificationHelper(structureRepository, gridStructure, placementManager);
    }

    public static StructureModificationHelper GetHelper(Type classType)
    {
        if(classType == typeof(PlayerDemolishingState)) return structureDemolishingHelper;
        else if(classType == typeof(PlayerBuildingRoadState)) return roadStructurePlacementHelper;
        else return singleStructurePlacementHelper;
    }
}
