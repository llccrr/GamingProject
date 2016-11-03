using UnityEngine;
using System.Collections;
using UnityEditor;

//Permit to update the tileIntersectionSize in realTime
[CustomEditor(typeof(ProceduralMapGenerator))]
public class MapEditor : Editor {

    public override void OnInspectorGUI()
    {
		ProceduralMapGenerator map = (ProceduralMapGenerator)target;
		if (DrawDefaultInspector ()) {
			map.GenerateMap ();
		}

		if (GUILayout.Button ("Regénérer la MapEditor")) {
			map.GenerateMap ();
		}
    }
}
