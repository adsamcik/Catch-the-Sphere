using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(SphereStats))]
public class StatsEditor : Editor {

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        SphereStats s = (SphereStats)target;
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Abilities", new GUIStyle() { fontSize = 17 });
        foreach (var item in s.abilities) {
            EditorGUILayout.LabelField(item.GetType().Name);
        }
    }
}
