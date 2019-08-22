using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TerrainGenerator))]
public class TerrainGenerator_Editor : Editor
{

    //Variable for keeping track of the state of the system so we can have dynamic UI.
    private int stage = 0;

    public DropData _dropData;
    
    public override void OnInspectorGUI()
    {
        TerrainGenerator TerGen = (TerrainGenerator) target;

        EditorGUILayout.BeginVertical(GUILayout.MinHeight(110f));
        if (stage == 0)
        {
            //Draw all the sliders, and monitor them for changes.
            EditorGUI.BeginChangeCheck();
            TerGen.mapWidth = EditorGUILayout.IntSlider("Map Width:", TerGen.mapWidth, 1, 250);
            TerGen.mapHeight = EditorGUILayout.IntSlider("Map Height:", TerGen.mapHeight, 1, 250);
            TerGen.noiseScale = EditorGUILayout.Slider("Noise Scale:", TerGen.noiseScale, 0.01f, 60f);
            TerGen.noiseStrength = EditorGUILayout.Slider("Noise Strength:", TerGen.noiseStrength, 0.001f, 100f);
            TerGen.octaves = EditorGUILayout.IntSlider("Octaves:", TerGen.octaves, 1, 10);
            TerGen.lacunarity = EditorGUILayout.Slider("Lacunarity:", TerGen.lacunarity, 1f, 16f);
            //If any sliders were changed, tell the map to refresh.
            if (EditorGUI.EndChangeCheck())
            {
                TerGen.RefreshMap();
            }
        }

        if (stage == 1)
        {
            TerGen.erosionIterations = EditorGUILayout.IntField("Iteration Count:", TerGen.erosionIterations);
            TerGen.erosionSettings = EditorGUILayout.ObjectField("Drop Data:", TerGen.erosionSettings, typeof(ScriptableObject), false) as DropData;
            if (GUILayout.Button("Simulate"))
            {
                TerGen.BeginSim();
            }

            if (GUILayout.Button("Revert"))
            {
                TerGen.RefreshMap();
            }

        }
        EditorGUILayout.EndVertical();

        /*
        if (DrawDefaultInspector() && stage == 1)
        {
            TerGen.RefreshMap();
        }

        if (GUILayout.Button("Refresh"))
        {
            TerGen.RefreshMap();
        }

        if (GUILayout.Button("Simulate Erosion"))
        {
            TerGen.BeginSim();
        }
        */
        
        

        GUILayout.BeginHorizontal();
            EditorGUI.BeginDisabledGroup(stage == 0);
            if (GUILayout.Button("Back"))
            {
                stage--;
            }
            EditorGUI.EndDisabledGroup();
        
            EditorGUI.BeginDisabledGroup(stage == 1);
            if (GUILayout.Button("Next"))
            {
                stage++;
            }
            EditorGUI.EndDisabledGroup();
        GUILayout.EndHorizontal();
        

    }
}