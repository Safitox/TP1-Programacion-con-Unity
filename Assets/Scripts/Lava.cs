using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{
    Vector3 posInicial;
    [SerializeField] Vector3 velAscenso;
    // Start is called before the first frame update
    void Start()
    {
        posInicial = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GameManager.Instance.blGameOn)
            transform.position += velAscenso * Time.deltaTime   ;
        
    }
   

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            GameObject.Find("GameManager").GetComponent<GameManager>().JugadorTocado(1000);

        }


    }

    public void Reiniciar()=>transform.position = posInicial;

}
