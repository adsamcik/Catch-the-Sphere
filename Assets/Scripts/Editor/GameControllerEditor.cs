using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEditor.SceneManagement;
using UnityEditorInternal;
using Abilities;
using System.Linq;

[CustomEditor(typeof(GameController))]
public class GameControllerEditor : Editor {
    ReorderableList abilityList;

    void OnEnable() {
        var obj = (GameController)target;

        var interfaceType = typeof(Ability);
        var all = System.AppDomain.CurrentDomain.GetAssemblies()
          .SelectMany(x => x.GetTypes())
          .Where(x => interfaceType.IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
          .Select(x => System.Activator.CreateInstance(x));
        foreach (var ability in all) {
            if(ability == null) {
                Debug.LogWarning("ability is null");
                continue;
            }
            if (obj.standard == null && ability.GetType() == typeof(Standard))
                obj.standard = (Standard)ability;
            else if (obj.abilities.FindIndex(x => x.ability.GetType() == ability.GetType()) == -1)
                obj.abilities.Add(new AbilityInfo((Ability)ability, 1, true));
        }

        abilityList = new ReorderableList(obj.abilities, typeof(AbilityInfo), true, true, false, false);

        // Add listeners to draw events
        abilityList.drawHeaderCallback += DrawHeader;
        abilityList.drawElementCallback += DrawElement;
    }

    private void OnDisable() {
        // Make sure we don't get memory leaks etc.
        abilityList.drawHeaderCallback -= DrawHeader;
        abilityList.drawElementCallback -= DrawElement;
    }

    private void DrawHeader(Rect rect) {
        GUI.Label(rect, "Abilities");
    }

    private void DrawElement(Rect rect, int index, bool active, bool focused) {
        AbilityInfo item = ((GameController)target).abilities[index];

        EditorGUI.BeginChangeCheck();
        item.enabled = EditorGUI.Toggle(new Rect(rect.x, rect.y, 15, rect.height), item.enabled);
        EditorGUI.LabelField(new Rect(rect.x + 15, rect.y, rect.width - 30, rect.height), item.ability.GetType().Name);
        item.chanceToSpawn = EditorGUI.FloatField(new Rect(rect.width - 30, rect.y, 30, rect.height), item.chanceToSpawn);
        if (EditorGUI.EndChangeCheck()) {
            EditorUtility.SetDirty(target);
            EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
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
            EditorUtility.SetDirty(target);
            EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
        }
    }

}
