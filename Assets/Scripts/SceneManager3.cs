using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager3 : MonoBehaviour
{
    [Header("GameObjects")]
    public Transform poolRobots1;
    Transform poolBalas;
    List<GameObject> robots1 = new List<GameObject>();
    public List<Transform> spawners = new List<Transform>();
    List<GameObject> balasEnemigos = new List<GameObject>();
    //[SerializeField]
//    bool blSpawnersActivos = true;
    [SerializeField]
    Transform spawnJugador;
    [SerializeField] Transform centro;
    [SerializeField] Transform Lava;
    [SerializeField] GameObject goTutorial;


    Transform jugador;

    [Header("Variables")]
    private bool blGameOn = false;

    [Header("prefabs/materiales")]
    [SerializeField] 
    GameObject pf_robot1;
    [SerializeField] GameObject pf_balaEnemigo;



    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject go in  GameObject.FindGameObjectsWithTag("spawner"))
        {
            spawners.Add(go.transform);
        }
        jugador = GameManager.Instance.jugador;
        poolBalas = GameObject.Find("poolBalasJugador").transform;
        EstadoTutorial(true);
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
        GameManager.Instance.onPlayerdied -= MurioElJugador;

    }
    private void Respawn() => SpawnearJugador();
    

    private void EstadoGame(bool OnOff)
    {
        if (OnOff)
        {
            Invoke("SpawnearUno", 10f);
            SpawnearJugador();
        }
        blGameOn = OnOff;
    }

    private void EstadoTutorial(bool OnOff)
    {
        goTutorial.SetActive(OnOff);
        if (OnOff)
        {
            SpawnearJugador();
        }
    }


    public void SpawnearUno()
    {
            ActivarNuevoRobot();
            Invoke("SpawnearUno", Random.Range(7f, 12f));
    }



    void ActivarNuevoRobot()
    {
        float maxdist = 100000;
        int spSeleccionado = 0;
        for(int i=0; i<spawners.Count;i++)
        {
                if ((spawners[i].transform.position - jugador.position).sqrMagnitude < maxdist)
                {
                    spSeleccionado = i;
                    maxdist = (spawners[i].transform.position - jugador.position).sqrMagnitude;
                }

        }
        GameObject enemigo = GenerarRobot1(true);
        enemigo.SetActive(true);
        enemigo.GetComponent<Navecita>().Activar();
        enemigo.transform.position = spawners[spSeleccionado].position;
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

        GameObject go = Instantiate(pf_robot1);
        go.GetComponent<Navecita>().Jugador = jugador;
        go.GetComponent<Navecita>().centro = centro;
        go.SetActive(Activar);
        robots1.Add( go);
        return go;
    }



    public void ActivarNuevoRobotDestruido(Transform original) => GameManager.Instance.Explosion (original.position);

    public GameObject GenerarBalaEnemiga()
    {
        foreach (GameObject ro in balasEnemigos)
        {
            if (!ro.activeInHierarchy)
            {
                return ro;
            }
        }
        GameObject go = Instantiate(pf_balaEnemigo, poolBalas);

        balasEnemigos.Add(go);
        return go;
    }



    public void MurioElJugador()
    {
        foreach (GameObject go in robots1)
        {
            if (go.activeInHierarchy)
            {
                go.GetComponent<Navecita>().blDisparar = false;
                go.GetComponent<Navecita>().blmovible = false;
                go.SetActive(false);
            }

        }
        Lava.GetComponent<Lava>().Reiniciar();
    }

    public void SpawnearJugador() => jugador.position = spawnJugador.position;

    


}
