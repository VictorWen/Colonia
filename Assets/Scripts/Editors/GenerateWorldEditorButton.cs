using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(World))]
public class GenerateWorldEditorButton : Editor
{
    public override void OnInspectorGUI()
    {
        World gen = (World)target;

        if (DrawDefaultInspector() && gen.autoUpdate)
        {
            gen.GenerateWorld();
        }

        if (GUILayout.Button("Generate World"))
        {
            gen.GenerateWorld();
        }
    }
}
