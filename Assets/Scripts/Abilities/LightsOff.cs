using UnityEngine;
using System.Collections;
using System;

public class LightsOff : Ability {
    float prevIntensity;
    public void FixedUpdate(Rigidbody rigidbody) {
        throw new NotImplementedException();
    }

    public void Initialize(GameObject g) {
        prevIntensity = RenderSettings.ambientIntensity;
        RenderSettings.ambientIntensity = 0;
        
    }

    public void OnFieldEnter(GameObject g) {
        throw new NotImplementedException();
    }

    public void OnFieldExit(GameObject g) {
        throw new NotImplementedException();
    }

    public int Pop() {
        throw new NotImplementedException();
    }

    public IEnumerator PopAnimation(Action func) {
        throw new NotImplementedException();
    }
}
