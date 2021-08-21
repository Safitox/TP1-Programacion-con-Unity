using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Robot1 : MonoBehaviour
{
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
    public bool blDisparar = false;
    private Animator anim;
    [SerializeField] float chancePowerUp = 0f;

    [Header("Audio")]
    [SerializeField] AudioClip sndImpacto;
    [SerializeField]
    AudioClip sndDisparo;
    [SerializeField]
    AudioClip[] sndActivar;
    private AudioSource _audio;


    void Start()
    {
        Cabeza = transform.GetChild(0);
        _audio = GetComponent<AudioSource>();
    }


    private void Awake()
    {
        anim = GetComponent<Animator>();
        cara.material.color = Color.white;
        SCMAN = GameObject.Find("SceneManager").GetComponent<SceneManager1>(); 
    }

    private void FixedUpdate()
    {
        if (blmovible)
            Perseguir();
    }

    private void OnEnable()
    {
        anim.SetBool("Apagar", true);
        vida = 20;
        blmovible = false;
        CancelInvoke();
    }

    public void Activar()
    {
        this.transform.SetParent(SCMAN.poolRobots1);
        anim.SetBool("Apagar", false);
        Invoke("Disparar", 4f);
        Invoke("Persecucion", 2.5f);

    }

    public void Persecucion()
    {

        blDisparar = true;
        blmovible = true;
        cara.material.color = Color.red;
        _audio.PlayOneShot(sndActivar[UnityEngine.Random.Range(0, sndActivar.Length)]);
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
            if (sndImpacto)
                _audio.PlayOneShot(sndImpacto, 1f);
            if (blmovible)
            {
                vida -= GameManager.Instance.basicBulletDmg * float.Parse(pars[1].ToString());
                EvaluarVida();
            }

        }
        else if (pars[0].ToString() == "exploder" && blmovible)
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
            _audio.Stop();
            blDisparar = false;
            if (UnityEngine.Random.Range(0, 100) <= chancePowerUp)
                GameManager.Instance.SoltarPowerUp(transform.position);
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
        if (_audio.enabled )
            _audio.PlayOneShot(sndDisparo, 0.3f);
        Invoke("Disparar", UnityEngine.Random.Range(1f, 3f));
    }
}
