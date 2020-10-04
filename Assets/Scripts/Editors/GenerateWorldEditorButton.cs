using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WorldTerrain))]
public class GenerateWorldEditorButton : Editor
{
    public override void OnInspectorGUI()
    {
        WorldTerrain gen = (WorldTerrain)target;

        if (DrawDefaultInspector() && gen.autoUpdate)
        {
            gen.GenerateTerrain();
        }

        if (GUILayout.Button("Generate World"))
        {
            gen.GenerateTerrain();
        }
    }
}
