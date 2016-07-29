using UnityEngine;
using System.Collections;

public class ObjectController : MonoBehaviour {
    const float MAX_SPEED = 20;
    const float MAX_SPEED_SQR = MAX_SPEED * MAX_SPEED;

    Rigidbody r;
    Stats s;
    Vector3 velocity;
    MeshRenderer mr;

    void Start() {
        StartCoroutine("IsInside");
        r = GetComponent<Rigidbody>();
        s = GetComponent<Stats>();
    }

    IEnumerator IsInside() {
        while (true) {
            if (Mathf.Abs(transform.position.x) > 8 || Mathf.Abs(transform.position.z) > 8 || transform.position.y < -1) {
                transform.position = new Vector3(0, 6, 0);
                r.velocity = Vector3.zero;
            }
            yield return new WaitForSeconds(1);
        }
    }

    public void FixedUpdate() {
        if (r.velocity.sqrMagnitude > MAX_SPEED_SQR)
            r.velocity *= 0.99f;

        s.AbilityUpdate(r);
    }

    public void ToggleFreeze() {
        if (r.isKinematic)
            r.velocity = velocity;
        else {
            velocity = r.velocity;
            r.velocity = Vector3.zero;
        }

        r.isKinematic = !r.isKinematic;
    }

    void LoadMeshRenderer() {
        if (mr == null)
            mr = gameObject.GetComponent<MeshRenderer>();
    }

    /// <summary>
    /// Adds material on top of all materials
    /// </summary>
    /// <param name="m">Material</param>
    public void AddMaterial(Material m) {
        LoadMeshRenderer();
        if (mr.materials == null)
            mr.materials = new Material[1] { m };
        else {
            Material[] materials = new Material[mr.materials.Length + 1];
            mr.materials.CopyTo(materials, 0);
            materials[mr.materials.Length] = m;
        }
    }

    /// <summary>
    /// Set <paramref name="m"/> as the only material
    /// </summary>
    /// <param name="m">Material</param>
    public void SetMaterial(Material m) {
        LoadMeshRenderer();
        mr.materials = new Material[] { m };
    }

    /// <summary>
    /// Set <paramref name="m"/> as base material (first one to render)
    /// </summary>
    /// <param name="m">Material</param>
    public void SetBaseMaterial(Material m) {
        LoadMeshRenderer();
        mr.material = m;
    }

    /// <summary>
    /// Changes model of the sphere
    /// </summary>
    /// <param name="m">Mesh</param>
    public void SetModel(Mesh m) {
        gameObject.GetComponent<MeshFilter>().mesh = m;
    }

}
