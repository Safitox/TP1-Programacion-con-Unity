using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
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
    [SerializeField] Transform jugador;

    [Header("UI")]
    [SerializeField] TMPro.TextMeshProUGUI txtVida;
    [SerializeField] TMPro.TextMeshProUGUI txtMisiles;
    GameObject UIMuerto;

    [Header("Sonidos")]
    [SerializeField] AudioClip sndJugadorFesteja;
    [SerializeField] AudioClip sndJugadorMuyDa�ado;
    [SerializeField] AudioClip sndJugadorMuere;


    public static GameManager GM;
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
    void Awake()
    {
        if (GM != null)
            GameObject.Destroy(GM);
        else
            GM = this;


        DontDestroyOnLoad(this.gameObject);

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


    public void PASEDENIVEL()
    {
        
        SceneManager.LoadScene(Escenas[EscenaActual],LoadSceneMode.Single);
        if (EscenaActual!=0)
            GetComponent<AudioSource>().PlayOneShot(sndJugadorFesteja, 1);
        EscenaActual++;


    }

    public void SacudirCamara(float tiempo)
    {
        tpoSacudida = tiempo;
    }

}
