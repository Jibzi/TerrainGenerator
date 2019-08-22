using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Numerics;
using UnityEngine;
using UnityEngine.Networking;

public class DropManager
{


    public Vector2Int[] neighboursStencil = new Vector2Int[8];

    [SerializeField] private DropData _settings;
    
    [SerializeField] private TerrainGenerator _owner;
    private Drop _drop;

    public DropManager(TerrainGenerator owner, DropData settings)
    {
        _owner = owner;
        
        neighboursStencil[0] = Vector2Int.left;
        neighboursStencil[1] = Vector2Int.right;
        neighboursStencil[2] = Vector2Int.down;
        neighboursStencil[3] = Vector2Int.up;
        neighboursStencil[4] = Vector2Int.up + Vector2Int.right;
        neighboursStencil[5] = Vector2Int.down + Vector2Int.right;
        neighboursStencil[6] = Vector2Int.up + Vector2Int.left;
        neighboursStencil[7] = Vector2Int.down + Vector2Int.left;

        _settings = settings;

        _drop = new Drop(10, 10, 10, _settings, this);
        
    }

    public TerrainGenerator GetOwner()
    {
        return _owner;
    }

    public void Update()
    {
        _drop.DropUpdate();
    }

    public void BeginSim(int iterations)
    {
        for (int i = 0; i < iterations; i++)
        {
            _drop.Simulate();
            ResetDrop();
        }
        
        //Need to update the terrain mesh now.
        _owner.RefreshGeometry();
    }

    public void ResetDrop()
    {
        _drop = new Drop(Random.Range(0, _owner.GetMapWidth()), Random.Range(0, _owner.GetMapHeight()), 10, _settings, this);
    }
}
