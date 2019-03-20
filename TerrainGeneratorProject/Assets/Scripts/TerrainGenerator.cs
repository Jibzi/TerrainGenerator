using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class TerrainGenerator : MonoBehaviour
{

    [SerializeField, Range(1, 200)] public int mapWidth = 10;
    [SerializeField, Range(1, 200)] public int mapHeight = 10;
    [SerializeField, Range(0.001f, 30f)] public float noiseScale = 1f;
    [SerializeField, Range(0.001f, 100f)] public float noiseStrength = 1f;
    [SerializeField, Range(1, 10)] public int octaves = 1;
    [SerializeField, Range(1, 16)] public float lacunarity = 2f;

    private MeshFilter _meshFilter;
    private float[,] noiseMap;

    void Start()
    {
        
        noiseMap = NoiseMap.GenerateNoiseMap(2, 2, 1, 1, 1);
        _meshFilter = gameObject.GetComponent<MeshFilter>();
        RefreshMap();
    }

    public void RefreshMap()
    {
        noiseMap = NoiseMap.GenerateNoiseMap(mapWidth, mapHeight, noiseScale, octaves, lacunarity);
            
        MeshData meshData = MeshGenerator.GenerateMesh(noiseMap, noiseStrength);
        _meshFilter.sharedMesh = meshData.CreateMesh();
        
        Debug.Log("New mesh generated.");
        Debug.Log("Vertex count: " + _meshFilter.sharedMesh.vertexCount);
        Debug.Log("Triangle array length: " + _meshFilter.sharedMesh.triangles.Length);
    }
}
