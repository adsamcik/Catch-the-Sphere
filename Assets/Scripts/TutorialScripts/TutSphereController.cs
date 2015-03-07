using UnityEngine;
using System.Collections;

public class TutSphereController : MonoBehaviour
{


    RaycastHit hit;
    Text GameController;
    Abilities Abilities;

    Vector3 Velocity;
    bool frozen;
    float punish;
    public int phase = 1;

    void Start()
    {
        GameController = GameObject.Find("Text").GetComponent<Text>();
        Abilities = GetComponentInChildren<Abilities>();
        rigidbody.drag = Random.Range(0, 2);
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
        GameController.AddScore((10) * (Mathf.Abs(rigidbody.velocity.x) + Mathf.Abs(rigidbody.velocity.z)));
        GameController.destroyed++;
        StartCoroutine("Puff");
        yield return null;
    }

    IEnumerator Phase3()
    {
        GameController.AddScore((10) * (Mathf.Abs(rigidbody.velocity.x) + Mathf.Abs(rigidbody.velocity.z)));
        GameController.destroyed++;
        GetComponent<SphereCollider>().enabled = false; Abilities.Activate();
        yield return null;
    }

    IEnumerator Puff()
    {
        collider.enabled = false;
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
            rigidbody.isKinematic = false;
            rigidbody.velocity = Velocity;
            frozen = false;
        }
        else
        {
            Velocity = rigidbody.velocity;
            rigidbody.velocity = Vector3.zero;
            rigidbody.isKinematic = true;
            frozen = true;
        }
    }

}
