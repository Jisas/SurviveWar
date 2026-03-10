using System;
using UnityEngine;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

[RequireComponent(typeof(MeshFilter))]
public class TerrainMeshGenerator : MonoBehaviour
{
    public MeshFilter meshFilter;
    public TerrainMeshVariables meshVariables;

    public void OnValidate()
    {
        GenerateMap();
    }

    public void GenerateMap()
    {
        MeshGenerator meshGenerator = new MeshGenerator(meshVariables);
        meshGenerator.Schedule(meshVariables.terrainMeshDetail * meshVariables.terrainMeshDetail, 10000).Complete();

        meshFilter.mesh = meshGenerator.DisposeAndGetMesh();
    }
}

[BurstCompile]
public struct MeshGenerator : IJobParallelFor
{

    [NativeDisableParallelForRestriction] private NativeArray<Vector3> _verticies;
    [NativeDisableParallelForRestriction] private NativeArray<int> _triangleIndicies;
    private TerrainMeshVariables _meshVariables;

    public MeshGenerator(TerrainMeshVariables mv)
    {
        _meshVariables = mv;

        _verticies = new NativeArray<Vector3>(_meshVariables.TotalVerts, Allocator.TempJob);
        _triangleIndicies = new NativeArray<int>(_meshVariables.TotalTriangles, Allocator.TempJob);
    }

    public void Execute(int threadIndex)
    {
        int x = threadIndex / _meshVariables.terrainMeshDetail;
        int y = threadIndex % _meshVariables.terrainMeshDetail;

        int a = threadIndex + Mathf.FloorToInt(threadIndex / (float)_meshVariables.terrainMeshDetail);
        int b = a + 1;
        int c = b + _meshVariables.terrainMeshDetail;
        int d = c + 1;

        _verticies[a] = new Vector3(x + 0, 0, y + 0) * _meshVariables.TileEdgeLength;
        _verticies[b] = new Vector3(x + 0, 0, y + 1) * _meshVariables.TileEdgeLength;
        _verticies[c] = new Vector3(x + 1, 0, y + 0) * _meshVariables.TileEdgeLength;
        _verticies[d] = new Vector3(x + 1, 0, y + 1) * _meshVariables.TileEdgeLength;

        _triangleIndicies[threadIndex * 6 + 0] = a;
        _triangleIndicies[threadIndex * 6 + 1] = b;
        _triangleIndicies[threadIndex * 6 + 2] = c;
        _triangleIndicies[threadIndex * 6 + 3] = b;
        _triangleIndicies[threadIndex * 6 + 4] = d;
        _triangleIndicies[threadIndex * 6 + 5] = c;
    }

    public Mesh DisposeAndGetMesh()
    {
        var m = new Mesh();

        m.SetVertices(_verticies);
        m.triangles = _triangleIndicies.ToArray();
        m.uv = ConfigureUV();

        m.RecalculateNormals();

        _verticies.Dispose();
        _triangleIndicies.Dispose();
        return m;
    }

    public Vector2[] ConfigureUV()
    {
        Vector2[] uvs = new Vector2[_verticies.Length];

        for (int i = 0; i < _verticies.Length; i++)
        {
            uvs[i] = new Vector2(i % _meshVariables.terrainMeshDetail, i / _meshVariables.terrainMeshDetail);
        }

        return uvs;
    }
}

[Serializable]
public struct TerrainMeshVariables
{
    [Range(1, 255)]
    public int terrainMeshDetail;
    public float terrainWidth;
    public float height;
    public int TotalVerts => (terrainMeshDetail + 1) * (terrainMeshDetail + 1);
    public int TotalTriangles => terrainMeshDetail * terrainMeshDetail * 6;
    public float TileEdgeLength => terrainWidth / terrainMeshDetail;
}
[Serializable]
public struct TerrainHeightmapVariables
{
    [Header("Noise")]
    public float noiseScale;
    [Range(0, 4)]
    public float frequency, lacunarity;
    [Range(1, 10)]
    public int octaves;
    public float weight;

    public float falloffSteepness, falloffOffset;
    [Header("Extras")]
    public float waterLevel;
}

public struct Maps
{
    [NativeDisableParallelForRestriction] public NativeArray<float> HeightMap;
    [NativeDisableParallelForRestriction] public NativeArray<Color> ColorMap;

    public Maps(NativeArray<float> h, NativeArray<Color> c)
    {
        HeightMap = h;
        ColorMap = c;
    }

    public void Dispose()
    {
        ColorMap.Dispose();
        HeightMap.Dispose();
    }
}
