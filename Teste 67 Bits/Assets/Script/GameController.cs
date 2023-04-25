using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
    public List<GameObject> poolEnemy;
    public Transform parent;
    public GameObject EnemyObject;
    private int numberSpw;

    void Start() {
        GameObject tmp;
        for(int i = 0; i < 50; i++) { //Criando a Pool de objetos
            tmp = Instantiate(EnemyObject, parent);
            tmp.SetActive(false);
            poolEnemy.Add(tmp);
        }

        SpwEnemy();
        StartCoroutine(timerSpwEnemy());
    }

    public void SpwEnemy() {
        numberSpw++;
        if(numberSpw >= poolEnemy.Count) { numberSpw = 0; }
        if(!poolEnemy[numberSpw].activeSelf) {
            float randomX = Random.Range(-9f, 9f);
            float randomZ = Random.Range(-5f, 20.0f);
            poolEnemy[numberSpw].transform.position = new Vector3(randomX, 0, randomZ);
            poolEnemy[numberSpw].SetActive(true);

            if(poolEnemy[numberSpw].GetComponent<Enemy>().morto) {
                poolEnemy[numberSpw].GetComponent<Enemy>().resetEnemy();
            }
        }
    }

    IEnumerator timerSpwEnemy() {
        yield return new WaitForSeconds(3f);
        SpwEnemy();
        StartCoroutine(timerSpwEnemy());
    }
}
