using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.Linq;
using UnityEngine.UI;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(GameController))]
public class GameControllerEditor : Editor {
    void OnEnable() {
        var obj = (GameController)target;
        obj.Awake();
    }

    bool toggle = true;
    public override void OnInspectorGUI() {
        //base.OnInspectorGUI();
        var obj = (GameController)target;
        if (Application.isPlaying) {
            EditorGUILayout.LabelField("Spawned", obj.spawned.ToString());
        }
        obj._sun = (Light)EditorGUILayout.ObjectField("Sun", obj._sun, typeof(Light), true);
        obj.finalResults = (Text)EditorGUILayout.ObjectField("Final result", obj.finalResults, typeof(Text), true);
        obj.pauseMenu = (GameObject)EditorGUILayout.ObjectField("Pause menu", obj.pauseMenu, typeof(GameObject), true);
        toggle = EditorGUILayout.Foldout(toggle, "Abilities");
        if (toggle) {
            for (int i = 0; i < obj.AbilitySpheres.Count; i++) {
                Pair<Ability, bool> a = obj.AbilitySpheres[i];
                a.second = EditorGUILayout.Toggle(a.first.GetType().Name, a.second);
            }
        }
        if (GUI.changed) { EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene()); }
    }

}
