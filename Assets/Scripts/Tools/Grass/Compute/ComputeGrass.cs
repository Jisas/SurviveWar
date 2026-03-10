using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.UI.Image;

[RequireComponent(typeof(MeshFilter))]
[ExecuteInEditMode]
public class ComputeGrass : MonoBehaviour
{
    public Mesh mesh;
    MeshFilter filter;

    public Color AdjustedColor;

    [Range(1, 600000000)]
    public int grassLimit = 50000;

    //private Vector3 lastPosition = Vector3.zero;

    [SerializeField]
    List<Vector3> positions = new List<Vector3>();
    [SerializeField]
    List<Color> colors = new List<Color>();
    [SerializeField]
    List<int> indicies = new List<int>();
    [SerializeField]
    List<Vector3> normals = new List<Vector3>();
    [SerializeField]
    List<Vector2> length = new List<Vector2>();

    public float sizeWidth = 1f;
    public float sizeLength = 1f;
    public float density = 1f;

    public float normalLimit = 1;

    public float range;
    public float rangeR, rangeG, rangeB;

    public int i = 0;
    int[] indi;

#if UNITY_EDITOR

    void OnEnable()
    {
        filter = GetComponent<MeshFilter>();
    }

    public void ClearMesh()
    {
        i = 0;
        positions = new List<Vector3>();
        indicies = new List<int>();
        colors = new List<Color>();
        normals = new List<Vector3>();
        length = new List<Vector2>();
    }

    public void AddGrass()
    {
        for (int v = 0; v < filter.sharedMesh.vertexCount; v++)
        {
            // place based on density
            for (int k = 0; k < density; k++)
            {                                            
                if (k != 0)
                {
                    // brushrange
                    float t = 2f * Mathf.PI * Random.Range(0f, range);
                    float u = Random.Range(0f, range) + Random.Range(0f, range);
                    float r = (u > 1 ? 2 - u : u);

                    Vector3 origin = Vector3.zero;

                    // place random in radius, except for first one
                    if (k != 0)
                    {
                        origin.x += r * Mathf.Cos(t);
                        origin.y += r * Mathf.Sin(t);
                    }
                    else
                    {
                        origin = Vector3.zero;
                    }

                    var grassPosition = filter.sharedMesh.vertices[v] + origin;
                    grassPosition -= this.transform.position;

                    positions.Add((grassPosition));
                    length.Add(new Vector2(sizeWidth, sizeLength));

                    // add random color variations                          
                    colors.Add(new Color(
                        AdjustedColor.r + (Random.Range(0, 1.0f) * rangeR),
                        AdjustedColor.g + (Random.Range(0, 1.0f) * rangeG),
                        AdjustedColor.b + (Random.Range(0, 1.0f) * rangeB),
                        1));

                    normals.Add(filter.sharedMesh.normals[v]);

                    indicies.Add(i);
                    i++;
                }
            }
        }
       
        //SetMesh();
    }

    void SetMesh()
    {
        // set all info to mesh
        mesh = filter.sharedMesh;
        //mesh.SetVertices(positions);
        indi = indicies.ToArray();
        mesh.SetIndices(indi, MeshTopology.Points, 0);
        mesh.SetUVs(0, length);
        mesh.SetColors(colors);
        mesh.SetNormals(normals);
    }
#endif
}
