using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punch : MonoBehaviour {
    public GameObject PlayerMain;
    void OnTriggerEnter(Collider collision) {
        if (collision.gameObject.tag == "Enemy") {
            collision.gameObject.GetComponent<Enemy>().golpe(PlayerMain.transform.position);
        }
    }
}
