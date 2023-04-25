using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyEnemy : MonoBehaviour {
    public GameObject allBody;

    public bool getBody() {
        if(allBody.GetComponent<Enemy>().morto) {
            allBody.SetActive(false);
            return true;
        }
        return false;
    }
}
