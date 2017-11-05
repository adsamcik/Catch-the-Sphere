using UnityEngine;
using System.Collections;

public class FollowShadow : MonoBehaviour {
    SpriteRenderer Sprite;
    Transform Follow;

    void Start() {
        Follow = gameObject.transform.parent.Find("Invisible").transform;
        Sprite = GetComponent<SpriteRenderer>();
    }

	void Update () {
        transform.position = new Vector3(Follow.position.x,Follow.position.y-0.6f,Follow.position.z);
        //float scale = 0.6f + ((transform.position.y - 1) / 20);
        float opacity = 0.5f - ((transform.position.y - 1) / 40);
        Sprite.color = new Color(0,0,0,opacity);
	}
}
