using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEditor.SceneManagement;
using UnityEditorInternal;
using Abilities;
using System.Linq;
using System.IO;
using System.Collections.Generic;

[CustomEditor(typeof(GameController))]
public class GameControllerEditor : Editor {
    ReorderableList abilityList = null;

    void OnEnable() {
        var obj = (GameController)target;

        obj.Initialize();

        abilityList = new ReorderableList(obj.abilities, typeof(AbilityInfo), true, true, false, false);

        Save();

        // Add listeners to draw events
        abilityList.drawHeaderCallback += DrawHeader;
        abilityList.drawElementCallback += DrawElement;
    }

    private void OnDisable() {
        // Make sure we don't get memory leaks etc.
        if (abilityList != null) {
            abilityList.drawHeaderCallback -= DrawHeader;
            abilityList.drawElementCallback -= DrawElement;
        }
    }

    private void DrawHeader(Rect rect) {
        GUI.Label(rect, "Abilities");
    }

    private void DrawElement(Rect rect, int index, bool active, bool focused) {
        AbilityInfo item = ((GameController)target).abilities[index];

        EditorGUI.BeginChangeCheck();
        item.enabled = EditorGUI.Toggle(new Rect(rect.x, rect.y, 15, rect.height), item.enabled);
        EditorGUI.LabelField(new Rect(rect.x + 15, rect.y, rect.width - 30, rect.height), item.abilityName);
        item.chanceToSpawn = EditorGUI.FloatField(new Rect(rect.width - 30, rect.y, 30, rect.height), item.chanceToSpawn);
        if (EditorGUI.EndChangeCheck()) {
            Save();
        }
    }


    public override void OnInspectorGUI() {
        //base.OnInspectorGUI();
        var obj = (GameController)target;
        if (Application.isPlaying)
            EditorGUILayout.LabelField("Spawned", obj.spawned.ToString());
        obj._sun = (Light)EditorGUILayout.ObjectField("Sun", obj._sun, typeof(Light), true);
        obj.finalResults = (Text)EditorGUILayout.ObjectField("Final result", obj.finalResults, typeof(Text), true);
        obj.pauseMenu = (GameObject)EditorGUILayout.ObjectField("Pause menu", obj.pauseMenu, typeof(GameObject), true);
        abilityList.DoLayoutList();
        if (GUI.changed) {
            Save();
        }
    }

    void Save() {
        StreamWriter sw = new StreamWriter(GameController.ABILITY_FILE);
        sw.Write(Newtonsoft.Json.JsonConvert.SerializeObject(((GameController)target).abilities));
        /*sw.Write("[");
        foreach (var item in ((GameController)target).abilities)
            sw.Write(item.ToJson());
        sw.Write("]");*/
        sw.Close();
    }

}
