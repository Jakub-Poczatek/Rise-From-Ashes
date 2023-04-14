using System.Collections;
using UnityEngine;

public class MeshGeneration : MonoBehaviour
{
    public int xSize;
    public int zSize;
    public int sectionSize;
    public Gradient gradient; 
    public float offsetX;
    public float offsetZ;
    public Area flat, plains, hills, mountain;
    
    private float minHeight = float.MaxValue;
    private float maxHeight = float.MinValue;
    private Mesh mesh;
    private Vector3[] vertices;
    private int[] triangles;
    private Color[] colors;
    private Area[] areas;


    // Start is called before the first frame update
    void Start()
    {
        areas = new Area[]
        {
            flat, plains, hills, mountain
        };
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        offsetX = Random.Range(-100000, 100000);
        offsetZ = Random.Range(-100000, 100000);
        CreateMesh();
        UpdateMesh();
    }

    private void Update()
    {
        minHeight = float.MaxValue;
        maxHeight = float.MinValue;
        CreateMesh();
        UpdateMesh();
    }

    private void CreateMesh()
    {
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];
        triangles = new int[xSize * zSize * 6];
        int index = 0;
        int vert = 0;
        int tris = 0;


        int tempCounter = 0;
        for (int z = 0; z < zSize; z += sectionSize)
        {
            for (int x = 0; x < xSize; x += sectionSize)
            {
                CreateSection(index, vert, tris, x, z, areas[tempCounter]);
                tempCounter++;

                // Update index and tris
                index += sectionSize * sectionSize;
                tris += (sectionSize - 1) * (sectionSize - 1) * 6;
            }

            // Update vert
            vert += sectionSize * (sectionSize - 1);
        }

        tris = 0;
        vert = 0;

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
                //UpdateMesh();
            }
            vert++;
        }
    }

    private (int, int, int) CreateSection(int index, int vert, int tris, int xOrigin, int zOrigin, Area area)
    {

        for (int z = zOrigin; z < zOrigin + sectionSize; z++)
        {
            for (int x = xOrigin; x < xOrigin + sectionSize; x++)
            {
                float y = Mathf.PerlinNoise(x * area.perlinTilling + offsetX, z * area.perlinTilling + offsetZ) * area.perlinMultiplier;
                vertices[x + (xSize * z)] = new Vector3(x, y, z);

                if (y > maxHeight) maxHeight = y;
                if (y < minHeight) minHeight = y;

                index++;
            }
        }

        return (index, tris, vert);
    }

    

    private void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        //mesh.colors = colors;
        mesh.RecalculateNormals();
    }

    private void OnDrawGizmos()
    {
        if (vertices == null)
            return;

        for (int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i], 0.1f);
        }
    }
}

[System.Serializable]
public class Area
{
    public float perlinTilling, perlinMultiplier;
}


/*private void CreateMesh()
    {
        vertices = new Vector3[(xSize+1) * (zSize+1)];
        triangles = new int[xSize * zSize * 6];
        int index = 0;
        int vert = 0;
        int tris = 0;
        for (int z = 0; z < zSize; z += sectionSize)
        {
            for (int x = 0; x < xSize; x += sectionSize)
            {
                //StartCoroutine(CreateSection(index, vert, tris, x, z, flat));
                (index, vert, tris) = CreateSection(index, vert, tris, x, z, flat);
                *//*index += 100;
                vert += 100;
                tris += 600;*/

/*if (x == 4 && z == 4)
    CreateSection(x, z, flat);
else
    CreateSection(x, z, areas[Random.Range(1, areas.Length - 1)]);*//*
}
}

*//*int vert = 0;
int tris = 0;
triangles = new int[xSize * zSize * 6];


for (int i = 0; i < 4; i++)
{
    for (int z = 0; z < sectionSize - 1; z++)
    {
        for (int x = 0; x < sectionSize - 1; x++)
        {
            triangles[tris] = vert;
            triangles[tris + 1] = vert + sectionSize;
            triangles[tris + 2] = vert + 1;
            triangles[tris + 3] = vert + 1;
            triangles[tris + 4] = vert + sectionSize;
            triangles[tris + 5] = vert + sectionSize + 1;

            vert++;
            tris += 6;
            yield return new WaitForSeconds(0.5f);
            UpdateMesh();
        }
        vert++;
    }
    print("NextIter");
    vert++;
}*//*
}*/

/*private IEnumerator CreateSection(int index, int vert, int tris, int xOrigin, int zOrigin, Area area)
    {

        for (int z = zOrigin; z < zOrigin+sectionSize; z++)
        {
            for (int x = xOrigin; x < xOrigin+sectionSize; x++)
            {
                float y = Mathf.PerlinNoise(x * area.perlinTilling + offsetX, z * area.perlinTilling + offsetZ) * area.perlinMultiplier;
                *//*float y = Mathf.PerlinNoise(Mathf.PerlinNoise(x * area.perlinTilling + offsetX, z * area.perlinTilling + area.zOffset) * area.perlinMultiplier,
                    Mathf.PerlinNoise(x * perlingTilling + offsetX, z * perlingTilling + offsetZ) * perlinMultiplier) * secondaryPerlinMultiplier;*//*
                vertices[index] = new Vector3(x, y, z);
                yield return new WaitForSeconds(0.1f);

                if (y > maxHeight) maxHeight = y;
                if(y < minHeight) minHeight = y;

                index++;
            }
        }


        for (int z = 0; z < sectionSize - 1; z++)
        {
            for (int x = 0; x < sectionSize - 1; x++)
            {
                triangles[tris] = vert;
                triangles[tris + 1] = vert + sectionSize;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + sectionSize;
                triangles[tris + 5] = vert + sectionSize + 1;

                vert++;
                tris += 6;
                yield return new WaitForSeconds(0.01f);
                UpdateMesh();
            }
            vert++;
        }
        print("NextIter");


        *//*for (int z = zOrigin; z < sectionSize - 1; z++)
        {
            for (int x = xOrigin; x < sectionSize - 1; x++)
            {
                triangles[tris + 0] = x + (xSize * z) + vert;
                triangles[tris + 1] = x + (xSize * z) + xSize + vert;
                triangles[tris + 2] = x + (xSize * z) + 1 + vert;
                triangles[tris + 3] = x + (xSize * z) + 1 + vert;
                triangles[tris + 4] = x + (xSize * z) + xSize + vert;
                triangles[tris + 5] = x + (xSize * z) + xSize + 1 + vert;

                tris += 6;
            }
        }*/

/*colors = new Color[vertices.Length];
for (int i = 0, z = 0; z <= zSize; z++)
{
    for (int x = 0; x <= xSize; x++)
    {
        float height = Mathf.InverseLerp(minHeight, maxHeight, vertices[i].y);
        colors[i] = gradient.Evaluate(height);
        i++;
    }
}*//*
}*/