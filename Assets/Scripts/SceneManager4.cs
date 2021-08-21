using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager4 : MonoBehaviour
{
    [Header("GameObjects")]
    public Transform poolRobots1;
    Transform poolBalas;
    List<GameObject> robots1 = new List<GameObject>();
    List<GameObject> robots3 = new List<GameObject>();
    List<GameObject> poolCajas = new List<GameObject>();
    [SerializeField] List<Transform> spawners = new List<Transform>();
    [SerializeField] GameObject goTutorial;

    [SerializeField]
    Transform spawnJugador;
    [SerializeField] Transform Spawner;
    public static SceneManager4 SM;

    Transform jugador;
    private bool playing = false;
    public bool MechaShrekOn = false;

    [Header("Variables")]


    [Header("prefabs/materiales")]
    [SerializeField] 
    GameObject pf_enemigo;
    [SerializeField]
    GameObject pf_robot3;
    [SerializeField]
    GameObject pf_cajas;

    void Start()
    {
        if (SM != null)
            GameObject.Destroy(SM);
        else
            SM = this;
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
        goTutorial.SetActive(OnOff);
        if (!OnOff)
        {
            SpawnearUno();
            Respawn();
        }
    }

    void EstadoGame(bool OnOff)
    {
        playing = OnOff;
        
            
    }

    public void SpawnearUno()
    {
            if (MechaShrekOn)
                ActivarNuevoRobot();
            ActivarNuevoRobot3();
        Invoke("SpawnearUno", Random.Range(7f, 12f));
    }



    void ActivarNuevoRobot()
    {
        GameObject enemigo = GenerarRobot1(true);
        enemigo.SetActive(true);
        enemigo.transform.position = Spawner.position;
    }


    void ActivarNuevoRobot3()
    {
        float maxdist = 0;
        int spSeleccionado = 0;
        for (int i = 0; i < spawners.Count; i++)
        {
                if ((spawners[i].transform.position - jugador.position).sqrMagnitude > maxdist)
                {
                    spSeleccionado = i;
                    maxdist = (spawners[i].transform.position - jugador.position).sqrMagnitude;
                }

        }
        GameObject enemigo = GenerarRobot3(true);
        enemigo.transform.position = spawners[spSeleccionado].position;
        enemigo.SetActive(true);

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

    public GameObject GenerarRobot3(bool Activar = true)
    {
        foreach (GameObject ro in robots3)
        {
            if (!ro.activeInHierarchy)
            {
                return ro;
            }
        }

        GameObject go = Instantiate(pf_robot3);
        go.SetActive(Activar);
        robots3.Add(go);
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
        if (!GameManager.Instance.blTutorial)
            GameManager.Instance.stockMisiles = 3;

    }


}
