using UnityEngine;
using System.Collections;

public class MenuBall : MonoBehaviour {
	void Start () {
        rigidbody.AddExplosionForce(Random.Range(-250,-1000), new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1)), 100f);
        rigidbody.useGravity = true;   
        StartCoroutine("Count");
	}
    IEnumerator Count() {
        while (true)
        {
            if (Mathf.Abs(transform.position.x) > 5 || Mathf.Abs(transform.position.z) > 8)
            {
                Instantiate(Resources.Load("MenuSphere"), new Vector3(3,2,5), new Quaternion());
                Destroy(gameObject);
            }
            yield return new WaitForFixedUpdate();
        }
        
    }
}
