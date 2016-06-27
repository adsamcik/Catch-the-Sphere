using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.Linq;

[CustomEditor(typeof(GameController))]
public class GameControllerEditor : Editor {
    void OnEnable() {
        var obj = (GameController)target;
        var interfaceType = typeof(Ability);
        var all = System.AppDomain.CurrentDomain.GetAssemblies()
          .SelectMany(x => x.GetTypes())
          .Where(x => interfaceType.IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
          .Select(x => System.Activator.CreateInstance(x));
        foreach (var ability in all) {
            if(obj.AbilitySpheres.FindIndex(x => x.first.GetType() == ability.GetType()) == -1) {
                obj.AbilitySpheres.Add(new Pair<Ability, bool>((Ability)ability, true));
            }
        }
    }

    public override void OnInspectorGUI() {
        //base.OnInspectorGUI();
        var obj = (GameController)target;
        EditorGUILayout.ObjectField("Sun", GameController.sun, typeof(Light), true);
        EditorGUILayout.ObjectField("Final result", obj.finalResults, typeof(GameObject), true);
        EditorGUILayout.IntField("Spawned", obj.spawned);
        EditorGUILayout.ObjectField("Pause menu", obj.pauseMenu, typeof(GameObject), true);
        for (int i = 0; i < obj.AbilitySpheres.Count; i++) {
            Pair<Ability, bool> a = obj.AbilitySpheres[i];  
            a.second = EditorGUILayout.Toggle(a.first.GetType().Name, a.second);
        }
    }

}
