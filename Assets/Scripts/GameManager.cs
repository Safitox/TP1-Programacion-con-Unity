using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : Singleton<GameManager>
{
    [Header("Escenas")]
    [SerializeField] string[] Escenas;


    [Header("Variables")]
    [SerializeField]
    int Vidas = 3;
    public int VidaInicial = 20;
    public int VidaJugador = 20;
    public int EscenaActual = 0;
    bool blInvulnerable=false;
    float tpoSacudida=0;
    public int stockMisiles = 10;
   // private bool blTutorial = false;

    [Header("GameObjects")]
    [SerializeField]
    Transform poolExplosiones;
    [SerializeField]
    GameObject pf_Explosion1;
    List<GameObject> Explosiones = new List<GameObject>();
    [SerializeField]
    GameObject pf_Muerto;
    [SerializeField]
    GameObject pf_CajaMunicion;

    [SerializeField] GameObject uiDamage;

    [Header("Referencias")]
    [SerializeField] GameObject obSCMAN;
    public Transform jugador;

    [Header("UI")]
    [SerializeField] TMPro.TextMeshProUGUI txtVida;
    [SerializeField] TMPro.TextMeshProUGUI txtMisiles;
    GameObject UIMuerto;

    [Header("Sonidos")]
    [SerializeField] AudioClip sndJugadorFesteja;
    [SerializeField] AudioClip sndJugadorMuyDañado;
    [SerializeField] AudioClip sndJugadorMuere;

    [Header("Acciones")]
    public Action onStartGame;
    private bool _blTutorial = false;
    private bool _blGameOn = false;
    public event OnVariableChangeDelegate OnCambioEstadoTutorial;
    public event OnVariableChangeDelegate OnCambioEstadoGame;
    public delegate void OnVariableChangeDelegate(bool newVal);

    public bool blTutorial
    {
        get
        {
            return _blTutorial;
        }
        set
        {
            if (_blTutorial == value) return;
            _blTutorial = value;
            if (OnCambioEstadoTutorial != null)
                OnCambioEstadoTutorial(_blTutorial);
        }
    }

    public bool blGameOn    {
        get
        {
            return _blGameOn;
        }
        set
        {
            if (_blGameOn == value) return;
            _blGameOn = value;
            if (OnCambioEstadoGame != null)
                OnCambioEstadoGame(_blGameOn);
        }
    }




    public int chequeo = 0;

    void Start()
    {

        VidaJugador = 20;
        SceneManager.sceneLoaded += OnSceneLoaded;

    }

    // Update is called once per frame
    void Update()
    {
        if (tpoSacudida != 0 && false)
        {

            tpoSacudida -= Time.deltaTime;
            jugador.GetChild(0).localPosition = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1)) * 0.3f;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (blTutorial)
                blTutorial = false;
        }


    }

    private void FixedUpdate()
    {
        if (EscenaActual != 0)
            txtVida.text = VidaJugador.ToString() + "-- ( " + new string('|', Vidas) + " )"; 
        if (EscenaActual==4)
            txtMisiles.text = "Misiles (E) - " + new string('X', stockMisiles);

    }



    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (EscenaActual != 0)
        {
            txtVida = GameObject.Find("UITXTvida").GetComponent<TMPro.TextMeshProUGUI>();
            obSCMAN = GameObject.Find("SceneManager");
            poolExplosiones = GameObject.Find("poolExplosiones").transform;
            uiDamage = GameObject.Find("UIDamage");
            UIMuerto = GameObject.Find("UIMuerto");
            UIMuerto.SetActive(false);
            jugador = GameObject.FindGameObjectWithTag("Player").transform;
            Explosiones.Clear();
            VidaJugador = VidaInicial;
            if (EscenaActual==4)
                txtMisiles = GameObject.Find("UITXTmisiles").GetComponent<TMPro.TextMeshProUGUI>();
        }
    }


    public void Explosion(Vector3 posicion)
    {
        GameObject exp = GenerarExplosion();

        exp.transform.position = posicion;
        exp.SetActive(true);
    }

    GameObject GenerarExplosion()
    {
        foreach (GameObject ro in Explosiones)
        {
            if (!ro.activeInHierarchy)
            {
                return ro;
            }
        }
        GameObject go = Instantiate(pf_Explosion1, poolExplosiones);
        Explosiones.Add(go);
        return go;
    }


    public void JugadorTocado(int damage)
    {
        if (!blInvulnerable)
        {
            VidaJugador -= damage;
            uiDamage.GetComponent<Animator>().SetTrigger("ouch");
            if (VidaJugador <= 0)
                MuereJugador();
        }

    }


    void MuereJugador()
    {
        UIMuerto.SetActive(true);
        obSCMAN.SendMessage("MurioElJugador");
        Vidas--;
        GetComponent<AudioSource>().PlayOneShot(sndJugadorMuere, 1);
        if (Vidas != 0)
        {
            GameObject.Instantiate(pf_Muerto,  jugador.position, jugador.rotation);
            blInvulnerable = true;
            Invoke("AunVivo", 5f);
        }
        else
        {
            Application.Quit();
        }
    }

    public void CargarMisiles()
    {
        stockMisiles += 10;
    }



    void AunVivo()
    {
        blInvulnerable = false;
        UIMuerto.SetActive(false);
        VidaJugador = VidaInicial;
        obSCMAN.SendMessage("SpawnearJugador");
    }


    public void PASEDENIVEL(int overrider=0)
    {
        if (overrider != 0)
            EscenaActual = overrider-1;
        SceneManager.LoadScene(Escenas[EscenaActual],LoadSceneMode.Single);
        if (EscenaActual!=0)
            GetComponent<AudioSource>().PlayOneShot(sndJugadorFesteja, 1);
        EscenaActual++;
        blTutorial = true;


    }

    public void SacudirCamara(float tiempo)
    {
        tpoSacudida = tiempo;
    }



    public void IniciarPartida()
    {
        if (onStartGame != null)
            onStartGame();
    }

}
