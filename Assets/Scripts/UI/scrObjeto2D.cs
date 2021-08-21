using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrObjeto2D : MonoBehaviour
{
    Transform jugador;
    private bool visible = false;
    void Start()
    {
        jugador = GameManager.Instance.jugador;
    }

    private void FixedUpdate()
    {

        if (!jugador)
            return;
        Visibilidad((transform.position - jugador.position).sqrMagnitude <= 100);
            Vector3 targetPostition = new Vector3(jugador.position.x,
                        this.transform.position.y,
                        jugador.position.z);
            this.transform.LookAt(targetPostition);
        }

    void Visibilidad(bool estado)
    {
        if (visible != estado)
        {
            try
            {
                transform.GetChild(0).GetComponent<Animator>().SetBool("aparecer", estado);
            }
            catch
            {
            }
            visible = estado;
        }

    }
}
