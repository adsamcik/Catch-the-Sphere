using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Stats))]
public class StatsEditor : Editor {

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        Stats s = (Stats)target;
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Abilities", new GUIStyle() { fontSize = 17 });
        foreach (var item in s.abilities) {
            EditorGUILayout.LabelField(item.GetType().Name);
        }
    }
}
