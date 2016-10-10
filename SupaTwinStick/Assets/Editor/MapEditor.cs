using UnityEngine;
using System.Collections;
using UnityEditor;

//Permit to update the tileIntersectionSize in realTime
[CustomEditor(typeof(ProceduralMapGenerator))]
public class MapEditor : Editor {

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ProceduralMapGenerator map = (ProceduralMapGenerator)target;
        map.GenerateMap();
    }
}
