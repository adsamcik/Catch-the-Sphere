﻿using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class GravityField: Ability {
    List<GameObject> colliding = new List<GameObject>();
    GameObject gameObject;

    public void FixedUpdate(Rigidbody rigidbody) {
        throw new NotImplementedException();
    }

    public int Pop() {
        return 2000;
    }

    public IEnumerator PopAnimation() {
        GameController.AddScore(2000);
        gameObject.GetComponent<Collider>().enabled = false;
        Transform parent = gameObject.transform.parent;
        parent.GetComponent<Renderer>().enabled = false;
        parent.GetComponent<Rigidbody>().isKinematic = true;
        parent.GetComponent<Rigidbody>().velocity = new Vector3(0, -1, 0);
        //StartCoroutine("SpriteField", gameObject.GetComponentInChildren<SpriteRenderer>());

        foreach (GameObject sphere in colliding)
            if (sphere) sphere.GetComponent<Move>().ToggleFreeze();

        yield return new WaitForSeconds(1.5f);

        while (GameController.paused) yield return new WaitForFixedUpdate();

        foreach (GameObject sphere in colliding)
            if (sphere) sphere.GetComponent<Move>().ToggleFreeze();

        UnityEngine.Object.Destroy(parent.gameObject);
    }

    public void Initialize(GameObject g) {
        gameObject = g;
        g.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/Gravity");
    }

    public void OnFieldEnter(GameObject g) {
        throw new NotImplementedException();
    }

    public void OnFieldExit(GameObject g) {
        throw new NotImplementedException();
    }
}
