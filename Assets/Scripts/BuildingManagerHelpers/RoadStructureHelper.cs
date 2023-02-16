using UnityEngine;

public class RoadStructureHelper
{
    public RotationValue RoadPrefabRotation { get; set; }
    public GameObject RoadPrefab { get; set; }

    public RoadStructureHelper(GameObject roadPrefab, RotationValue rotationValue)
    {
        this.RoadPrefab = roadPrefab;
        this.RoadPrefabRotation = rotationValue;
    }
}