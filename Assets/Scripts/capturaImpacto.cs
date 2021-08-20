using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class capturaImpacto : MonoBehaviour
{
    [SerializeField] float Multiplicador = 1f;


    private void OnCollisionEnter(Collision collision)
    {
        object[] pars = new object[3];
        pars[0] = collision.gameObject.tag ;
        pars[1] = Multiplicador;
        gameObject.SendMessageUpwards( "Colisionado", pars, SendMessageOptions.DontRequireReceiver);
    }
}
