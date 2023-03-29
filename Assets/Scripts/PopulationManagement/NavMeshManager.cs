using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class NavMeshManager : MonoBehaviour
{
    public NavMeshSurface navMeshSurface;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    private void Start()
    {
        Rebake();
    }

    public static NavMeshManager Instance { get; private set; }

    private NavMeshManager() { }

    public void Rebake()
    {
        navMeshSurface.BuildNavMesh();
    }
}
