using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class AnimationExplosion : MonoBehaviour {
    public float loopduration;
    float time;

	
	void Update () {
        time += Time.deltaTime / loopduration;
        float r = Mathf.Sin(time * (2 * Mathf.PI)) * 0.5f + 0.25f;
        float g = Mathf.Sin((time + 0.33333333f) * 2 * Mathf.PI) * 0.5f + 0.25f;
        float b = Mathf.Sin((time + 0.66666667f) * 2 * Mathf.PI) * 0.5f + 0.25f;
        float correction = 1 / (r + g + b);
        r *= correction;
        g *= correction;
        b *= correction; 
        GetComponent<Renderer>().sharedMaterial.SetVector("_ChannelFactor", new Vector4(r, g, b, 0));
    }
}
