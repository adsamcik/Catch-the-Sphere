using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.Linq;
using UnityEngine.UI;
using UnityEditor.SceneManagement;
using UnityEditorInternal;
using Abilities;

[CustomEditor(typeof(GameController))]
public class GameControllerEditor : Editor {
    ReorderableList abilityList;

    void OnEnable() {
        var obj = (GameController)target;
        obj.Awake();

        abilityList = new ReorderableList(obj.abilities, typeof(AbilityInfo), true, true, true, true);

        // This could be used aswell, but I only advise this your class inherrits from UnityEngine.Object or has a CustomPropertyDrawer
        // Since you'll find your item using: serializedObject.FindProperty("list").GetArrayElementAtIndex(index).objectReferenceValue
        // which is a UnityEngine.Object
        // reorderableList = new ReorderableList(serializedObject, serializedObject.FindProperty("list"), true, true, true, true);

        // Add listeners to draw events
        abilityList.drawHeaderCallback += DrawHeader;
        abilityList.drawElementCallback += DrawElement;

        abilityList.onAddCallback += AddItem;
        abilityList.onRemoveCallback += RemoveItem;
    }

    private void OnDisable() {
        // Make sure we don't get memory leaks etc.
        abilityList.drawHeaderCallback -= DrawHeader;
        abilityList.drawElementCallback -= DrawElement;

        abilityList.onAddCallback -= AddItem;
        abilityList.onRemoveCallback -= RemoveItem;
    }

    /// <summary>
    /// Draws the header of the list
    /// </summary>
    /// <param name="rect"></param>
    private void DrawHeader(Rect rect) {
        GUI.Label(rect, "Our fancy reorderable list");
    }

    /// <summary>
    /// Draws one element of the list (ListItemExample)
    /// </summary>
    /// <param name="rect"></param>
    /// <param name="index"></param>
    /// <param name="active"></param>
    /// <param name="focused"></param>
    private void DrawElement(Rect rect, int index, bool active, bool focused) {
        AbilityInfo item = ((GameController)target).abilities[index];

        EditorGUI.BeginChangeCheck();
        item.enabled = EditorGUI.Toggle(new Rect(rect.x, rect.y, 15, rect.height), item.enabled);
        EditorGUI.LabelField(new Rect(rect.x + 15, rect.y, rect.width - 30, rect.height), item.ability.GetType().Name);
        item.chanceToSpawn = EditorGUI.FloatField(new Rect(rect.width - 30, rect.y, 30, rect.height), item.chanceToSpawn);
        if (EditorGUI.EndChangeCheck()) {
            EditorUtility.SetDirty(target);
        }

        // If you are using a custom PropertyDrawer, this is probably better
        // EditorGUI.PropertyField(rect, serializedObject.FindProperty("list").GetArrayElementAtIndex(index));
        // Although it is probably smart to cach the list as a private variable ;)
    }

    private void AddItem(ReorderableList list) {
        ((GameController)target).abilities.Add(new AbilityInfo());

        EditorUtility.SetDirty(target);
    }

    private void RemoveItem(ReorderableList list) {
        ((GameController)target).abilities.RemoveAt(list.index);

        EditorUtility.SetDirty(target);
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
        abilityList.DoLayoutList();
        if (GUI.changed) { EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene()); }
    }

}
