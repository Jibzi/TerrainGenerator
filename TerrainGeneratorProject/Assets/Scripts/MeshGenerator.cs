using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshGenerator
{

    private static float limitAmplitudeScale = 0.00001f;

    public static MeshData GenerateMesh(float[,] heightMap, float amplitudeScale)
    {
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);
        float topLeftX = (width - 1) / -2f;
        float topLeftZ = (height - 1) / 2f;

        if (amplitudeScale < limitAmplitudeScale)
        {
            amplitudeScale = limitAmplitudeScale;
        }

        MeshData meshData = new MeshData(width, height);
        int vertIndex = 0;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
   
                meshData.verts[vertIndex] = new Vector3(topLeftX + x, (heightMap[x, y] * amplitudeScale), topLeftZ - y);
                meshData.uvs[vertIndex] = new Vector2(x / (float)width, y / (float)height);

                if ((x < width - 1) && (y < height - 1))
                {
                    meshData.AppendTriangleByIndex(vertIndex, (vertIndex + width + 1), (vertIndex + width));
                    meshData.AppendTriangleByIndex((vertIndex + width + 1), vertIndex, (vertIndex + 1));
                }

                vertIndex++;
            }
        }

        return meshData;
    }
}

public class MeshData
{
    public Vector3[] verts;
    public int[] tris;
    public Vector2[] uvs;

    int triIndex;

    public MeshData(int meshWidth, int meshHeight)
    {
        verts = new Vector3[meshWidth * meshHeight];
        tris = new int[(meshWidth - 1) * (meshHeight - 1) * 6];
        uvs = new Vector2[meshWidth * meshHeight];
    }

    public void AppendTriangleByIndex(int a, int b, int c)
    {
        tris[triIndex] = a;
        tris[triIndex + 1] = b;
        tris[triIndex + 2] = c;
        triIndex += 3;
    }

    public Mesh CreateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = verts;
        mesh.triangles = tris;
        mesh.uv = uvs;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        mesh.name = "Generated Mesh";

        return mesh;
    }
}
