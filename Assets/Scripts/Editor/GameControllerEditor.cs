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
    void OnEnable() {
        var obj = (GameController)target;

        obj.Initialize();

        if (!Application.isPlaying) {
            if (obj.abilities != null) {
                bool removed = false;
                for (int i = 0; i < obj.abilities.Count; i++) {
                    if (obj.abilities[i].ability == null) {
                        obj.abilities.RemoveAt(i--);
                        removed = true;
                    }
                }
                IEnumerable<Ability> newAbilities;
                newAbilities = GameController.abilityList.Where(x => obj.abilities.FirstOrDefault(y => x.GetType() == y.ability.GetType()) == default(AbilityInfo));
                foreach (var a in newAbilities)
                    obj.abilities.Add(new AbilityInfo(a, 1, true));
                if (newAbilities.Count() > 0 || removed)
                    Save();
            } else {
                foreach (var a in GameController.abilityList)
                    obj.abilities.Add(new AbilityInfo(a, 1, true));
                Save();
            }
        }
    }

    private void DrawElement(AbilityInfo item, Rect rect) {
        const int LINE_HEIGHT = 17;
        EditorGUI.BeginChangeCheck();
        float offset = 0;
        if (!Application.isPlaying) {
            item.enabled = EditorGUI.Toggle(new Rect(rect.x, rect.y, 15, LINE_HEIGHT), item.enabled);
            offset = 15;
        }
        EditorGUI.LabelField(new Rect(rect.x + offset, rect.y, rect.width - 30, LINE_HEIGHT), item.name);
        item.chanceToSpawn = EditorGUI.FloatField(new Rect(rect.width - 30, rect.y, 30, LINE_HEIGHT), item.chanceToSpawn);
        item.description = EditorGUI.TextField(new Rect(rect.x, rect.y + LINE_HEIGHT, rect.width, LINE_HEIGHT), item.description);
        if (EditorGUI.EndChangeCheck())
            Save();
    }


    public override void OnInspectorGUI() {
        //base.OnInspectorGUI();
        var obj = (GameController)target;
        if (Application.isPlaying)
            EditorGUILayout.LabelField("Spawned", obj.spawned.ToString());
        //obj._sun = (Light)EditorGUILayout.ObjectField("Sun", obj._sun, typeof(Light), true);
        obj.pauseMenu = (GameObject)EditorGUILayout.ObjectField("Pause menu", obj.pauseMenu, typeof(GameObject), true);
        foreach (var item in obj.abilities) {
            DrawElement(item, EditorGUILayout.GetControlRect(false, 40));
        }
        if (GUI.changed) {
            Save();
        }
    }

    void Save() {
        StreamWriter sw = new StreamWriter("Assets/Resources/" + GameController.ABILITY_FILE + ".json");
        sw.Write(JsonUtility.ToJson(new AbilityInfoList(((GameController)target).abilities)));
        sw.Dispose();
        AssetDatabase.Refresh();
    }

}
