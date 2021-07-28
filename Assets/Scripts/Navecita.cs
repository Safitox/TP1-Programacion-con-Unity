using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navecita : MonoBehaviour
{
    public float vida = 50;
    public Transform Jugador;
    public float velocidadPersecucion = 0.6f;
    public float rotRestring = 0.1f;
    [SerializeField] float velocidadBala = 550000f;
    public bool blmovible;
    [SerializeField] float maximaDistanciaDeCentro = 8f;
    SceneManager3 SCMAN;
    [SerializeField] Transform canon1;
    public bool blDisparar = false;
    public Transform centro;

    [Header("Audio")]
    [SerializeField]
    AudioClip sndDisparo;
    [SerializeField]
    AudioClip sndImpacto;





    private void Awake()
    {
        SCMAN = GameObject.Find("SceneManager").GetComponent<SceneManager3>();
    }

    private void FixedUpdate()
    {
        if (blmovible)
            Perseguir();
    }


    public void Activar()
    {
        vida = 20;
        Invoke("Disparar", 4f);
        Invoke("Persecucion", 2.5f);

    }

    public void Persecucion()
    {

        blDisparar = true;
        blmovible = true;
    }





    void Perseguir()
    {

        transform.LookAt(Jugador);
        //Quaternion Q = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, Jugador.transform.position, 5, rotRestring * Time.deltaTime));
        // transform.rotation =new Quaternion(transform.rotation.x, Q.y, transform.rotation.z, Q.w);
        Vector3 V = new Vector3(centro.position.x, transform.position.y, centro.position.z);
        if ((V- transform.position).sqrMagnitude >= maximaDistanciaDeCentro*maximaDistanciaDeCentro)
        {
            //Debug.Log(Vector3.Distance(V, transform.position));
            V = new Vector3(transform.position.x, Jugador.position.y, transform.position.z);
        }
        transform.position = Vector3.MoveTowards(transform.position,  V, velocidadPersecucion * Time.deltaTime);

    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("bala"))
        {
            vida -= 2;
            EvaluarVida();
            if (sndImpacto)
                GetComponent<AudioSource>().PlayOneShot(sndImpacto, 0.5f);
        }
        else if (collision.gameObject.CompareTag("exploder"))
        {
            vida -= 20 ;
            EvaluarVida();

        }

    }



    void EvaluarVida()
    {
        if (vida <= 0)
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
          if (GetComponent<AudioSource>().enabled)
            GetComponent<AudioSource>().PlayOneShot(sndDisparo, 0.3f);
        Invoke("Disparar", UnityEngine.Random.Range(1f, 3f));
    }




}
