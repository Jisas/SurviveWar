using UnityEditor;
using UnityEditorInternal;
using UnityEngine;


[CustomEditor(typeof(ObjectsPainter))]
public class ObjectPainterEditor : Editor
{
    public ObjectsPainter treePainter;
    readonly string[] toolbarStrings = { "Add", "Remove" };

    void OnEnable()
    {
        treePainter = (ObjectsPainter)target;
        treePainter.toolbarInt = 0;
    }

    void OnSceneGUI()
    {
        if (treePainter.toolbarInt == 0)
        {
            Handles.color = Color.cyan;
            Handles.DrawWireDisc(treePainter.hitPosGizmo, treePainter.hitNormal, treePainter.brushSize);
            Handles.color = new Color(0, 0.5f, 0.5f, 0.4f);
            Handles.DrawSolidDisc(treePainter.hitPosGizmo, treePainter.hitNormal, treePainter.brushSize);
        }
        if (treePainter.toolbarInt == 1)
        {
            Handles.color = Color.red;
            Handles.DrawWireDisc(treePainter.hitPosGizmo, treePainter.hitNormal, treePainter.brushSize);
            Handles.color = new Color(0.5f, 0f, 0f, 0.4f);
            Handles.DrawSolidDisc(treePainter.hitPosGizmo, treePainter.hitNormal, treePainter.brushSize);
        }
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.LabelField("Objects Limit", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(treePainter.i.ToString() + "/", EditorStyles.label);
        treePainter.treeLimit = EditorGUILayout.IntField(treePainter.treeLimit);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();

        serializedObject.Update();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("prefabs"));
        EditorGUILayout.EndHorizontal();
        serializedObject.ApplyModifiedProperties();
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Paint Status (Right-Mouse Button to paint)", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
        treePainter.toolbarInt = GUILayout.Toolbar(treePainter.toolbarInt, toolbarStrings);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Brush Settings", EditorStyles.boldLabel);
        LayerMask tempMask = EditorGUILayout.MaskField("Hit Mask", InternalEditorUtility.LayerMaskToConcatenatedLayersMask(treePainter.hitMask), InternalEditorUtility.layers);
        treePainter.hitMask = InternalEditorUtility.ConcatenatedLayersMaskToLayerMask(tempMask);
        LayerMask tempMask2 = EditorGUILayout.MaskField("Painting Mask", InternalEditorUtility.LayerMaskToConcatenatedLayersMask(treePainter.paintMask), InternalEditorUtility.layers);
        treePainter.paintMask = InternalEditorUtility.ConcatenatedLayersMaskToLayerMask(tempMask2);

        treePainter.brushSize = EditorGUILayout.Slider("Brush Size", treePainter.brushSize, 0.1f, 100f);
        treePainter.density = EditorGUILayout.Slider("Density", treePainter.density, 0.1f, 20f);
        treePainter.normalLimit = EditorGUILayout.Slider("Normal Limit", treePainter.normalLimit, 0f, 1f);
        EditorGUILayout.Space();

        if (GUILayout.Button("Clear Objects"))
        {
            if (EditorUtility.DisplayDialog("Clear Painted Objects?",
               "Are you sure you want to clear the objects?", "Clear", "Don't Clear"))
            {
                treePainter.ClearObjects();
            }
        }
    }
}
