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
    private int VidasIniciales = 3;
    private int _Vidas =0;
    public float VidaInicial = 20;
    public float _VidaJugador = 0;
    public int EscenaActual = 0;
    bool blInvulnerable=false;
    public int _stockMisiles = -1;
    public float volMusica = 0.2f;
    public float basicBulletDmg = 1f;
    [SerializeField] float defaultBulletDamage;

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


    [Header("Referencias")]
    public Transform jugador;
    private PowerUpDispenser _PowerUpDispenser;

    [Header("UI")]
    [SerializeField] GameObject canvasJuego;

    [Header("Sonidos")]
    [SerializeField] AudioClip sndJugadorFesteja;
    [SerializeField] AudioClip sndJugadorMuyDañado;
    [SerializeField] AudioClip sndJugadorMuere;
    public AudioClip MusicaTutorial;
    public AudioClip MusicaVictoria;
    public AudioClip MusicaGameOver;
    [SerializeField] AudioClip sndPowerUp;

    [Header("Acciones")]
    public Action onStartGame;
    public Action onPlayerDamaged;
    public Action onRespawn;
    public Action onPlayerdied;
    public Action onRestart;
    private bool _blTutorial = false;
    private bool _blGameOn = false;
    private bool _blPlusDMG = false;
    public Action<bool> OnCambioEstadoTutorial;
    public Action<bool> OnCambioEstadoGame;
    public Action<bool> OnPlusDMG;
    public Action<int>  OnCambioVidas;
    public Action<int> OnCambioMisiles;
    public Action<float> OnCambioVidaJugador ;
    public Action OnPressEscape;
    public Action onPause;
    public Action onContinue;
    public Action onWin;
    public Action onDefeat;
    public Action onLevelChange;

    #region EventosVariables
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
    public bool blPlusDMG
    {
        get
        {
            return _blPlusDMG;
        }
        set
        {
            if (_blPlusDMG == value) return;
            _blPlusDMG = value;
            OnPlusDMG?.Invoke(_blPlusDMG);
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
    public int Vidas
    {
        get
        {
            return _Vidas;
        }
        set
        {
            if (_Vidas == value) return;
            _Vidas = value;
            if (OnCambioVidas != null)
                OnCambioVidas(_Vidas);
        }
    }
    public int stockMisiles
    {
        get
        {
            return _stockMisiles;
        }
        set
        {
            if (_stockMisiles == value) return;
            _stockMisiles = value;
            if (_stockMisiles > 20)
                _stockMisiles = 20;
            if (OnCambioMisiles != null)
                OnCambioMisiles(_stockMisiles);
        }
    }
    public float VidaJugador
    {
        get
        {
            return _VidaJugador;
        }
        set
        {
            if (_VidaJugador == value) return;
            _VidaJugador = value;
            if (OnCambioVidaJugador != null)
                OnCambioVidaJugador(_VidaJugador / VidaInicial);
        }
    }

    #endregion

    public int chequeo = 0;

    void Start()
    {
        VidaJugador = 20;
        SceneManager.sceneLoaded += OnSceneLoaded;
        _PowerUpDispenser = GetComponent<PowerUpDispenser>();

    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (blTutorial)
            {
                blTutorial = false;
                blGameOn = true;
                IniciarPartida();
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnPressEscape?.Invoke();
            if (EscenaActual!=0)
                Pause();
               
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (EscenaActual != 0)
        {
            CancelInvoke();
            basicBulletDmg = defaultBulletDamage;
            blPlusDMG = false;
            poolExplosiones = GameObject.Find("poolExplosiones").transform;
            jugador = GameObject.FindGameObjectWithTag("Player").transform;
            Explosiones.Clear();
            VidaJugador = VidaInicial;
            stockMisiles = 0;
            jugador.GetComponent<FPSJugadorController>().blSaltoHabilitado = (EscenaActual != 2);
            if (EscenaActual == 4)
                stockMisiles = 3;
            else if (EscenaActual == 1)
            {
                Vidas = VidasIniciales;
                VidaJugador = VidaInicial;
            }
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
            if (VidaJugador <= 0)
                MuereJugador();
            else
            {
                onPlayerDamaged?.Invoke();
            }

        }

    }


    void MuereJugador()
    {
        Vidas--;
        onPlayerdied?.Invoke();
        GetComponent<AudioSource>().PlayOneShot(sndJugadorMuere, 1);
        if (Vidas != 0)
        {
            GameObject.Instantiate(pf_Muerto, jugador.position, jugador.rotation);
            blInvulnerable = true;
            Invoke("AunVivo", 5f);
        }
        else
            Restart();
    }

    public void Restart()
    {
        onRestart?.Invoke();
        EscenaActual = 0;
        Resume();
        CursorLock(false);
        SceneManager.LoadScene(0);
    }

    public void CargarMisiles() => stockMisiles += 10;

    public void AunVivo()
    {
        blInvulnerable = false;
        VidaJugador = VidaInicial;
        onRespawn?.Invoke() ;
    }


    public void PASEDENIVEL(int overrider=0)
    {
        blTutorial = true;
        CursorLock(true);
        blGameOn = false;
        if (overrider != 0)
            EscenaActual = overrider-1;
        SceneManager.LoadScene(Escenas[EscenaActual],LoadSceneMode.Single);
        onLevelChange?.Invoke();
        if (EscenaActual!=0)
            GetComponent<AudioSource>().PlayOneShot(sndJugadorFesteja, 1);
        EscenaActual++;
    }

    public void IniciarPartida()
    {
        blTutorial = false;
        blGameOn = true;
        onStartGame?.Invoke();
    }

    public void Quit() => Application.Quit();
    

    public void AddVida(int cuanto)
    {
        VidaJugador += cuanto;
        if (VidaJugador > VidaInicial)
            VidaJugador = VidaInicial;

    }

    public void AddPlusDmg()
    {
        basicBulletDmg *= 2;
        blPlusDMG = true;
        Invoke("RemovePlusDmg", 10f);
    }
    public void RemovePlusDmg()
    {
        basicBulletDmg *= 0.5f;
        blPlusDMG = (basicBulletDmg != defaultBulletDamage);
    }


    public void SoltarPowerUp(Vector3 pos)
    {
        if (blGameOn)
            _PowerUpDispenser.dropPowerUp(pos);
    }

    public void Pause()
    {
        Time.timeScale = 0;
        CursorLock(false);
        onPause?.Invoke();
    }
    public void Resume()
    {
        Time.timeScale = 1;
        CursorLock(true);
        onContinue?.Invoke();
    }

    void CursorLock(bool OnOff)
    {
        if (OnOff)
            Cursor.lockState = CursorLockMode.Locked;
        else
            Cursor.lockState = CursorLockMode.None;
    }

    public void Victoria()
    {
        onWin?.Invoke();
        VidaJugador = 5000;
        Pause();
    }

    public void Defeat()
    {
        onDefeat?.Invoke();
        VidaJugador = 5000;
        Pause();
    }

    public void RecogerPowerUp() => GetComponent<AudioSource>().PlayOneShot(sndPowerUp, 0.5f);
}

