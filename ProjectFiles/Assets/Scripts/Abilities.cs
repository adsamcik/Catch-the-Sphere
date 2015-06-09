using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Abilities : MonoBehaviour {
    CameraEffects CameraEffect;
    List<GameObject> colliding = new List<GameObject>();
    Transform parent;

    Score Score;

    bool touched;

    void Start() {
        CameraEffect = Camera.main.GetComponent<CameraEffects>();
        parent = gameObject.transform.parent;
        SphereSettings();
    }

    void SphereSettings() {
        if (gameObject.name == "LowerScore") { Score = GameObject.Find("Score").GetComponent<Score>(); StartCoroutine("DestroyIn", Random.Range(6, 10)); }
        else if (gameObject.name == "Teleport") { StartCoroutine("Teleporting"); }
    }
    void OnTriggerEnter(Collider other) {
        if (other.tag == "Sphere") colliding.Add(other.gameObject);
    }

    void OnTriggerExit(Collider other) {
        colliding.Remove(other.gameObject);
        Debug.Log("Exit");
    }

    IEnumerator DestroyIn(float time) {
        int scoretoadd = 0;
        int previous = 0;
        int scoretemp;
        while (time > 0) {
            scoretemp = Score.scoreTemp;
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
        parent.GetComponent<Collider>().enabled = false;
        Destroy(parent.GetComponent<Rigidbody>());
        CameraEffect.ShakeCamera(0.25f);
        foreach (GameObject sphere in colliding) {
            sphere.GetComponent<Rigidbody>().AddExplosionForce(2000, gameObject.transform.position, 20f);
        }

        MeshRenderer mr = parent.GetComponent<MeshRenderer>();
        Material m = new Material(mr.material);
        mr.material = m;

        for (float i = m.GetVector("_ChannelFactor").x; i < 2; i += Time.deltaTime) {
            m.SetFloat("_Displacement", i);
            m.SetVector("_ChannelFactor", new Vector4(i, i, i, 1));
            yield return new WaitForEndOfFrame();
        }
        Destroy(parent.gameObject);
    }

    IEnumerator GravityField() {
        gameObject.GetComponent<Collider>().enabled = false;
        Transform parent = gameObject.transform.parent;
        parent.GetComponent<Collider>().enabled = false;
        parent.GetComponent<Renderer>().enabled = false;
        parent.GetComponent<Rigidbody>().isKinematic = true;
        parent.GetComponent<Rigidbody>().velocity = new Vector3(0,-1,0);
        StartCoroutine("SpriteField", gameObject.GetComponentInChildren<SpriteRenderer>());

        foreach (GameObject sphere in colliding) {
            if (sphere) sphere.GetComponent<Move>().Freeze();
        }

        yield return new WaitForSeconds(1.5f);

        while (GameController.paused) yield return new WaitForFixedUpdate();

        foreach (GameObject sphere in colliding) {
            if (sphere) sphere.GetComponent<Move>().Freeze();
        }
        Destroy(parent.gameObject);
    }

    IEnumerator LowerScore() {
        parent.GetComponent<Collider>().enabled = false;
        parent.GetComponent<Renderer>().enabled = false;
        Vector3 velocity = parent.GetComponent<Rigidbody>().velocity;
        GameController.AddScoreNoModifier(-((3 * velocity.y * 3 * velocity.x) + Mathf.Pow(parent.position.y, 3)));
        GetComponent<ParticleSystem>().Emit(100);
        CameraEffect.ShakeCamera(0.5f);

        yield return new WaitForSeconds(1);
        Destroy(parent.gameObject);
    }

    IEnumerator Teleporting() {
        float ttt = -1; // Time To Teleport
        float maxttt = 0.75f;
        float score = 0;
        Vector3 SphereRand;
        while (!touched) {
            SphereRand = Random.insideUnitSphere * 5;
            if (ttt < 0) { parent.position = new Vector3(SphereRand.x, SphereRand.y + 6, SphereRand.z); ttt = maxttt;}
            ttt -= Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        score = 3500 * (ttt / maxttt);
        GameController.AddScore(score);
        Destroy(parent.gameObject);
    }

    IEnumerator Teleport() {
        touched = true;
        Destroy(parent.gameObject);
        yield return null;
    }

    IEnumerator Invisibility() {
        GameController.AddScoreNoModifier(2 * Mathf.Pow(parent.position.y, 3));
        Destroy(parent.gameObject);
        yield return null;
    }

    IEnumerator SpriteField(SpriteRenderer sprite) {
        sprite.transform.position = new Vector3(transform.position.x, 0.1f, transform.position.z);
        gameObject.transform.parent.localRotation = new Quaternion();
        while (sprite.transform.localScale.x < 0.2f) {
            sprite.transform.localScale += new Vector3(Time.deltaTime * 4, Time.deltaTime * 4, 0);
            yield return new WaitForFixedUpdate();
        }
    }
    public void Activate() {
        StartCoroutine(gameObject.name);
    }
}
