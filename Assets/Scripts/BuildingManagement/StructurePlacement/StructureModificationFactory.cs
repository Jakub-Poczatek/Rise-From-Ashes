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
    private static StructureModificationHelper residentialPlacementHelper;

    public static void PrepareStructureModificationFactory(StructureRepository structureRepository, GridStructure gridStructure)
    {
        singleStructurePlacementHelper =
            new SingleStructurePlacementHelper(structureRepository, gridStructure);
        structureDemolishingHelper =
            new StructureDemolishingHelper(structureRepository, gridStructure);
        roadStructurePlacementHelper =
            new RoadPlacementModificationHelper(structureRepository, gridStructure);
        residentialPlacementHelper = 
            new ResidentialPlacementHelper(structureRepository, gridStructure);
    }

    public static StructureModificationHelper GetHelper(Type classType)
    {
        if(classType == typeof(PlayerDemolishingState)) return structureDemolishingHelper;
        else if(classType == typeof(PlayerBuildingRoadState)) return roadStructurePlacementHelper;
        else if(classType == typeof(PlayerBuildingResidentialState)) return residentialPlacementHelper;
        else return singleStructurePlacementHelper;
    }
}
