using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ComputeGrass))]
public class ComputeGrassEditor : Editor
{
    ComputeGrass grassAllocator;

    private void OnEnable()
    {
        grassAllocator = (ComputeGrass)target;
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.LabelField("Grass Limit", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(grassAllocator.i.ToString() + "/", EditorStyles.label);
        grassAllocator.grassLimit = EditorGUILayout.IntField(grassAllocator.grassLimit);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();

        GUILayout.Space(10);
        grassAllocator.range = EditorGUILayout.Slider("Brush Size", grassAllocator.range, 0.1f, 10f);
        EditorGUILayout.Space();

        grassAllocator.normalLimit = EditorGUILayout.Slider("Normal Limit", grassAllocator.normalLimit, 0f, 1f);
        grassAllocator.density = EditorGUILayout.Slider("Density", grassAllocator.density, 0.1f, 30f);
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Width and Length ", EditorStyles.boldLabel);
        grassAllocator.sizeWidth = EditorGUILayout.Slider("Grass Width", grassAllocator.sizeWidth, 0f, 2f);
        grassAllocator.sizeLength = EditorGUILayout.Slider("Grass Length", grassAllocator.sizeLength, 0f, 2f);
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Color", EditorStyles.boldLabel);
        grassAllocator.AdjustedColor = EditorGUILayout.ColorField("Brush Color", grassAllocator.AdjustedColor);
        EditorGUILayout.LabelField("Random Color Variation", EditorStyles.boldLabel);
        grassAllocator.rangeR = EditorGUILayout.Slider("Red", grassAllocator.rangeR, 0f, 1f);
        grassAllocator.rangeG = EditorGUILayout.Slider("Green", grassAllocator.rangeG, 0f, 1f);
        grassAllocator.rangeB = EditorGUILayout.Slider("Blue", grassAllocator.rangeB, 0f, 1f);

        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add Grass"))
        {
            grassAllocator.AddGrass();
        }

        if (GUILayout.Button("Clear Grass"))
        {
            if (EditorUtility.DisplayDialog("Clear Painted Grass?",
               "Are you sure you want to clear the grass?", "Clear", "Don't Clear"))
            {
                grassAllocator.ClearMesh();
            }
        }
        EditorGUILayout.EndHorizontal();
    }
}