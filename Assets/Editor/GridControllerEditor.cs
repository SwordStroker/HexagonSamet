using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GridController))]
public class GridControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GridController gridController = target as GridController;

        if (GUILayout.Button("Generate Grid"))
            gridController.CreateHexagonMap();

        if (GUILayout.Button("Clear Grid"))
            gridController.ClearGrid();
    }
}
