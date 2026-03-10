using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(MeshSimplifierTool))]
public class MeshSimplifierToolEditor : Editor
{
    MeshSimplifierTool simplifier;

    private void OnEnable()
    {
        simplifier = (MeshSimplifierTool)target;
    }

    public override void OnInspectorGUI()
    {
        simplifier.quality = EditorGUILayout.Slider("Quality", simplifier.quality, 0.2f, 1f);        
        EditorGUILayout.Space();

        serializedObject.Update();
        EditorGUILayout.LabelField("Meshes", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("meshList"));
        EditorGUILayout.EndHorizontal();
        serializedObject.ApplyModifiedProperties();
        EditorGUILayout.Space();

        if (GUILayout.Button("Simplify Meshes"))
        {
            simplifier.SimplifyMeshes();
        }
    }
}