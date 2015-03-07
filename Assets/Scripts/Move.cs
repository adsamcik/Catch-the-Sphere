using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour
{


    RaycastHit hit;
	public GameController GameController;
    Abilities Abilities;

    Vector3 Velocity;
    bool paused;
    public bool frozen { get; private set; }
    float punish;
    void Start() {
        GameController = GameObject.Find("GameController").GetComponent<GameController>();
        Abilities = GetComponentInChildren<Abilities>(); 
		rigidbody.drag = Random.Range(0, 2); 
        StartCoroutine("IsInside"); 
	}

    IEnumerator IsInside()
    {
        while (true)
        {
            if (Mathf.Abs(transform.position.x) > 8 || Mathf.Abs(transform.position.z) > 8 || transform.position.y < -1) { Instantiate(gameObject, new Vector3(0, 6, 0), new Quaternion()); Destroy(gameObject); }
            yield return new WaitForSeconds(1);
        }

    }


    public void Touched()
    {
        StopCoroutine("IsInside");
        if (frozen) GameController.AddScore(20 * (Mathf.Abs(Velocity.x) + Mathf.Abs(Velocity.z)));
        else GameController.AddScore(10 * (Mathf.Abs(rigidbody.velocity.x) + Mathf.Abs(rigidbody.velocity.z) + Mathf.Abs(rigidbody.velocity.y / 2f)));
        GameController.destroyed++;
        if (Abilities) { GetComponent<SphereCollider>().enabled = false; Abilities.Activate(); }
        else StartCoroutine("Puff");
    }

    IEnumerator Puff() {
        collider.enabled = false;
        for (float i = 0.00f; i < 0.1f; i += Time.deltaTime) {
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

    public void Pause()
    {
        paused = !paused;
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
