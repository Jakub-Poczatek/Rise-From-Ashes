using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StructureRepository : MonoBehaviour
{
    public StructPool modelDataPool;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<string> GetResourceGenStructNames()
    {
        return modelDataPool.resourceGenStructs.Select(rgStruct => rgStruct.structureName).ToList();
    }

    public string GetRoadStructName()
    {
        return modelDataPool.RoadStruct.structureName;
    }
}
