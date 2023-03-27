using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;

public static class StructureModificationFactory
{
    private static StructureModificationHelper resGenModificationHelper;
    private static StructureModificationHelper demolishingHelper;
    private static StructureModificationHelper roadModificationHelper;
    private static StructureModificationHelper residentialModificationHelper;

    public static void PrepareStructureModificationFactory(StructureRepository structureRepository, GridStructure gridStructure)
    {
        resGenModificationHelper =
            new ResGenModificationHelper(structureRepository, gridStructure);
        demolishingHelper =
            new StructureDemolishingHelper(structureRepository, gridStructure);
        roadModificationHelper =
            new RoadModificationHelper(structureRepository, gridStructure);
        residentialModificationHelper = 
            new ResidentialModificationHelper(structureRepository, gridStructure);
    }

    public static StructureModificationHelper GetHelper(Type classType)
    {
        if(classType == typeof(PlayerDemolishingState)) return demolishingHelper;
        else if(classType == typeof(PlayerBuildingRoadState)) return roadModificationHelper;
        else if(classType == typeof(PlayerBuildingResidentialState)) return residentialModificationHelper;
        else return resGenModificationHelper;
    }
}
