using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Robot1 : MonoBehaviour
{
    // Start is called before the first frame update
    public float vida = 50;
    public Transform Jugador;
    Transform Cabeza;
    public float velocidadPersecucion=0.6f;
    public float rotRestring = 0.1f;
    [SerializeField] float velocidadBala = 550000f;
    public bool blmovible;
    [SerializeField] float minimaDistanciaDejugador=4f;
    [SerializeField] MeshRenderer cara;
    SceneManager1 SCMAN;
    [SerializeField] Transform canon1;
    [SerializeField] Transform canon2;
    [SerializeField] AudioClip sndImpacto;
    public bool blDisparar = false;


    [Header("Audio")]
    [SerializeField]
    AudioClip sndDisparo;
    [SerializeField]
    AudioClip[] sndActivar;

    void Start()
    {
        Cabeza = transform.GetChild(0);
    }



    void Update()
    {

    }


    private void Awake()
    {
        cara.material.color = Color.white;
        SCMAN = GameObject.Find("SceneManager").GetComponent<SceneManager1>(); 
    }

    private void FixedUpdate()
    {
        if (blmovible)
            Perseguir();
    }


    public void Activar()
    {
        vida = 20;
        this.transform.SetParent(SCMAN.poolRobots1);
        transform.GetComponent<Animator>().SetTrigger("Activar");
        Invoke("Disparar", 4f);
        Invoke("Persecucion", 2.5f);

    }

    public void Persecucion()
    {

        blDisparar = true;
        blmovible = true;
        cara.material.color = Color.red;
        GetComponent<AudioSource>().PlayOneShot(sndActivar[UnityEngine.Random.Range(0, sndActivar.Length)]);
    }





    void Perseguir()
    {

        transform. LookAt(Jugador);
        if ((transform.position - Jugador.position).sqrMagnitude >= minimaDistanciaDejugador*minimaDistanciaDejugador)
            transform.position = Vector3.MoveTowards(transform.position, Jugador.position, velocidadPersecucion * Time.deltaTime);

    }


    public void Colisionado(object[] pars)
    {
        if (pars[0].ToString() == "bala")
        {
            vida -= 2 * float.Parse(pars[1].ToString());
            if (sndImpacto)
                GetComponent<AudioSource>().PlayOneShot(sndImpacto, 1f);
            EvaluarVida();

        }
        else if (pars[0].ToString() == "exploder")
        {
            vida -= 20 * float.Parse(pars[1].ToString());
            EvaluarVida();

        }
    }


    void EvaluarVida()
    { 
        if (vida<=0)
        {
            SCMAN.ActivarNuevoRobotDestruido(transform);
            blmovible = false;
            blDisparar = false;
            gameObject.SetActive(false);
        }
    
    }


    void Disparar()
    {

        if (!blDisparar)
            return;
        GameObject _bala = SCMAN.GenerarBalaEnemiga();
            _bala.SetActive(true);
            _bala.transform.position = canon1.position;
            _bala.transform.rotation = canon1.rotation;
            _bala.GetComponent<Rigidbody>().AddForce((transform.forward) * velocidadBala);
        _bala = SCMAN.GenerarBalaEnemiga();
        _bala.SetActive(true);
        _bala.transform.position = canon2.position;
        _bala.transform.rotation = canon2.rotation;
        _bala.GetComponent<Rigidbody>().AddForce((transform.forward) * velocidadBala);
        if (GetComponent<AudioSource>().enabled )
            GetComponent<AudioSource>().PlayOneShot(sndDisparo, 0.3f);
        Invoke("Disparar", UnityEngine.Random.Range(1f, 3f));
    }




}
