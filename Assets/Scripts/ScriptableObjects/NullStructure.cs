using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NullStructure : StructureBase
{
    private void OnEnable()
    {
        name = "Nullable Object";
        prefab = null;
        buildCost = new Cost(0, 0, 0);
        maintenanceGoldCost = 0;
    }
}
