using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TerrainGenerator))]
public class TerrainGenerator_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        TerrainGenerator TerGen = (TerrainGenerator) target;
        
        
        if (DrawDefaultInspector())
        {
            TerGen.RefreshMap();
        }

        if (GUILayout.Button("Refresh"))
        {
            TerGen.RefreshMap();
        }
    }
}