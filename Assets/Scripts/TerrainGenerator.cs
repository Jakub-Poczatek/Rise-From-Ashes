using Codice.Client.Common.GameUI;
using NSubstitute.Routing.Handlers;
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
    public Area flat, plains, hills;
    public GameObject[] trees;
    public GameObject grass;
    
    private float minHeight = float.MaxValue;
    private float maxHeight = float.MinValue;
    private Mesh mesh;
    private Vector3[] vertices;
    private int[] triangles;
    private Color[] colors;
    private Vector2[] uvs;
    private Area[] areas;
    private List<Area> areasToGenerate;
    private bool startUpGeneration = true;


    // Start is called before the first frame update
    void Start()
    {
        StartUpMeshGeneration();
        StartUpTrees();
        StartUpGrass();
        NavMeshManager.Instance.Rebake();
    }

    private void StartUpTrees()
    {
        GameObject parent = new GameObject("TreeParent");
        parent.transform.SetParent(transform.parent);
        Vector3 position;
        RaycastHit hit;
        bool foundPos = false;
        for (int i = 0; i < 250; i++)
        {
            do
            {
                position = new Vector3(Random.Range(-75, xSize - 75), 50, Random.Range(-75, xSize - 75));
                Physics.Raycast(position, Vector3.down, out hit, 99);
                try
                {
                    //print("Trying a cast at this position: " + position + "\nFound something at this y: " + hit.point.y + "\nName of found GO: " + hit.collider.gameObject.name);
                    foundPos = hit.collider.gameObject.CompareTag("Terrain");
                }
                catch (System.Exception)
                {
                    //print("Failed a cast at this position: " + position + "\nFound something at this y: " + hit.point.y);
                    foundPos = false;
                }
            } while (!foundPos);
            //print("Succeeded a cast: " + hit.collider.gameObject.name);
            position.y = vertices[Mathf.RoundToInt((position.x+100) + (xSize * (position.z+100)))].y;
            GameObject tree = Instantiate(trees[Random.Range(0, trees.Length)], position, Quaternion.identity);
            tree.isStatic = true;
            tree.transform.SetParent(parent.transform);
        }
    }

    private void StartUpGrass()
    {
        GameObject parent = new GameObject("GrassParent");
        parent.transform.SetParent(transform.parent);
        Vector3 position;
        RaycastHit hit;
        for (int z = -100; z < zSize-100; z++)
        {
            for (int x = -100; x < xSize-100; x++)
            {
                position = new Vector3(x, 50, z);
                Physics.Raycast(position, Vector3.down, out hit, Mathf.Infinity);
                try
                {
                    if (hit.collider.gameObject.CompareTag("Terrain") && Random.Range(0, 5) == 1)
                    {
                        position.y = vertices[(x + 100) + (xSize * (z + 100))].y;
                        GameObject g = Instantiate(grass, position, Quaternion.identity);
                        g.isStatic = true;
                        g.transform.SetParent(parent.transform);
                    }
                } catch (System.Exception)
                {
                    continue;
                }
            }
        }
    }

    /*private void Update()
    {
        minHeight = float.MaxValue;
        maxHeight = float.MinValue;
        CreateMesh();
        UpdateMesh();
    }*/

    private void StartUpMeshGeneration()
    {
        areasToGenerate = new List<Area>();
        areas = new Area[]
        {
            flat, plains, hills
        };
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        offsetX = Random.Range(-100000, 100000);
        offsetZ = Random.Range(-100000, 100000);
        CreateMesh();
        UpdateMesh();
        startUpGeneration = false;
    }

    private void CreateMesh()
    {
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];
        triangles = new int[xSize * zSize * 6];
        colors = new Color[vertices.Length];
        uvs = new Vector2[vertices.Length];
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
                    else if (z >= citySectorMin - (sectionSize * 1) && z <= citySectorMax + (sectionSize * 1) && x >= citySectorMin - (sectionSize * 1) && x <= citySectorMax + (sectionSize * 1))
                    {
                        tempArea = plains;
                    }
                    else
                        tempArea = areas[Random.Range(1, 3)];

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
                uvs[x + (xSize * z)] = new Vector2((float) x / xSize,(float) z / zSize);
            }
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
        float noise = Mathf.PerlinNoise(xPoint, zPoint);
        noise = (2 * noise - 1) * area.perlinMultiplier;
        //noise = Mathf.Clamp(noise, -1, 1) * area.height;
        return noise;
            
    }

    private void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.colors = colors;
        mesh.RecalculateNormals();
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }
}

[System.Serializable]
public class Area
{
    public float perlinTilling, perlinMultiplier;
}