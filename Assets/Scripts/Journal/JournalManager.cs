using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using Abilities;

public class JournalManager : MonoBehaviour {
    private const string ABILITY_FILE = GameController.ABILITY_FILE;
    private const float SPHERE_SCALE = 4;

    List<AbilityInfo> abilities = new List<AbilityInfo>();

    Text title, description;
    GameObject sphere;
    SphereController controller;
    SphereStats stats;
    int currentPosition;

    void LoadAbilities() {
        AbilityInfo[] abilities = JsonUtility.FromJson<AbilityInfoList>(Resources.Load<TextAsset>(ABILITY_FILE).text).array;
        foreach (var item in abilities) {
            if (item.enabled)
                this.abilities.Add(item);
        }
    }

    void Start() {
        LoadAbilities();
        GestureRecognition.OnSwipe += GestureRecognition_OnSwipe;
        sphere = (GameObject)Instantiate(Resources.Load<GameObject>("SphereMed"), Vector3.zero, Quaternion.Euler(90, 0, 0));
        Destroy(sphere.GetComponent<Rigidbody>());
        controller = sphere.GetComponent<SphereController>();
        controller.enabled = false;
        stats = sphere.GetComponent<SphereStats>();
        stats.enabled = false;
        sphere.transform.localScale = new Vector3(SPHERE_SCALE, SPHERE_SCALE, SPHERE_SCALE);
        Transform panel = GameObject.Find("Canvas/Panel").transform;
        title = panel.Find("Title").GetComponent<Text>();
        description = panel.Find("Description").GetComponent<Text>();
        Load(0);
    }

    void Update() {
        sphere.transform.Rotate(Vector3.forward, Time.deltaTime * 10);
    }

    private void GestureRecognition_OnSwipe(GestureRecognition.SwipeDirection dir) {
        switch (dir) {
            case GestureRecognition.SwipeDirection.Up:
                break;
            case GestureRecognition.SwipeDirection.Right:
                Next();
                break;
            case GestureRecognition.SwipeDirection.Down:
                break;
            case GestureRecognition.SwipeDirection.Left:
                Previous();
                break;
            default:
                break;
        }
    }

    private void Next() {
        if (currentPosition != abilities.Count - 1)
            Load(++currentPosition);
    }

    private void Previous() {
        if (currentPosition != 0)
            Load(--currentPosition);
    }

    private void Load(int position) {
        stats.RemoveAllAbilities();
        stats.AddAbility(abilities[position].ability.Clone());
        title.text = abilities[position].name;
        description.text = abilities[position].description;
    }
}
