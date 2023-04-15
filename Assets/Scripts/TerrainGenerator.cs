using Codice.Client.Common.GameUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public int xSize;
    public int zSize;
    public int sectionSize;
    public Gradient gradient; 
    public float offsetX;
    public float offsetZ;
    public Area flat, plains;
    
    private float minHeight = float.MaxValue;
    private float maxHeight = float.MinValue;
    private Mesh mesh;
    private Vector3[] vertices;
    private int[] triangles;
    private Color[] colors;
    private Area[] areas;
    private List<Area> areasToGenerate;
    private bool startUpGeneration = true;


    // Start is called before the first frame update
    void Start()
    {
        StartUpMeshGeneration();
    }

    private void CreateMesh()
    {
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];
        triangles = new int[xSize * zSize * 6];
        colors = new Color[vertices.Length];
        int citySectorMin = Mathf.CeilToInt(xSize * 0.40f);
        int citySectorMax = Mathf.CeilToInt(xSize * 0.60f);

        int areaCounter = 0;
        Area tempArea;

        for (int z = 0; z < zSize; z += sectionSize)
        {
            for (int x = 0; x < xSize; x += sectionSize)
            {
                if (startUpGeneration)
                {
                    if (z >= citySectorMin && z < citySectorMax && x >= citySectorMin && x < citySectorMax)
                    {
                        tempArea = flat;
                    }
                    else if (z >= citySectorMin - (sectionSize * 2) && z <= citySectorMax + (sectionSize * 2) && x >= citySectorMin - (sectionSize * 2) && x <= citySectorMax + (sectionSize * 2))
                    {
                        tempArea = plains;
                    }
                    else
                        tempArea = plains;

                    areasToGenerate.Add(tempArea);
                    CreateSection(x, z, tempArea);
                } 
                else
                {
                    CreateSection(x, z, areasToGenerate[areaCounter]);
                    areaCounter++;
                }
            }
        }

        int tris = 0;
        int vert = 0;

        for (int z = 0; z < zSize - 1; z++)
        {
            for (int x = 0; x < xSize - 1; x++)
            {
                triangles[tris] = vert;
                triangles[tris + 1] = vert + xSize;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize;
                triangles[tris + 5] = vert + xSize + 1;

                vert++;
                tris += 6;
            }
            vert++;
        }

        for (int z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float height = Mathf.InverseLerp(minHeight, maxHeight, vertices[x + (xSize * z)].y);
                colors[x + (xSize * z)] = gradient.Evaluate(height);
            }
        }
    }

    private void CreateSection(int xOrigin, int zOrigin, Area area)
    {

        for (int z = zOrigin; z < zOrigin + sectionSize; z++)
        {
            for (int x = xOrigin; x < xOrigin + sectionSize; x++)
            {
                float y = CalculatePerlinNoise(x, z, area);
                vertices[x + (xSize * z)] = new Vector3(x, y, z);

                if (y > maxHeight) maxHeight = y;
                if (y < minHeight) minHeight = y;
            }
        }
    }

    private float CalculatePerlinNoise(int x, int z, Area area)
    {
        float xPoint = x * area.perlinTilling + offsetX;
        float zPoint = z * area.perlinTilling + offsetZ;
        float noise = Mathf.PerlinNoise(xPoint, zPoint) * area.perlinMultiplier;
        noise = Mathf.Clamp01(noise) * area.height;
        return noise;
            
    }

    private void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.colors = colors;
        mesh.RecalculateNormals();
    }

    private void StartUpMeshGeneration()
    {
        areasToGenerate = new List<Area>();
        areas = new Area[]
        {
            flat, plains
        };
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        offsetX = Random.Range(-100000, 100000);
        offsetZ = Random.Range(-100000, 100000);
        CreateMesh();
        UpdateMesh();
        startUpGeneration = false;
    }
}

[System.Serializable]
public class Area
{
    public float perlinTilling, perlinMultiplier, height;
}

/*private void Update()
    {
        *//*minHeight = float.MaxValue;
        maxHeight = float.MinValue;
        CreateMesh();
        UpdateMesh();*//*
    }*/