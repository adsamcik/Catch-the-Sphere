﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Abilities : MonoBehaviour
{
    CameraEffects CameraEffect;
    GameController GameController;
    List<GameObject> colliding = new List<GameObject>();
    Transform parent;

    Score Score;

    bool touched;

    void Start() {
        CameraEffect = Camera.main.GetComponent<CameraEffects>();
        parent = gameObject.transform.parent;
        GameController = GameObject.Find("GameController").GetComponent<GameController>();
        SphereSettings();
    }

    void SphereSettings() {
        if (gameObject.name == "LowerScore") { Score = GameObject.Find("Score").GetComponent<Score>(); StartCoroutine("DestroyIn", Random.Range(7,15)); }
        else if (gameObject.name == "Teleport") { StartCoroutine("Teleporting"); }
    }
    void OnTriggerEnter(Collider other) {
         if(other.tag == "Sphere") colliding.Add(other.gameObject);
    }

    void OnTriggerExit(Collider other) {
        colliding.Remove(other.gameObject);
    }

    IEnumerator DestroyIn(float time) {
        int scoretoadd = 0;
        int previous = 0;
        int scoretemp;
        while (time > 0) {
            scoretemp = Score.scoretemp;
            if (scoretemp > previous) scoretoadd = scoretemp;
            else if (scoretemp < previous) { scoretoadd += previous; previous = 0; }
            time -= 0.5f;
            yield return new WaitForSeconds(0.5f);
        }

        Score.AddScoreNoModifier(scoretoadd + previous);
        GameController.destroyed++;

        Destroy(parent.gameObject);
    }

    IEnumerator Explosion() {
        parent.collider.enabled = false;
        parent.renderer.enabled = false;
        GetComponent<ParticleSystem>().Emit(100);
        CameraEffect.ShakeCamera(0.25f);
        foreach (GameObject sphere in colliding)
        {
            sphere.rigidbody.AddExplosionForce(2000, gameObject.transform.position, 20f);
        }
        yield return new WaitForSeconds(1);
        Destroy(parent.gameObject);
    }

    IEnumerator GravityField() {
        gameObject.collider.enabled = false;
        Transform parent = gameObject.transform.parent;
        parent.rigidbody.freezeRotation = true;
        parent.collider.enabled = false;
        parent.renderer.enabled = false;
        parent.rigidbody.isKinematic = true;
        StartCoroutine("SpriteField",gameObject.GetComponentInChildren<SpriteRenderer>());

		foreach (GameObject sphere in colliding) {
            if(sphere) sphere.GetComponent<Move>().Freeze();
		}

        yield return new WaitForSeconds(1.5f);

        while (GameController.paused) yield return new WaitForFixedUpdate();
        foreach (GameObject sphere in colliding)
        {
            if (sphere) sphere.GetComponent<Move>().Freeze();
        }
        Destroy(parent.gameObject);
    }

    IEnumerator LowerScore() {
        parent.collider.enabled = false;
        parent.renderer.enabled = false;
        parent.GetComponent<Move>().GameController.AddScoreNoModifier(-((3*parent.rigidbody.velocity.y * 3*parent.rigidbody.velocity.x) + Mathf.Pow(parent.position.y, 3)));
        GetComponent<ParticleSystem>().Emit(100);
        CameraEffect.ShakeCamera(0.5f);

        yield return new WaitForSeconds(1);
        Destroy(parent.gameObject);
    }

    IEnumerator Teleporting() {
        float ttt = -1; // Time To Teleport
        float maxttt = 0;
        float score=0;
        Vector3 SphereRand;
        while (!touched) {
            SphereRand = Random.insideUnitSphere * 5;
            if (ttt < 0) { parent.position = new Vector3(SphereRand.x, SphereRand.y + 6, SphereRand.z); ttt = 0.75f; maxttt = ttt; score = 1000 / ttt;}
            score = 2000 * (ttt/maxttt);
            ttt -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        score = score / (maxttt - 0.25f);
        parent.GetComponent<Move>().GameController.AddScoreNoModifier(score);
        Destroy(parent.gameObject);
    }

    IEnumerator Teleport() {
        touched = true;
        yield return null;
    }

    IEnumerator Invisibility() {
        parent.GetComponent<Move>().GameController.AddScoreNoModifier(2*Mathf.Pow(parent.position.y,3));
        yield return null;
    }

    IEnumerator SpriteField(SpriteRenderer sprite) {
        sprite.transform.position = new Vector3(transform.position.x,0.1f,transform.position.z);
        gameObject.transform.parent.localRotation = new Quaternion();
        while (sprite.transform.localScale.x < 0.2f) { 
            sprite.transform.localScale += new Vector3(Time.deltaTime*4, Time.deltaTime*4,0);
            yield return new WaitForFixedUpdate();
        }
    }
    public void Activate() {
        StartCoroutine(gameObject.name);
    }
}