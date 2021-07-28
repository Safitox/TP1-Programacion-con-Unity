using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class capturaTrigger : MonoBehaviour
{
    [SerializeField] float Multiplicador = 1f;


    private void OnTriggerEnter(Collider other)
    {
        object[] pars = new object[3];
        pars[0] = other.gameObject.tag;
        pars[1] = Multiplicador;

        gameObject.SendMessageUpwards("Colisionado", pars);

    }
}
