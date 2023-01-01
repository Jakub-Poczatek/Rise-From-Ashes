using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Structure Pool", menuName = "StructureManagement/Pool")]
public class StructPool : ScriptableObject
{
    public RoadStruct RoadStruct;
    public List<ResourceGenStruct> resourceGenStructs;
}
