using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class TerrainGenerator : MonoBehaviour
{

    [SerializeField, Range(1, 300)] public int mapWidth = 10;
    [SerializeField, Range(1, 300)] public int mapHeight = 10;
    
    [SerializeField, Range(0.01f, 60f)] public float noiseScale = 1f;
    private float Rate_noiseScale = 4f;
    
    [SerializeField, Range(0.001f, 100f)] public float noiseStrength = 1f;
    private float Rate_noiseStrength = 4f;
    
    [SerializeField, Range(1, 10)] public int octaves = 1;
    private int Rate_octaves = 1;
    
    [SerializeField, Range(1, 16)] public float lacunarity = 2f;
    private float Rate_lacunarity = 0.5f;

    [SerializeField, Range(-5f, 5f)] public float minLevel = 0f;

    [SerializeField] public int erosionIterations = 1000;
    [SerializeField] public DropData erosionSettings;

    private MeshFilter _meshFilter;
    private MeshData _meshData;
    private float[,] noiseMap;

    private DropManager _dropManager;

    void Start()
    {
        
        noiseMap = NoiseMap.GenerateNoiseMap(2, 2, 1, 1, 1, 0f);
        _meshFilter = gameObject.GetComponent<MeshFilter>();
        RefreshMap();
    }

    public void OnValidate()
    {
        if (_meshFilter == null)
        {
            _meshFilter = gameObject.GetComponent<MeshFilter>();
        }
    }

    public void RefreshMap()
    {
        noiseMap = NoiseMap.GenerateNoiseMap(mapWidth, mapHeight, noiseScale, octaves, lacunarity, minLevel);
            
        _meshData = MeshGenerator.GenerateMesh(noiseMap, noiseStrength);
        _meshFilter.sharedMesh = _meshData.CreateMesh();
        
    }

    public void RefreshGeometry()
    {
        _meshData = MeshGenerator.GenerateMesh(noiseMap, noiseStrength);
        _meshFilter.sharedMesh = _meshData.CreateMesh();
    }

    //Returns the value of a given coordinate. Returns a negative value if there is currently no noiseMap generated.
    public float SampleCurrentMap(int x, int y)
    {
        if (noiseMap != null)
        {
            if (x > -1 && x < mapWidth && (y > -1 && y < mapHeight))
            {
                return noiseMap[x, y];
            }
        }

        return -1f;
    }

    public int GetMapWidth()
    {
        return mapWidth;
    }

    public int GetMapHeight()
    {
        return mapHeight;
    }

    public void AlterHeightAtIndex(int x, int y, float val)
    {
        if (noiseMap != null)
        {
            if (x > -1 && x < mapWidth && (y > -1 && y < mapHeight))
            {
                noiseMap[x, y] += val;
            }
        }
    }

    public void BeginSim()
    {
        _dropManager = new DropManager(this, erosionSettings);
        _dropManager.BeginSim(erosionIterations);
    }

    public void Update()
    {
        if (Input.GetKey(KeyCode.S))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                noiseScale -= Rate_noiseScale * Time.deltaTime;
                RefreshMap();
                return;
            }
            else
            {
                noiseScale += Rate_noiseScale * Time.deltaTime;
                RefreshMap();
                return;
            }
        }

        if (Input.GetKey(KeyCode.T))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                noiseStrength -= Rate_noiseStrength * Time.deltaTime;
                RefreshMap();
                return;
            }
            else
            {
                noiseStrength += Rate_noiseStrength * Time.deltaTime;
                RefreshMap();
                return;
            }
        }

        if (Input.GetKey(KeyCode.L))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                lacunarity -= Rate_lacunarity * Time.deltaTime;
                RefreshMap();
                return;
            }
            else
            {
                lacunarity += Rate_lacunarity * Time.deltaTime;
                RefreshMap();
                return;
            }
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                octaves -= Rate_octaves;
                RefreshMap();
                return;
            }
            else
            {
                octaves += Rate_octaves;
                RefreshMap();
                return;
            }
        }
    }
}
