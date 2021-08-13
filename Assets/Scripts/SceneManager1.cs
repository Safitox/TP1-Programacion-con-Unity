using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SceneManager1 : MonoBehaviour
{
    [Header("GameObjects")]
    public Transform poolRobots1;
    public Transform poolRobotsDestruidos1;
    Transform poolBalas;
    List<GameObject> robots1 = new List<GameObject>();
    List<GameObject> robotsDestruidos1 = new List<GameObject>();
    List<GameObject> cabezasRobot1 = new List<GameObject>();
    public List<Spawner1> spawners = new List<Spawner1>();
    List<GameObject> balasEnemigos = new List<GameObject>();
    [SerializeField]
    MeshRenderer[] Indicadores;
    [SerializeField] GameObject puerta;
    //bool blSpawnersActivos = true;
    [SerializeField]
    Transform spawnJugador;
    [SerializeField]
    Transform spawnTutorial;
    [SerializeField] GameObject goTutorial;


    Transform jugador;
    [Header("Referencias")]

    [Header("Variables")]
    public int ChancesCabezaBuena = 20;
    public int SpawnersRestantes = 0;
    private bool blGameOn=false;

    [Header("prefabs/materiales")]
    [SerializeField] 
    GameObject pf_robot1;
    [SerializeField]
    GameObject pf_robotDestruido1;
    [SerializeField]
    GameObject pf_cabezaRobot1;
    [SerializeField] GameObject pf_balaEnemigo;
    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject go in  GameObject.FindGameObjectsWithTag("spawner"))
        {
            spawners.Add(go.GetComponent<Spawner1>());
            SpawnersRestantes++;
            Indicadores[SpawnersRestantes - 1].material.color = Color.red;
        }
        jugador = GameObject.FindGameObjectWithTag("Player").transform;
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

    private void EstadoGame(bool OnOff)
    {
        if (OnOff)
        {
            Invoke("SpawnearUno", 3f);
            Respawn();
        }
        blGameOn = OnOff;
    }

    private void EstadoTutorial(bool OnOff)
    {
        goTutorial.SetActive(OnOff);
        if (OnOff)
        {
            SpawnearJugador(spawnTutorial.position);
        }
    }

    private void Respawn()
    {
        SpawnearJugador(spawnJugador.position);
    }


    public void SpawnearUno()
    {
        if (SpawnersRestantes != 0 && blGameOn)
        {
            ActivarNuevoRobot();
        }
        Invoke("SpawnearUno", Random.Range(7f, 12f));
    }



    void ActivarNuevoRobot()
    {
        float maxdist = 0;
        int spSeleccionado = 0;
        for(int i=0; i<spawners.Count;i++)
        {
            if (spawners[i].Activo)
            {
                if ((spawners[i].transform.position - jugador.position).sqrMagnitude > maxdist)
                {
                    spSeleccionado = i;
                    maxdist = (spawners[i].transform.position - jugador.position).sqrMagnitude;
                }
            }

        }
        GameObject enemigo = GenerarRobot1(true);
        enemigo.SetActive(true);
        //enemigo.GetComponent<Animator>().SetTrigger("Desactivar");
        spawners[spSeleccionado].Anclar(enemigo.transform);
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
        go.GetComponent<Robot1>().Jugador = jugador;
        go.SetActive(Activar);
        robots1.Add( go);
        return go;
    }



    public void ActivarNuevoRobotDestruido(Transform original)
    {
        GameObject rdes = GenerarRobotDestruido();
        rdes.transform.position = original.position;
        rdes.transform.rotation = original.rotation;
        rdes.SetActive(true);
        GameManager.Instance.Explosion (original.position);
        rdes = CabezaRobot1();
        rdes.transform.position = original.position;
        rdes.SetActive(true);
        if (Random.Range(0,ChancesCabezaBuena)==1)
        {
            rdes.GetComponent<cabezaBomba>().Iniciar(10f);
        
        }
        else
            rdes.GetComponent<cabezaBomba>().Iniciar(Random.Range(1f,2f));


    }



    GameObject GenerarRobotDestruido()
    {
        foreach (GameObject ro in robotsDestruidos1)
        {
            if (!ro.activeInHierarchy)
            {
                return ro;
            }
        }
        GameObject go = Instantiate(pf_robotDestruido1, poolRobotsDestruidos1);
        robotsDestruidos1.Add(go);
        return go;
    }



    GameObject CabezaRobot1()
    {
        foreach (GameObject ro in cabezasRobot1)
        {
            if (!ro.activeInHierarchy)
            {
                return ro;
            }
        }
        GameObject go = Instantiate(pf_cabezaRobot1, poolRobotsDestruidos1);
        cabezasRobot1.Add(go);
        return go;
    }


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


    public void SpawnerDestruido()
    {
        Indicadores[SpawnersRestantes - 1].material.color = Color.green;
        SpawnersRestantes -= 1;
        if (SpawnersRestantes == 0)
            puerta.GetComponent<Animator>().SetTrigger("Abrir");
    }

    public void MurioElJugador()
    {
        foreach (GameObject go in robots1)
        {
            go.GetComponent<Robot1>().blDisparar = false;
            go.GetComponent<Robot1>().blmovible  = false;
            go.SetActive(false);

        }
//        blSpawnersActivos = false;
    }

    public void SpawnearJugador(Vector3 pos)
    {
        jugador.position = pos;

    }


}
