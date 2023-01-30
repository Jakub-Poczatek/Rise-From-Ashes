using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Road Structure", menuName = "StructureManagement/Data/RoadStruct")]
public class RoadStruct : StructureBase
{
    [Tooltip("Road facing up and right")]
    public GameObject cornerPrefab;
    [Tooltip("Road facing up, right and down")]
    public GameObject threeWayPrefab;
    public GameObject fourWayPrefab;
    public RotationValue rotationValue = RotationValue.R0;
}

public enum RotationValue
{
    R0,
    R90,
    R180,
    R270
}
