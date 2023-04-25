using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public Animator anim;
    public Rigidbody rigidMain;
    public Collider colliderMain;
    public GameObject parentEnemy;
    public bool morto;
    private Rigidbody[] ragdollBodies;
    private Collider[] ragdollCollider;

    void Awake() {
        ragdollBodies = parentEnemy.GetComponentsInChildren<Rigidbody>();
        ragdollCollider = parentEnemy.GetComponentsInChildren<Collider>();
        ToggleRagdoll(false);
    }

    public void golpe(Vector3 pos) {
        ToggleRagdoll(true);

        foreach(Rigidbody rb in ragdollBodies) {
            rb.AddForce((pos - parentEnemy.transform.position) * -1750f);
        }

        StartCoroutine(readyToCollect());
    }

    public void resetEnemy() {
        morto = false;
        ToggleRagdoll(false);
    }

    public void ToggleRagdoll(bool state) {
        anim.enabled = !state;
        rigidMain.isKinematic = state;
        colliderMain.enabled = !state;

        foreach(Rigidbody rb in ragdollBodies) {
            rb.isKinematic = !state;
        }

        foreach(Collider collider in ragdollCollider) {
            collider.enabled = state;
        }
    }

    IEnumerator readyToCollect() {
        yield return new WaitForSeconds(1f);
        morto = true;
    }
}
