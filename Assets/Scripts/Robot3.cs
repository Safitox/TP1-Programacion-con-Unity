using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


public class Robot3 : MonoBehaviour
{
    [SerializeField]
    GameObject goLanzallamas;
    NavMeshAgent NV;


    public float vida = 50;
    public Transform Jugador;
    public float velocidadPersecucion=0.6f;
    public float rotRestring = 0.1f;
    public bool blmovible;
    [SerializeField] float minimaDistanciaDejugador=4f;
    [SerializeField] MeshRenderer cara;
    SceneManager1 SCMAN;
    public bool blDisparar = false;
    [SerializeField] float chancePowerUp = 0f;

    [Header("Audio")]
    [SerializeField] AudioClip sndImpacto;
    private AudioSource _audio;
    private float baseSpeed = 0f;


    void Start()
    {
        _audio = GetComponent<AudioSource>();
        Jugador = GameManager.Instance.jugador;

    }

    private void Awake()
    {
        NV = GetComponent<NavMeshAgent>();
        NV.baseOffset = 1f;
        baseSpeed = NV.speed;
    }


    private void FixedUpdate()
    {
       // if (blmovible)
        {
            if ((transform.position - Jugador.position).sqrMagnitude >= minimaDistanciaDejugador * minimaDistanciaDejugador)
                NV.speed = baseSpeed;
            else
            {
                transform.LookAt(Jugador);
                NV.speed = 0f;
            }
               

            NV.SetDestination(Jugador.position);
            //Perseguir();
        }
    }

    private void OnEnable()
    {
        vida = 20;
        blmovible = false;
        CancelInvoke();
    }


    public void Persecucion()
    {

        blDisparar = true;
        blmovible = true;
        cara.material.color = Color.red;
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
            vida -= GameManager.Instance.basicBulletDmg * float.Parse(pars[1].ToString());
            if (sndImpacto)
                _audio.PlayOneShot(sndImpacto, 1f);
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
            //SCMAN.ActivarNuevoRobotDestruido(transform);
            _audio.Stop();
            blDisparar = false;
            if (UnityEngine.Random.Range(0, 100) <= chancePowerUp)
                GameManager.Instance.SoltarPowerUp(transform.position);
            GameManager.Instance.Explosion(transform.position);
            gameObject.SetActive(false);
        }
    
    }


    void Disparar()
    {
    }
}
