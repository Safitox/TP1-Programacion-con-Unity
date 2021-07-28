using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner1 : MonoBehaviour
{
    // Start is called before the first frame update
    SceneManager1 SCMAN;
    [SerializeField]
    Transform anclajeEnemigo;
    public bool Activo = true;
    void Start()
    {
    }
    private void Awake()
    {
        SCMAN = GameObject.Find("SceneManager").GetComponent<SceneManager1>();
        GetComponent<ParticleSystem>().Stop();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Anclar(Transform enemigo)
    {

        enemigo.position = anclajeEnemigo.position;
        enemigo.rotation = anclajeEnemigo.rotation;
        enemigo.SetParent(anclajeEnemigo);
        GetComponent<Animator>().SetTrigger("AbrirTapa");

    }
    public void Desanclar()
    {
        if (anclajeEnemigo.GetChild(0))
        {
            anclajeEnemigo.GetChild(0).GetComponent<Robot1>().Activar();

         //   anclajeEnemigo.GetChild(0).SetParent(null);
         //   anclajeEnemigo.GetChild(0).SetParent(null);
        }



    }

    public void Colisionado(object[] pars)
    {
      //  Debug.Log(pars[0].ToString());
     if (pars[0].ToString() == "exploder" && Activo)
        {
            SCMAN.SpawnerDestruido();
            GetComponent<ParticleSystem>().Play() ;
           
            Activo = false;
        }
    }


}
