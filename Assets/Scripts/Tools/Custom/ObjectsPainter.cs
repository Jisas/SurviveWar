using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class ObjectsPainter : MonoBehaviour
{
    // The array of objects to spawn.
    [SerializeField] private GameObject[] prefabs;

    [Header("Settings")]
    [Range(1, 1000000000)] public int treeLimit = 500000;

    public bool painting;
    public bool removing;

    public LayerMask hitMask = 1;
    public LayerMask paintMask = 1;
    public float brushSize;
    public float density = 1f;
    public float normalLimit = 1;

    [HideInInspector] public Vector3 hitPosGizmo;
    [HideInInspector] public Vector3 hitNormal;
    Vector3 hitPos;
    Vector3 mousePos;

    public int i = 0;
    public int toolbarInt;

    private List<GameObject> newbjects = new();

#if UNITY_EDITOR
    private void OnEnable()
    {
        SceneView.duringSceneGui -= this.OnScene;
        SceneView.duringSceneGui += this.OnScene;
    }

    private void OnDestroy()
    {
        // When the window is destroyed, remove the delegate
        // so that it will no longer do any drawing.
        SceneView.duringSceneGui -= this.OnScene;
    }

    public void ClearObjects()
    {
        for (int i = 0; i < newbjects.Count; i++)
        {
            DestroyImmediate(newbjects[i]);
        }

        newbjects.Clear();
    }

    // The function that is called to spawn the objects.
    public void OnScene(SceneView scene)
    {
        if (Selection.Contains(this.gameObject))
        {
            RaycastHit terrainHit;
            Event e = Event.current;
            mousePos = e.mousePosition;
            float ppp = EditorGUIUtility.pixelsPerPoint;
            mousePos.y = scene.camera.pixelHeight - mousePos.y * ppp;
            mousePos.x *= ppp;

            // ray for gizmo(disc)
            Ray rayGizmo = scene.camera.ScreenPointToRay(mousePos);

            if (Physics.Raycast(rayGizmo, out RaycastHit hitGizmo, Mathf.Infinity, hitMask.value))
            {
                hitPosGizmo = hitGizmo.point;
            }

            // add objects
            if (e.type == EventType.MouseDrag && e.button == 1 && toolbarInt == 0)
            {
                for (int k = 0; k < density; k++)
                {
                    // brushrange
                    float t = 1.5f * Mathf.PI * Random.Range(0f, brushSize);
                    float u = Random.Range(0f, brushSize) + Random.Range(0f, brushSize);
                    float r = (u > 1 ? 2 - u : u);
                    Vector3 origin = Vector3.zero;

                    // place random in radius, except for first one
                    origin.x += r * Mathf.Cos(t);
                    origin.y += r * Mathf.Sin(t);

                    // add random range to ray
                    Ray ray = scene.camera.ScreenPointToRay(mousePos);
                    ray.origin += origin;

                    if (Physics.Raycast(ray, out terrainHit, 200f, hitMask.value) && i < treeLimit && terrainHit.normal.y <= (1 + normalLimit) && terrainHit.normal.y >= (1 - normalLimit))
                    {
                        if ((paintMask.value & (1 << terrainHit.transform.gameObject.layer)) > 0)
                        {
                            hitPos = terrainHit.point;
                            var treePosition = hitPos;

                            var randomTree = Random.Range(0, prefabs.Length);

                            var obj = Instantiate(prefabs[randomTree]) as GameObject;
                            newbjects.Add(obj);

                            obj.transform.SetPositionAndRotation(treePosition, Quaternion.identity);
                        }
                    }
                }
                e.Use();
            }

            // removing objects
            if (e.type == EventType.MouseDrag && e.button == 1 && toolbarInt == 1)
            {
                Ray ray = scene.camera.ScreenPointToRay(mousePos);

                if (Physics.Raycast(ray, out terrainHit, 200f, hitMask.value))
                {
                    hitPos = terrainHit.point;
                    hitPosGizmo = hitPos;
                    hitNormal = terrainHit.normal;

                    for (int j = 0; j < newbjects.Count; j++)
                    {
                        Vector3 pos = newbjects[j].transform.position;

                        float dist = Vector3.Distance(terrainHit.point, pos);

                        // if its within the radius of the brush, remove all info
                        if (dist <= brushSize)
                        {
                            var obj = newbjects[j];
                            DestroyImmediate(obj);
                            newbjects.RemoveAt(j);
                        }
                    }
                }
                e.Use();
            }
        }
    }
#endif
}

