using UnityEngine;
using System.Collections;

public class TutSphereController : MonoBehaviour
{

/*
    RaycastHit hit;
    TutorialText tutorialText;
    Abilities Abilities;

    Vector3 Velocity;
    bool frozen;
    float punish;
    public int phase = 1;

    void Start()
    {
        tutorialText = GameObject.Find("Text").GetComponent<TutorialText>();
        Abilities = GetComponentInChildren<Abilities>();
        GetComponent<Rigidbody>().drag = Random.Range(0, 2);
    }

    public void Touched() {
        StartCoroutine("Phase"+phase);
    }

    IEnumerator Phase0() {
        yield return null;
    }


    IEnumerator Phase1()
    {
        StartCoroutine("Puff");
        yield return null;
    }

    IEnumerator Phase2()
    {
        tutorialText.AddScore((10) * (Mathf.Abs(GetComponent<Rigidbody>().velocity.x) + Mathf.Abs(GetComponent<Rigidbody>().velocity.z)));
        tutorialText.destroyed++;
        StartCoroutine("Puff");
        yield return null;
    }

    IEnumerator Phase3()
    {
        tutorialText.AddScore((10) * (Mathf.Abs(GetComponent<Rigidbody>().velocity.x) + Mathf.Abs(GetComponent<Rigidbody>().velocity.z)));
        tutorialText.destroyed++;
        GetComponent<SphereCollider>().enabled = false; Abilities.Activate();
        yield return null;
    }

    IEnumerator Puff()
    {
        GetComponent<Collider>().enabled = false;
        for (float i = 0.00f; i < 0.1f; i += Time.deltaTime)
        {
            transform.localScale -= new Vector3(Time.deltaTime * 10, Time.deltaTime * 10, Time.deltaTime * 10);
            yield return new WaitForFixedUpdate();
        }
        Destroy(gameObject);
    }

    public void Freeze()
    {
        if (frozen == true)
        {
            GetComponent<Rigidbody>().isKinematic = false;
            GetComponent<Rigidbody>().velocity = Velocity;
            frozen = false;
        }
        else
        {
            Velocity = GetComponent<Rigidbody>().velocity;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().isKinematic = true;
            frozen = true;
        }
    }
    */
}
