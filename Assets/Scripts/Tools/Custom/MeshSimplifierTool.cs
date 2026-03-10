using System.Collections;
using UnityEngine;
using UnityMeshSimplifier;

public class MeshSimplifierTool : MonoBehaviour
{
    public float quality = 0.5f;
    [SerializeField] MeshFilter[] meshList;

    public void SimplifyMeshes()
    {
        for (int i = 0; i < meshList.Length; i++)
        {
            var simplifier = new UnityMeshSimplifier.MeshSimplifier();
            simplifier.Initialize(meshList[i].sharedMesh);
            simplifier.SimplifyMesh(quality);

            var destMesh = simplifier.ToMesh();
            meshList[i].sharedMesh = destMesh;
        }
    }
}