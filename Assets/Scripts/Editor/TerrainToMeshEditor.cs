using Codice.CM.Common.Tree;
using System.Collections;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(TerrainToMesh))]
public class TerrainToMeshEditor : Editor
{
    TerrainToMesh meshTerrain;

    private void OnEnable()
    {
        meshTerrain = (TerrainToMesh)target;
    }

    public override void OnInspectorGUI()
    {

        serializedObject.Update();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("terrain"));
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("copyObj"));
        EditorGUILayout.EndHorizontal();
        serializedObject.ApplyModifiedProperties();
        EditorGUILayout.Space();

        if (GUILayout.Button("Copy Terrain"))
        {
            meshTerrain.CopyTerrain();
        }
    }
}