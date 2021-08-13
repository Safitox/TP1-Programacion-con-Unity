using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSJugadorController : MonoBehaviour
{
    public float speed = 5;
    [SerializeField] float frecuenciaBalas = 0.5f;
    [SerializeField] float frecuenciaMisiles = 5f;
    float ultimaBala = 0f;
    float ultimoMisil = 0f;
    public Vector2 velocity;
    [SerializeField] float velocidadBala = 1;
    [SerializeField] float velocidadMisil = 1;
    float timerSalto =0;
    public float fuerzaSalto = 2f;
    Rigidbody rb;
    List<GameObject> balasjugador = new List<GameObject>();
    List<GameObject> misilesJugador = new List<GameObject>();
    Transform poolBalas;
    [SerializeField] GameObject pf_Bala;
    [SerializeField] GameObject pf_Misil;
    [SerializeField] Transform puntaArma;
    [SerializeField] Camera cam;
    [SerializeField] Transform anclajeCabeza;
    [SerializeField] GameObject Arma;
    bool blArmaActiva = true;
    [SerializeField] AudioClip sndOuch;
    Vector3 centroRef = new Vector3(0.5f, 0.5f, 0f);
    bool blMisil;
    bool blTutorial;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
                
    }

    private void Awake()
    {
        poolBalas = GameObject.Find("poolBalasJugador").transform;
    }

    private void OnEnable()
    {
        GameManager.Instance.OnCambioEstadoTutorial += EstadoTutorial;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnCambioEstadoTutorial -= EstadoTutorial;
    }

    void EstadoTutorial(bool OnOff)
    {
        blTutorial = OnOff;
    }

    void Update()
    {
        velocity.y = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        velocity.x = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space) && (Time.timeSinceLevelLoad - timerSalto) > 1)
        {
            rb.AddForce(Vector3.up * 100 * fuerzaSalto);
            timerSalto = Time.timeSinceLevelLoad;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
            speed = 8;
        if (Input.GetKeyUp(KeyCode.LeftShift))
            speed = 5;
        if (!blTutorial)
        {
            if (Input.GetMouseButton(0))
            {
                blMisil = false;
                Disparar();
            }
        }
        if (Input.GetKeyUp(KeyCode.KeypadPlus) || Input.GetKeyUp(KeyCode.Plus))
            GameManager.Instance.PASEDENIVEL();
        if (Input.GetKeyUp(KeyCode.E))
        {
            if (!blTutorial)
            {
                blMisil = true;
                Disparar();
            }
            else
            {
                GameManager.Instance.IniciarPartida();
            }
        }
        transform.Translate(velocity.x, 0, velocity.y);
    }


    void Disparar()
    {
        if (blArmaActiva && !blMisil)
        {
            if (Time.timeSinceLevelLoad - ultimaBala >= frecuenciaBalas)
            {
                Ray ray = cam.ViewportPointToRay(centroRef);
                RaycastHit hit;
                Vector3 objetivo;
                if (Physics.Raycast(ray, out hit))
                {
                    objetivo = hit.point;
                }
                else
                {
                    objetivo = ray.GetPoint(270f);
                }
//                Debug.DrawLine(puntaArma.position, objetivo, Color.red ,3f);
                GameObject _bala = GenerarBala();
                _bala.SetActive(true);
                _bala.transform.position = puntaArma.position;
                _bala.transform.forward = objetivo.normalized;
                _bala.GetComponent<Rigidbody>().AddForce((objetivo- puntaArma.position).normalized * velocidadBala, ForceMode.Impulse);

                ultimaBala = Time.timeSinceLevelLoad;
            }
        }
        else if (blMisil)
        {
            if (GameManager.Instance.stockMisiles == 0)
                return;
            if (Time.timeSinceLevelLoad - ultimoMisil >= frecuenciaMisiles)
            {
                blMisil = false;
                Ray ray = cam.ViewportPointToRay(centroRef);
                RaycastHit hit;
                Vector3 objetivo;
                if (Physics.Raycast(ray, out hit))
                {
                    objetivo = hit.point;
                }
                else
                {
                    objetivo = ray.GetPoint(270f);
                }
                //                Debug.DrawLine(puntaArma.position, objetivo, Color.red ,3f);
                GameObject _bala = GenerarMisil();
                _bala.SetActive(true);
                _bala.transform.position = puntaArma.position;
                _bala.transform.forward = objetivo.normalized;
                _bala.GetComponent<Rigidbody>().AddForce((objetivo - puntaArma.position).normalized * velocidadMisil, ForceMode.Impulse);
                GameManager.Instance.stockMisiles--;
                ultimoMisil = Time.timeSinceLevelLoad;
            }
        }
        else if (anclajeCabeza.GetChild(0)!=null && !Arma.activeInHierarchy)
        {
            GameObject cabeza = anclajeCabeza.GetChild(0).gameObject;
            cabeza.GetComponent<Rigidbody>().isKinematic = false;
            cabeza.transform.SetParent(null);
            cabeza.GetComponent<Rigidbody>().AddForce((cam.transform.forward).normalized * 500);
            Arma.SetActive(true);
            blArmaActiva = true;
        }
    }



    GameObject GenerarBala()
    {
        foreach (GameObject ro in balasjugador)
        {
            if (!ro.activeInHierarchy)
            {
                return ro;
            }
        }
        GameObject go = Instantiate(pf_Bala, poolBalas);
        balasjugador.Add(go);
        return go;
    }
    GameObject GenerarMisil()
    {
        foreach (GameObject ro in misilesJugador)
        {
            if (!ro.activeInHierarchy)
            {
                return ro;
            }
        }
        GameObject go = Instantiate(pf_Misil, poolBalas);
        misilesJugador.Add(go);
        return go;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("cabeza"))
        {
            Transform cab = collision.transform;
            cab.parent = anclajeCabeza;
            cab.position = anclajeCabeza.position;
            cab.rotation = anclajeCabeza.rotation;
            cab.GetComponent<Rigidbody>().isKinematic = true;
            Arma.SetActive(false);
            blArmaActiva = false;
        }
        else if (collision.gameObject.CompareTag("exploder"))
        {
            GameManager.Instance.JugadorTocado(18);

        }
        else if (collision.gameObject.CompareTag("bala"))
        {
            GameManager.Instance.JugadorTocado(1);
        }
        else if (collision.gameObject.CompareTag("salida"))
        {
            GameManager.Instance.PASEDENIVEL();
            
        }
        else if (collision.gameObject.CompareTag("cajaMisiles"))
        {
            GameManager.Instance.stockMisiles += 10;
            collision.gameObject.SetActive(false);
        }

    }


    
    
}
