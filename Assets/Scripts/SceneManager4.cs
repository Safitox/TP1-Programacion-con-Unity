using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager4 : MonoBehaviour
{
    [Header("GameObjects")]
    public Transform poolRobots1;
    Transform poolBalas;
    List<GameObject> robots1 = new List<GameObject>();
    List<GameObject> poolCajas = new List<GameObject>();
    [SerializeField]
    Transform spawnJugador;
    [SerializeField] Transform Spawner;
    public static SceneManager4 SM;
    [SerializeField] GameObject Victoria;



    Transform jugador;

    [Header("Variables")]


    [Header("prefabs/materiales")]
    [SerializeField] 
    GameObject pf_enemigo;
    [SerializeField]
    GameObject pf_cajas;

    void Start()
    {
        if (SM != null)
            GameObject.Destroy(SM);
        else
            SM = this;
        Victoria.SetActive(false);
        jugador = GameManager.Instance.jugador;
        poolBalas = GameObject.Find("poolBalasJugador").transform;
    }

    private void OnEnable()
    {
        GameManager.Instance.OnCambioEstadoTutorial += EstadoTutorial;
        GameManager.Instance.OnCambioEstadoGame += EstadoGame;
        GameManager.Instance.onRespawn += Respawn;
        GameManager.Instance.onPlayerdied += MurioElJugador;

    }

    private void OnDisable()
    {
        GameManager.Instance.OnCambioEstadoTutorial -= EstadoTutorial;
        GameManager.Instance.OnCambioEstadoGame -= EstadoGame;
        GameManager.Instance.onRespawn -= Respawn;
        GameManager.Instance.onPlayerdied += MurioElJugador;

    }

    void EstadoTutorial(bool OnOff)
    {

    }

    void EstadoGame(bool OnOff)
    {

    }

    public void SpawnearUno()
    {
        if (Spawner.gameObject.activeInHierarchy)
            ActivarNuevoRobot();
        Invoke("SpawnearUno", Random.Range(7f, 12f));
    }



    void ActivarNuevoRobot()
    {
        GameObject enemigo = GenerarRobot1(true);
        enemigo.SetActive(true);
        enemigo.transform.position = Spawner.position;
    }



    public GameObject GenerarRobot1( bool Activar=true)
    {
        foreach(GameObject ro in robots1)
        {
            if (!ro.activeInHierarchy)
            {
                return ro;
            }
        }

        GameObject go = Instantiate(pf_enemigo);
        go.GetComponent<BombaRotadora>().target = jugador;
        go.SetActive(Activar);
        go.GetComponent<BombaRotadora>().Activa = true;
        go.GetComponent<BombaRotadora>().Vida = 4;
        robots1.Add( go);
        return go;
    }


    public GameObject GenerarCaja()
    {
        foreach (GameObject ro in poolCajas)
        {
            if (!ro.activeInHierarchy)
            {
                return ro;
            }
        }

        GameObject go = Instantiate(pf_cajas);
        poolCajas.Add(go);
        return go;
    }



    public void ActivarNuevoRobotDestruido(Transform original) => GameManager.Instance.Explosion (original.position);

    public void MurioElJugador()
    {
            //reservado
    }

    public void Respawn()
    {
        jugador.position = spawnJugador.position;
        GameManager.Instance.stockMisiles = 3;

    }

    public void Victorioso()
    {
        Victoria.SetActive(true);
        GameManager.Instance.VidaJugador = 50000;
        Time.timeScale = 0;
    }


}
