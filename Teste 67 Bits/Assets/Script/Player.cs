using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    public List<GameObject> mochila;
    public Rigidbody rb;
    public FixedJoystick Joystick;
    public Animator anim;
    public Transform parentGO, Pos;
    public GameObject handPunch, body, PlayerGO, LastBody;
    public Text moedaText, maxText, upradeText;
    public Renderer renderPlayer;
    private float speed = 6f, altura = 0f;
    private bool waitPunch;
    private int limitePoolMochila, corposCostas, mochilaMax, moedas, precoUpgd;

    void Start() {
        mochilaMax = 5;
        precoUpgd = 1;
        limitePoolMochila = 25;
        mochila = new List<GameObject>();
        LastBody = PlayerGO;
        setPoolMochila();
    }

    public void setPoolMochila() {
        GameObject tmp;
        for(int i = 0; i < limitePoolMochila; i++) { //Criando a Pool de objetos
            tmp = Instantiate(body, Pos.position + new Vector3(0f, altura, 0f), Pos.rotation, parentGO);
            tmp.SetActive(false);
            tmp.GetComponent<SpringJoint>().connectedBody = LastBody.GetComponent<Rigidbody>(); //Pega o último da pilha para ter o efeito de inércia
            altura += 1.2f;
            mochila.Add(tmp);
            LastBody = tmp;
        }
    }

    public void UpgradeFunc() {
        if(moedas >= precoUpgd) {
            moedas-= precoUpgd;
            mochilaMax+= 1;
            precoUpgd+= 2;
            attTextUI();
            changeColor();
        }
    }

    void FixedUpdate() {
        rb.velocity = new Vector3(Joystick.Horizontal * speed, rb.velocity.y, Joystick.Vertical * speed);

        if(Joystick.Horizontal != 0 || Joystick.Vertical != 0) {
            transform.rotation = Quaternion.LookRotation(rb.velocity);
            anim.SetBool("Running", true);
        }
        else {
            anim.SetBool("Running", false);
        }
    }

    public void Punch() {
        if(waitPunch == false) {
            StartCoroutine(timePunch());
        }
    }

    public void attTextUI() {
        moedaText.text = moedas.ToString() + " Coins";
        maxText.text = mochilaMax.ToString() + " Max";
        upradeText.text = precoUpgd.ToString() + " Coins";
    }

    public void changeColor() {
        float cor1 = Random.Range(0f, 1f);
        float cor2 = Random.Range(0f, 1f);
        float cor3 = Random.Range(0f, 1f);

        renderPlayer.material.SetColor("_Color", new Color(cor1, cor2, cor3, 1f));
    }

    void OnTriggerStay(Collider collision) {
        if (collision.gameObject.tag == "Body" && corposCostas < mochilaMax) { //Empilhar nas costas
            if(collision.gameObject.GetComponent<BodyEnemy>().getBody()) {
                if(corposCostas >= limitePoolMochila) {
                    setPoolMochila(); //Aumenta o pool caso necessário
                }
                mochila[corposCostas].SetActive(true);
                corposCostas++;
            }
        }
    }

    void OnTriggerEnter(Collider collision) {
        if (collision.gameObject.tag == "Sell") { //Vender inimigos
            float altura = 0f;
            moedas += corposCostas;
            attTextUI();

            for(int i = 0; i < corposCostas; i++) {
                mochila[i].transform.position = Pos.position + new Vector3(0f, altura, 0f);
                mochila[i].SetActive(false);
                altura += 1.2f;
            }

            corposCostas = 0;
        }
    }

    IEnumerator timePunch() {
        waitPunch = true;
        anim.SetBool("Punch", true);
        handPunch.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        handPunch.SetActive(false);
        waitPunch = false;
        anim.SetBool("Punch", false);
    }
}
