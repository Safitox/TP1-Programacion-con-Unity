using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombaRotadora : MonoBehaviour
{

    Vector3 direccion;
    public Transform target;
    public float fuerza = 8f;
    public bool Activa = false;
    [SerializeField] float DistanciaExplosiva = 2f;
    bool blbye = false;
    public int Vida = 20;
    [SerializeField] AudioClip muere;
    [SerializeField] AudioClip ouch;



    private void OnEnable()
    {
        gameObject.GetComponent<MeshRenderer>().material.color = Color.white;
        blbye = false;

    }
    private void Awake()
    {
        if (GameManager.GM.EscenaActual == 4)
            fuerza = 7f;
    }


    void FixedUpdate()
    {
        if (Activa)
        {
            direccion = (target.position - transform.position).normalized;
            GetComponent<Rigidbody>().AddForce(direccion * fuerza);

            if (!blbye)
            {
                if ((transform.position - target.position).sqrMagnitude <= DistanciaExplosiva * DistanciaExplosiva)
                {
                    blbye = true;
                    GetComponent<AudioSource>().PlayOneShot(muere, 0.2f);
                    Invoke("Explotar", Random.Range(0.1f, 0.7f));
                }
            }
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("piso"))
        {
            this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;

        }
        if (collision.gameObject.CompareTag("bala"))
        {
            GetComponent<AudioSource>().PlayOneShot(ouch, 0.2f);
            Vida -= 2;
            if (Vida <= 0)
                Explotar();
            else if (Vida < 5)
                gameObject.GetComponent<MeshRenderer>().material.color = Color.red;

        }
        else if (collision.gameObject.CompareTag("exploder"))
        {
            GetComponent<AudioSource>().PlayOneShot(ouch, 0.2f);
            Vida -= 12;
            if (Vida <= 0)
                Explotar();
            else if (Vida < 5)
                gameObject.GetComponent<MeshRenderer>().material.color = Color.red;

        }
    }

    void Explotar()
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        GameManager.GM.Explosion(transform.position);
        if (GameManager.GM.EscenaActual==4)
        {
            GameObject _caja = SceneManager4.SM.GenerarCaja();
            _caja.transform.position = transform.position;
            _caja.SetActive(true);
            this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

        }
        gameObject.SetActive(false);

    }

}
