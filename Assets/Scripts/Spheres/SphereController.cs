﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SphereController : MonoBehaviour {
    const float MAX_SPEED = 10;
    const float MAX_SPEED_SQR = MAX_SPEED * MAX_SPEED;

    Rigidbody r;
    SphereStats s;
    Vector3 velocity;
    MeshRenderer mr;

    bool isFrozen = false;

    void Start() {
        StartCoroutine("IsInside");
        r = GetComponent<Rigidbody>();
        s = GetComponent<SphereStats>();
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

    public void SetFreeze(bool value) {
        if (value == isFrozen)
            return;

        isFrozen = value;

        if (value) {
            velocity = r.velocity;
            r.velocity = Vector3.zero;
        } else
            r.velocity = velocity;

        r.isKinematic = value;
    }

    void LoadMeshRenderer() {
        if (mr == null)
            mr = gameObject.GetComponent<MeshRenderer>();
    }

    /// <summary>
    /// Returns true value of velocity even in frozen states
    /// </summary>
    /// <returns>velocity value</returns>
    public float GetSqrVelocity() {
        return isFrozen ? velocity.sqrMagnitude : r.velocity.sqrMagnitude;
    }

    /// <summary>
    /// Adds material on top of all materials
    /// </summary>
    /// <param name="m">Material</param>
    public void AddMaterial(Material m) {
        LoadMeshRenderer();
        Material[] materials = new Material[mr.materials.Length + 1];
        mr.materials.CopyTo(materials, 0);
        materials[mr.materials.Length] = m;
        mr.materials = materials;
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
    /// Remove given material
    /// todo doesn't work thanks to instancing
    /// </summary>
    /// <param name="m">material</param>
    public void RemoveMaterial(Material m) {
        int index = -1;
        for (int i = 0; i < mr.materials.Length; i++) {
            Debug.Log(m + " vs " + mr.materials[i]);
            if (m == mr.materials[i]) {
                index = i;
                break;
            }
        }

        Debug.Log(index);

        if (index == -1)
            return;
        else if (mr.materials.Length == 1)
            mr.materials = new Material[1] { GlobalManager.standardMaterial };
        else {
            Material[] temp = new Material[mr.materials.Length - 1];
            for (int i = 0; i < mr.materials.Length; i++) {
                if (i == index)
                    continue;
                else if (i > index)
                    temp[i - 1] = mr.materials[i];
                else
                    temp[i] = mr.materials[i];
            }
            mr.materials = temp;
        }
    }

    /// <summary>
    /// Changes model of the sphere
    /// </summary>
    /// <param name="m">Mesh</param>
    public void SetModel(Mesh m) {
        gameObject.GetComponent<MeshFilter>().mesh = m;
    }

    /// <summary>
    /// Removes all colliders from Sphere
    /// </summary>
    public void RemoveColliders() {
        Collider[] colliders = gameObject.GetComponents<Collider>();

        foreach (var item in colliders)
            Destroy(item);
    }

    /// <summary>
    /// Removes all trigger colliders from Sphere
    /// </summary>
    public void RemoveTriggerColliders() {
        Collider[] colliders = gameObject.GetComponents<Collider>();

        foreach (var item in colliders)
            if (item.isTrigger)
                Destroy(item);
    }
}
