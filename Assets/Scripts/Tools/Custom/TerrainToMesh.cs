using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainToMesh : MonoBehaviour
{
    [SerializeField] private Terrain terrain;
    [SerializeField] private GameObject copyObj;

#if UNITY_EDITOR
    public void CopyTerrain()
    {
        var mf = copyObj.GetComponent<MeshFilter>();
        var m = mf.sharedMesh;

        List<Vector3> newVerts = new();

        foreach (var vert in m.vertices)
        {
            var wPos = copyObj.transform.localToWorldMatrix * vert;
            var newVert = vert;

            newVert.y = terrain.SampleHeight(wPos);
            newVerts.Add(newVert);
        }

        m.SetVertices(newVerts.ToArray());

        m.RecalculateNormals();
        m.RecalculateTangents();
        m.RecalculateBounds();
    }
#endif
}
