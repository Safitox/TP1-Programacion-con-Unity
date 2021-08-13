using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager2 : MonoBehaviour
{
    [Header("GameObjects")]
    [SerializeField] Transform poolBalas;
    public List<GameObject> Bombas = new List<GameObject>();
    [SerializeField] Transform Jugador;
    [SerializeField] Transform spawnJugador;

    [SerializeField] Transform rotador;
    public GameObject proximabomba;
    GameObject bombaAway;
    List<GameObject> balasEnemigos = new List<GameObject>();
    [SerializeField]
    GameObject pf_balaEnemigo;
    [SerializeField] GameObject salida;

    [Header("Variables")]
    bool blRotar = false;
    bool blSalvadora = false;
    int stage=0;
    private bool blGameOn = false;

    [SerializeField]
    Transform spawnTutorial;
    [SerializeField] GameObject goTutorial;


    void Start()
    {
        Bombas.AddRange(GameObject.FindGameObjectsWithTag("bombarotadora"));
        Bombas.Add(GameObject.FindGameObjectWithTag("salvadora"));
     //   ConmutarRotacion(false);
        salida.SetActive(false);
        EstadoTutorial(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (blRotar)
            rotador.Rotate(0,  15 * Time.deltaTime,0); 
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

    private void Respawn()
    {
        SpawnearJugador(spawnJugador.position);
    }
    private void EstadoGame(bool OnOff)
    {
        if (OnOff)
        {
            Invoke("SoltarBomba", 3f);
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



    void ConmutarRotacion(bool Activar)
    {
        blRotar = Activar;
        foreach( GameObject go in Bombas)
        {
            if (go.activeInHierarchy)
            {
                if (go.CompareTag("salvadora"))
                {
                    go.GetComponent<Rigidbody>().isKinematic = Activar;
                 }
                else if (!go.GetComponent<BombaRotadora>().Activa)
                    go.GetComponent<Rigidbody>().isKinematic = Activar;
            }

        }
    }


    public void HayBomba(GameObject cual, bool salvadora = false)
    {
        blSalvadora = salvadora;
        proximabomba = cual;
        if (stage==0)
            SoltarBomba();        
    }

    void SoltarBomba()
    {
        if (!blGameOn)
            return;
        switch (stage)
        {
            case 0:
                ConmutarRotacion(false);
                bombaAway = proximabomba;
                bombaAway.GetComponent<Rigidbody>().isKinematic = false;
                bombaAway.GetComponent<SphereCollider>().enabled = false;
                bombaAway.transform.SetParent(null);
                stage++;
                Invoke("SoltarBomba", 0.7f);
                break;
            case 1:
                bombaAway.GetComponent<SphereCollider>().enabled = true;
                if (!blSalvadora)
                {
                    bombaAway.GetComponent<BombaRotadora>().target = Jugador;
                    bombaAway.GetComponent<BombaRotadora>().Activa = true;
//                    bombaAway.GetComponent<BombaRotadora>().SCMAN = this;
                }
                stage++;
                Invoke("SoltarBomba", 1f);
                break;
            case 2:
                bombaAway = null;
                ConmutarRotacion(true);
                stage = 0;
                if (proximabomba != null)
                    Invoke("SoltarBomba", 7f);
                break;
        }
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

    public void MurioElJugador()
    {
    }

    public void SpawnearJugador(Vector3 pos)
    {
        Jugador.position = pos;

    }

    public void ActivarSalida()
    {
        salida.SetActive(true);
    }

}
