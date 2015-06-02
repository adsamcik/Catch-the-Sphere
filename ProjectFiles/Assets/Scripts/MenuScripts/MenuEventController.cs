using UnityEngine;
using System.Collections;

public class MenuEventController : MonoBehaviour {

	public void LoadLevel(string name) {
		Application.LoadLevel(name);
	}
}
