using UnityEngine;
using System.Collections;

public class HighscoreSet : MonoBehaviour {

	// Use this for initialization
	void Start () {
        if(!GetComponent<TextMesh>()) Debug.LogError("Object " + gameObject.name + " is not a Text Mesh.");
        GetComponent<TextMesh>().text = "highscore " + PlayerPrefs.GetInt("hs_normal");
        Debug.Log(PlayerPrefs.HasKey("hs_normal"));
	}
	
}
