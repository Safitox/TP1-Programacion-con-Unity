using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Robot2 : MonoBehaviour
{
    // Start is called before the first frame update
    public float vida = 50;
    public Transform Jugador;
    public float rotRestring = 0.1f;
    [SerializeField] float velocidadBala = 550000f;
    [SerializeField] MeshRenderer cara;
    [SerializeField]
    SceneManager2 SCMAN;
    [SerializeField] Transform canon1;
    [SerializeField] Transform canon2;
    public bool blDisparar = false;



    [Header("Audio")]
    [SerializeField]
    AudioClip sndDisparo;


    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }


    private void Awake()
    {
        cara.material.color = Color.red;
        SCMAN = GameObject.Find("SceneManager").GetComponent<SceneManager2>();
        Activar();
    }

    private void FixedUpdate()
    {
            Perseguir();
    }


    public void Activar()
    {
        vida = 20;

        Invoke("Disparar", 4f);


    }






    void Perseguir()
    {

        transform. LookAt(Jugador);


    }






    void Disparar()
    {

        if (blDisparar)
        {
            GameObject _bala = SCMAN.GenerarBalaEnemiga();
            _bala.SetActive(true);
            _bala.transform.position = canon1.position;
            _bala.transform.rotation = canon1.rotation;
            _bala.GetComponent<Rigidbody>().AddForce((Jugador.position - canon2.position).normalized * velocidadBala, ForceMode.Impulse);
//            _bala.GetComponent<Rigidbody>().AddForce((transform.forward) * velocidadBala);
            _bala = SCMAN.GenerarBalaEnemiga();
            _bala.SetActive(true);
            _bala.transform.position = canon2.position;
            _bala.transform.rotation = canon2.rotation;
            _bala.GetComponent<Rigidbody>().AddForce((Jugador.position - canon2.position).normalized * velocidadBala, ForceMode.Impulse);
//            _bala.GetComponent<Rigidbody>().AddForce((transform.forward) * velocidadBala);
            if (GetComponent<AudioSource>().enabled)
                GetComponent<AudioSource>().PlayOneShot(sndDisparo, 0.1f);
        }
        Invoke("Disparar", UnityEngine.Random.Range(1f, 3f));
    }

    public void Colisionado()
    { }




}
